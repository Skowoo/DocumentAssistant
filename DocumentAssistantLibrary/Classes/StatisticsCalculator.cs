using DocumentAssistantLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAssistantLibrary.Classes
{
    public class StatisticsCalculator
    {
        public static string GenerateStatsString(List<Document> inputList)
        {
            if (inputList is null || inputList.Count < 1)
                return "Brak dokumentów spełniających wybrane kryteria";

            StringBuilder output = new();

            output.AppendLine("Liczba dokumentów:");
            output.AppendLine(inputList.Count().ToString() + "\n");

            output.AppendLine("Średni rozmiar dokumentu:");
            output.AppendLine(Math.Floor((decimal)inputList.Average(x => x.SignsSize)).ToString() + "\n");

            output.AppendLine("Średni czas tłumaczenia (dni):");

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
                output.AppendLine($"{Math.Round((double)averageDaysToCompleteDoc, 2).ToString()} obliczony na podstawie {validDocsCounter} dokumentów");
            else
                output.AppendLine("Brak plików z określoną datą dodania i wykonania.");

            return output.ToString();
        }
    }
}
