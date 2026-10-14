using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Orders_System_c__sql.Data;
using Orders_System_c__sql.Models;
using Orders_System_c__sql.Services;
using OrdersSystemSql;
using System.Collections.ObjectModel;

namespace Orders_System_c__sql.ViewModel
{
    public partial class OrderItemViewModel : ObservableObject
    {
        private readonly int orderIdForItem;
        private readonly WindowService windowService;
        private readonly DataOrderItem data;
        private readonly OrderItemService itemsService;

        public ObservableCollection<OrderItem> CurrentOrderItems { get; }
        public ObservableCollection<Product> Products { get; }

        [ObservableProperty]
        private Product? selectedProduct;

        [ObservableProperty]
        private int quantity;

        [ObservableProperty]
        private int currentStock;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public IRelayCommand BackCommand { get; }
        public IRelayCommand SaveCommand { get; }

        public OrderItemViewModel(
            WindowService windowService,
            DataOrderItem data,
            OrderItemService itemsService,
            ProductViewModel productViewModel,
            int orderId)
        {
            this.windowService = windowService;
            this.data = data;
            this.itemsService = itemsService;

            orderIdForItem = orderId;

            CurrentOrderItems = itemsService.CurrentOrderItems;
            Products = productViewModel.Products;

            SaveCommand = new RelayCommand(SaveDataOrderItem);
            BackCommand = new RelayCommand(windowService.BackWindow);
        }

        partial void OnSelectedProductChanged(Product? value)
        {
            CurrentStock = value?.Stock ?? 0;
            ErrorMessage = string.Empty;
        }

        private void SaveDataOrderItem()
        {
            ErrorMessage = string.Empty;

            if (SelectedProduct == null)
            {
                ErrorMessage = "Пожалуйста, выберите товар из списка!";
                return;
            }

            if (Quantity <= 0)
            {
                ErrorMessage = "Введите положительное количество товара!";
                return;
            }

            if (SelectedProduct.Stock <= 0)
            {
                ErrorMessage = "Товар отсутствует на складе!";
                return;
            }

            if (Quantity > SelectedProduct.Stock)
            {
                ErrorMessage =
                    $"Превышен лимит склада! Доступно: {SelectedProduct.Stock}";
                return;
            }

            int oldStock = SelectedProduct.Stock;

            OrderItem orderItem = new OrderItem()
            {
                OrderId = orderIdForItem,
                ProductId = SelectedProduct.ProductId,
                Quantity = Quantity,
                Product = SelectedProduct
            };

            try
            {
                SelectedProduct.Stock -= Quantity;

                data.Save(orderItem, SelectedProduct);

                CurrentStock = SelectedProduct.Stock;
                CurrentOrderItems.Add(orderItem);

                Quantity = 0;
            }
            catch (Exception)
            {
                SelectedProduct.Stock = oldStock;
                CurrentStock = oldStock;

                ErrorMessage =
                    "Не удалось сохранить товар. Проверьте подключение к базе данных.";
            }
        }
    }
}