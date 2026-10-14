using CommunityToolkit.Mvvm.Input;
using Orders_System_c__sql.Services;
using OrdersSystemSql;
using System.Collections.ObjectModel;

namespace Orders_System_c__sql.ViewModel
{
    public class MainViewModel
    {
        private readonly IWindowService windowService;

        public IRelayCommand AddNewCustomerCommand { get; }
        public IRelayCommand AddNewProductCommand { get; }
        public IRelayCommand CreateNewOrderCommand { get; }
        public IRelayCommand ViewAllOrdersCommand { get; }


        public ObservableCollection<Order> Orders { get; } = new();


        public MainViewModel(IWindowService windowService)
        {
            this.windowService = windowService;

            AddNewCustomerCommand = new RelayCommand(windowService.OpenAddNewCustomer);
            AddNewProductCommand = new RelayCommand(windowService.OpenAddNewProduct);
            CreateNewOrderCommand = new RelayCommand(windowService.OpenCreateNewOrder);
            ViewAllOrdersCommand = new RelayCommand(windowService.OpenViewAllOrders);

            Orders = new ObservableCollection<Order>();
        }
    }
}
