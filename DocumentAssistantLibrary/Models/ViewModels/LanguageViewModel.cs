namespace DocumentAssistantLibrary.Models.ViewModels
{
    public class LanguageViewModel
    {
        public LanguageViewModel(Language input)
        {
            LanguageID = input.LanguageID;
            LanguageName = input.LanguageName;
        }

        public int LanguageID { get; init; }

        public string LanguageName { get; init; }

        public override string ToString() => LanguageName;
    }
}
