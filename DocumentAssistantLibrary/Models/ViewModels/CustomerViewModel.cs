namespace DocumentAssistantLibrary.Models.ViewModels
{
    public class CustomerViewModel
    {
        public CustomerViewModel(Customer input)
        {
            this.CustomerID = input.CustomerID;
            this.CustomerName = input.CustomerName;
        }

        public string CustomerName { get; init; }

        public int CustomerID { get; init; }

        public override string ToString() => CustomerName;
    }
}
