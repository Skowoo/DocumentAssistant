using DocumentAssistantLibrary.Models;


namespace DocumentAssistantLibrary.Classes
{
    public readonly struct Statistics 
    {
        public Statistics(IList<Document> inputList)
        {
            if (inputList is null || inputList.Count < 1)
            {
                IsValid = false;
                return;
            }

            IsValid = true;
            var calculatedTranslations = CalculateAverageDaysToCompleteDoc(inputList);
            AverageTimeToCompleteDoc = calculatedTranslations.Item1;
            TranslatedDocumentCount = calculatedTranslations.Item2;
            DocumentCount = inputList.Count;
            AverageDocSize = (int)Math.Floor((double)inputList.Average(x => x.SignsSize));
        }

        public readonly bool IsValid;

        public readonly int DocumentCount = 0;

        public readonly int TranslatedDocumentCount = 0;

        public readonly double? AverageTimeToCompleteDoc = 0;

        public readonly int AverageDocSize = 0;


        private static Tuple<double?, int> CalculateAverageDaysToCompleteDoc(IList<Document> inputList)
        {
            int validDocsCounter = 0;
            double? averageDaysToCompleteDoc = null;

            foreach (Document document in inputList)
            {
                if (document.TimeDone is null || document.TimeAdded is null)
                    continue;

                TimeSpan completionTime = (TimeSpan)(document.TimeDone - document.TimeAdded);

                if (averageDaysToCompleteDoc is null)
                    averageDaysToCompleteDoc = completionTime.Days;
                else
                    averageDaysToCompleteDoc += completionTime.Days;

                validDocsCounter++;
            }
            if (validDocsCounter > 0)
                return Tuple.Create((double?)Math.Round((double)averageDaysToCompleteDoc / validDocsCounter, 2), validDocsCounter);
            else
                return Tuple.Create((double?)null, 0);
        }
    }
}
