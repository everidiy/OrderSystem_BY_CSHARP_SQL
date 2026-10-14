using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Orders_System_c__sql.Data;
using Orders_System_c__sql.Models;
using Orders_System_c__sql.Services;
using OrdersSystemSql;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Orders_System_c__sql.ViewModel
{
    public partial class NewOrderViewModel : ObservableObject
    {
        private readonly DataNewOrder data;
        private readonly NewOrderService newOrderService;
        private readonly Action<int> openAddItemWindow;
        private readonly Action closeWindow;
        private int orderId;

        public ObservableCollection<OrderItem> CurrentOrderItems { get; }
        public ObservableCollection<Customer> Customers => CustomerService.Customers;
        public ObservableCollection<Product> Products => ProductService.Products;

        [ObservableProperty]
        private Customer? selectedCustomer;

        [ObservableProperty]
        private OrderItem? selectedOrderItem;

        [ObservableProperty]
        private string totalPrice = "0.00";

        public IRelayCommand AddItemCommand { get; }
        public IRelayCommand DeleteItemCommand { get; }
        public IRelayCommand SaveCommand { get; }
        public IRelayCommand BackCommand { get; }

        public NewOrderViewModel(
            DataNewOrder data,
            NewOrderService newOrderService,
            Action<int> openAddItemWindow,
            Action closeWindow)
        {
            this.data = data;
            this.newOrderService = newOrderService;
            this.openAddItemWindow = openAddItemWindow;
            this.closeWindow = closeWindow;

            CurrentOrderItems = newOrderService.CurrentOrderItems;

            AddItemCommand = new RelayCommand(AddItem);
            DeleteItemCommand = new RelayCommand(DeleteItem);
            SaveCommand = new RelayCommand(Save);
            BackCommand = new RelayCommand(closeWindow);

            CurrentOrderItems.CollectionChanged +=
                (_, _) => UpdateTotal();
        }

        private void AddItem()
        {
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Choose customer, please!");
                return;
            }

            try
            {
                if (orderId == 0)
                    orderId = data.Create(SelectedCustomer.CustomerId);

                openAddItemWindow(orderId);
                UpdateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to create order: {ex.Message}");
            }
        }

        private void DeleteItem()
        {
            if (SelectedOrderItem == null)
            {
                MessageBox.Show("Select an item to delete.");
                return;
            }

            if (orderId == 0)
            {
                MessageBox.Show("The order has not been created yet.");
                return;
            }

            try
            {
                var item = SelectedOrderItem;

                data.DeleteItem(orderId, item);

                var product = Products.FirstOrDefault(
                    p => p.ProductId == item.ProductId);

                if (product != null)
                    product.Stock += item.Quantity;

                CurrentOrderItems.Remove(item);
                SelectedOrderItem = null;

                UpdateTotal();

                MessageBox.Show("Item successfully deleted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to delete item: {ex.Message}");
            }
        }

        private void Save()
        {
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Choose customer, please!");
                return;
            }

            if (orderId == 0 || CurrentOrderItems.Count == 0)
            {
                MessageBox.Show("Add at least one product to the order.");
                return;
            }

            try
            {
                data.Save(orderId, SelectedCustomer.CustomerId);

                MessageBox.Show("Order successfully saved!");
                closeWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to save order: {ex.Message}");
            }
        }

        private void UpdateTotal()
        {
            double total = 0;

            foreach (var item in CurrentOrderItems)
            {
                var product = Products.FirstOrDefault(
                    p => p.ProductId == item.ProductId);

                if (product != null)
                    total += product.Price * item.Quantity;
            }

            TotalPrice = total.ToString("N2");
        }
    }
}