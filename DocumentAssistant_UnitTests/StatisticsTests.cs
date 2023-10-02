using DocumentAssistantLibrary.Classes;
using DocumentAssistantLibrary.Models;

namespace DocumentAssistant_UnitTests
{
    [TestClass]
    public class StatisticsTests
    {
        private static List<Document> GenerateExampleList()
        {
            return new()
            {
                new Document
                {
                    DocumentID = 1,
                    SignsSize = 1000,
                    TimeAdded = new DateTime(2022, 01, 01),
                    TimeDone = new DateTime(2022, 02, 01)
                },
                new Document
                {
                    DocumentID = 2,
                    SignsSize = 1000,
                    TimeAdded = new DateTime(2022, 01, 01),
                    TimeDone = new DateTime(2022, 02, 01)
                },
                new Document
                {
                    DocumentID = 3,
                    SignsSize = 1000,
                    TimeAdded = new DateTime(2022, 01, 01),
                    TimeDone = new DateTime(2022, 02, 01)
                },
                new Document
                {
                    DocumentID = 4,
                    SignsSize = 1000,
                    TimeAdded = new DateTime(2022, 01, 01),
                    TimeDone = new DateTime(2022, 02, 01)
                },
                new Document
                {
                    DocumentID = 5,
                    SignsSize = 1000,
                    TimeAdded = new DateTime(2022, 01, 01),
                    TimeDone = new DateTime(2022, 02, 01)
                },
                new Document
                {
                    DocumentID = 6,
                    SignsSize = 1000,
                    TimeAdded = new DateTime(2022, 01, 01),
                    TimeDone = new DateTime(2022, 02, 01)
                },
                new Document
                {
                    DocumentID = 7,
                    SignsSize = 1000,
                    TimeAdded = new DateTime(2022, 01, 01),
                    TimeDone = new DateTime(2022, 02, 01)
                },
                new Document
                {
                    DocumentID = 8,
                    SignsSize = 1000,
                    TimeAdded = new DateTime(2022, 01, 01),
                    TimeDone = new DateTime(2022, 02, 01)
                },
                new Document
                {
                    DocumentID = 9,
                    SignsSize = 1000,
                    TimeAdded = new DateTime(2022, 01, 01),
                    TimeDone = new DateTime(2022, 02, 01)
                },
                new Document
                {
                    DocumentID = 10,
                    SignsSize = 1000,
                    TimeAdded = new DateTime(2022, 01, 01),
                    TimeDone = new DateTime(2022, 02, 01)
                }
            };
        }

        [TestMethod]
        public void OK_Tests()
        {
            var testList = GenerateExampleList();
            var testStats = new Statistics(testList);

            Assert.IsTrue(testStats.IsValid == true);

            //Counter
            Assert.AreEqual(10, testStats.DocumentCount);

            //Average days to finish document
            Assert.AreEqual(31, testStats.AverageTimeToCompleteDoc);

            //Average size of document
            Assert.AreEqual(1000, testStats.AverageDocSize);

            //Translated documents
            Assert.AreEqual(10, testStats.TranslatedDocumentCount);
        }

        [TestMethod]
        public void EmptyTests()
        {
            List<Document> testList = new();
            var testStats = new Statistics(testList);

            Assert.IsTrue(testStats.IsValid == false);

            testList = null;

            testStats = new Statistics(testList);

            Assert.IsTrue(testStats.IsValid == false);
        }
    }
}