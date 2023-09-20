using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WpfApp.Classes
{
    public class PaginatedObservableCollection<T> : ObservableCollection<T>
    {
        public ObservableCollection<T> PageView = new();

        public int TotalPages = 5;

        public int CurrentPage { get; private set; }

        public int pageSize = 30;

        public bool NextPageAvailable => CurrentPage < TotalPages;

        public bool PreviousPageAvailable => CurrentPage > 1;

        //public PaginatedObservableCollection(IList<T> values = null, int pageSize = 30)
        //{
        //    PageView = new ObservableCollection<T>();
        //    CurrentPage = 1;

        //    if (values is null)
        //    {
        //        TotalPages = 1;
        //        this.pageSize = pageSize;
        //        return;
        //    }

        //    foreach (T item in values)
        //        this.Add(item);
        //    this.pageSize = pageSize;
        //    TotalPages = (int)Math.Ceiling(values.Count / (double)pageSize);
        //}

        public void NextPage()
        {
            if (!NextPageAvailable) return;
            CurrentPage++;
            UpdatePageCollection();
        }

        public void PreviousPage()
        {
            if (!PreviousPageAvailable) return;
            CurrentPage--;
            UpdatePageCollection();
        }

        private void UpdatePageCollection()
        {
            List<T> itemsOnPage = this.Skip((CurrentPage - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToList();
            PageView.Clear();
            PageView = new ObservableCollection<T>(itemsOnPage);
        }
    }
}
