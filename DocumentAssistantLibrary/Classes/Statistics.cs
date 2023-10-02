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
                DocumentCount = 0;
                MediumDocSize = 0;
                AverageTimeToCompleteDoc = 0;
                return;
            }

            IsValid = true;
            AverageTimeToCompleteDoc = CalculateAverageDaysToCompleteDoc(inputList);
            DocumentCount = inputList.Count;
            MediumDocSize = inputList.Average(x => x.SignsSize) is null ? 0 : (double)inputList.Average(x => x.SignsSize);
        }

        public readonly bool IsValid;

        public readonly int DocumentCount;

        public readonly double? AverageTimeToCompleteDoc;

        public readonly double MediumDocSize;


        private static double? CalculateAverageDaysToCompleteDoc(IList<Document> inputList)
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
                return Math.Round((double)averageDaysToCompleteDoc, 2);
            else
                return null;
        }
    }
}
