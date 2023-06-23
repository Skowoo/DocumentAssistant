namespace DocumentAssistantLibrary.Models.ViewModels
{
    /// <summary>
    /// View model for Customer entity in app's database
    /// </summary>
    public class CustomerViewModel
    {
        /// <summary>
        /// Constructor for view model
        /// </summary>
        /// <param name="input"> Customer on which model should be based </param>
        public CustomerViewModel(Customer input)
        {
            this.CustomerID = input.CustomerID;
            this.CustomerName = input.CustomerName;
        }

        /// <summary>
        /// Customer name
        /// </summary>
        public string CustomerName { get; init; }

        /// <summary>
        /// Customer unique ID
        /// </summary>
        public int CustomerID { get; init; }

        /// <summary>
        /// ToString method
        /// </summary>
        /// <returns> string consisting of Customer's name </returns>
        public override string ToString() => CustomerName;
    }
}
