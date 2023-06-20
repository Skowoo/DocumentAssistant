﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfApp.Models;

namespace WpfApp.Classes
{
    public static class RandomDocGenerator
    {
        public static void GenerateExampleDocuments(int numberOfDocs)
        {
            Random rnd = new Random();

            CheckAndAddCustomers();
            CheckAndAddDocTypes();

            for (int i = 0; i < numberOfDocs; i++)
            {
                int langID = rnd.Next(1, 4);

                Models.Document tempDoc = new Models.Document
                {
                    Name = $"ExampleDoc{i + 1}",
                    SignsSize = rnd.Next(100, 10000),
                    TimeAdded = DateTime.Now,
                    Deadline = DateTime.Now.AddDays(rnd.Next(2, 30)),
                    IsConfirmed = false,
                    TypeID = rnd.Next(1, 5),
                    CustomerID = rnd.Next(1, 5),
                    OriginalLanguageID = langID,
                    TargetLanguageID = langID + 1
                };

                using (MainContext context = new MainContext())
                {
                    context.Documents.Add(tempDoc);
                    context.SaveChanges();
                }
            }
        }

        private static void CheckAndAddCustomers()
        {
            int customersCount = 0;
            using (MainContext context = new MainContext())
            {
                customersCount = context.Customers.Count();
            }

            if (customersCount >= 5) return;

            for (int i = 0; i < 5 - customersCount; i++)
            {
                using (MainContext context = new MainContext())
                {
                    context.Customers.Add(new Customer { CustomerName = $"ExampleCustomer{i}" } );
                    context.SaveChanges();
                }
            }
        }

        private static void CheckAndAddDocTypes()
        {
            int typesCount = 0;
            using (MainContext context = new MainContext())
            {
                typesCount = context.DocumentTypes.Count();
            }

            if (typesCount >= 5) return;

            for (int i = 0; i < 5 - typesCount; i++)
            {
                using (MainContext context = new MainContext())
                {
                    context.DocumentTypes.Add(new DocumentType { TypeName = $"ExampleType{i}" });
                    context.SaveChanges();
                }
            }
        }
    }
}
