namespace DocumentAssistantLibrary.Models.ViewModels
{
    public class DocumentTypeViewModel
    {
        public DocumentTypeViewModel(DocumentType input)
        {
            this.TypeID = input.TypeID;
            this.TypeName = input.TypeName;
        }

        public string TypeName { get; init; }

        public int TypeID { get; init; }

        public override string ToString() => TypeName;
    }
}
