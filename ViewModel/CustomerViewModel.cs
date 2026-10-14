using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Orders_System_c__sql.Data;
using Orders_System_c__sql.Services;
using OrdersSystemSql;
using System.Collections.ObjectModel;

namespace Orders_System_c__sql.ViewModel
{
    public partial class CustomerViewModel : ObservableObject
    {
        private readonly IWindowService windowService;
        private readonly DataCustomer data;

        public ObservableCollection<Customer> Customers => CustomerService.Customers;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string phone = string.Empty;


        public IRelayCommand BackCommand { get; }
        public IRelayCommand SaveCommand { get; }

        public CustomerViewModel(IWindowService windowService, DataCustomer data)
        {
            this.windowService = windowService;
            this.data = data;

            BackCommand = new RelayCommand(windowService.BackWindow);
            SaveCommand = new RelayCommand(() => SaveDataCustomer());
        }

        private void SaveDataCustomer()
        {
            var customer = new Customer()
            {
                Name = Name,
                Email = Email,
                Phone = Phone
            };

            var data = new DataCustomer();
            data.Save(customer);

            Customers.Add(customer);

            Name = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
        }
    }
}