namespace DocumentAssistantLibrary.Models.ViewModels
{
    /// <summary>
    /// View model for document type
    /// </summary>
    public class DocumentTypeViewModel
    {
        /// <summary>
        /// Constructor for view model
        /// </summary>
        /// <param name="input"> Document type on which model should be based </param>
        public DocumentTypeViewModel(DocumentType input)
        {
            this.TypeID = input.TypeID;
            this.TypeName = input.TypeName;
        }

        /// <summary>
        /// Document type name
        /// </summary>
        public string TypeName { get; init; }

        /// <summary>
        /// Document type unique ID
        /// </summary>
        public int TypeID { get; init; }

        /// <summary>
        /// ToString method
        /// </summary>
        /// <returns> string consisting of Document type name </returns>
        public override string ToString() => TypeName;
    }
}
