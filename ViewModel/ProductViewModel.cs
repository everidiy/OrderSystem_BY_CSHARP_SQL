using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Orders_System_c__sql.Data;
using Orders_System_c__sql.Models;
using Orders_System_c__sql.Services;
using System.Collections.ObjectModel;

namespace Orders_System_c__sql.ViewModel
{
    public partial class ProductViewModel : ObservableObject
    {
        private readonly IWindowService windowService;
        private readonly DataProduct data;

        public ObservableCollection<Product> Products => ProductService.Products;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private double price = 0;

        [ObservableProperty]
        private int quantity = 0;


        public IRelayCommand BackCommand { get; }
        public IRelayCommand SaveCommand { get; }

        public ProductViewModel(IWindowService windowService, DataProduct data)
        {
            this.windowService = windowService;
            this.data = data;

            BackCommand = new RelayCommand(windowService.BackWindow);
            SaveCommand = new RelayCommand(SaveDataProduct);
        }

        private void SaveDataProduct()
        {
            var product = new Product()
            {
                Name = Name,
                Price = Price,
                Stock = Quantity
            };

            data.Save(product);

            Products.Add(product);

            Name = string.Empty;
            Price = 0;
            Quantity = 0;
        }
    }
}
