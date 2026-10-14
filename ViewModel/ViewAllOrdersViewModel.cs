using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Orders_System_c__sql.Data;
using Orders_System_c__sql.Models;
using System.Collections.ObjectModel;

namespace Orders_System_c__sql.ViewModel
{
    public partial class ViewAllOrdersViewModel : ObservableObject
    {
        private readonly DataViewAllOrders data;
        private readonly Action backWindow;

        public ObservableCollection<OrderWithDetails> OrdersWithDetails
        { get; } = new ObservableCollection<OrderWithDetails>();

        public IRelayCommand BackCommand { get; }

        public ViewAllOrdersViewModel(
            DataViewAllOrders data,
            Action backWindow)
        {
            this.data = data;
            this.backWindow = backWindow;

            BackCommand = new RelayCommand(backWindow);

            GetAllRows();
        }

        private void GetAllRows()
        {
            OrdersWithDetails.Clear();

            foreach (var item in data.GetAllRows())
            {
                OrdersWithDetails.Add(item);
            }
        }
    }
}