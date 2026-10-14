namespace Orders_System_c__sql.Services
{
    public interface IWindowService
    {
        void OpenAddNewCustomer();
        void OpenAddNewProduct();
        void OpenCreateNewOrder();
        void OpenViewAllOrders();


        void BackWindow();
    }
}
