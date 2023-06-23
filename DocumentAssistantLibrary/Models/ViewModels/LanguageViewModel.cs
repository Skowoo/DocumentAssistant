namespace DocumentAssistantLibrary.Models.ViewModels
{
    /// <summary>
    /// View model for Language entity in app's database
    /// </summary>
    public class LanguageViewModel
    {
        /// <summary>
        /// Constructor for view model
        /// </summary>
        /// <param name="input"> Language on which model should be based </param>
        public LanguageViewModel(Language input)
        {
            LanguageID = input.LanguageID;
            LanguageName = input.LanguageName;
        }

        /// <summary>
        /// Language unique ID
        /// </summary>
        public int LanguageID { get; init; }

        /// <summary>
        /// Language name
        /// </summary>
        public string LanguageName { get; init; }

        /// <summary>
        /// ToString method
        /// </summary>
        /// <returns> string consisting of language name </returns>
        public override string ToString() => LanguageName;
    }
}
