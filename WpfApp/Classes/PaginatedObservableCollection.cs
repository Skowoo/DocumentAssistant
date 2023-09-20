using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WpfApp.Classes
{
    internal class PaginatedObservableCollection<T> : ObservableCollection<T>
    {
        public ObservableCollection<T> PageView { get; private set; }

        public int TotalPages { get; private set; }

        public int CurrentPage { get; private set; }

        public bool NextPageAvailable => CurrentPage < TotalPages;

        public bool PreviousPageAvailable => CurrentPage > 1;

        private int pageSize;

        public PaginatedObservableCollection(IList<T> values, int pageSize)
        {
            foreach (T item in values)
                this.Add(item);
            this.pageSize = pageSize;
            TotalPages = (int)Math.Ceiling(values.Count / (double)pageSize);
            CurrentPage = 1;
        }

        public void NextPage()
        {
            if (!NextPageAvailable) throw new ArgumentOutOfRangeException("Next page does not exist!");
            CurrentPage++;
            List<T> itemsOnPage = this.Skip((CurrentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            PageView = new ObservableCollection<T>(itemsOnPage);
        }

        public void PreviousPage()
        {
            if (!NextPageAvailable) throw new ArgumentOutOfRangeException("Previous page does not exist!");
            CurrentPage--;
            List<T> itemsOnPage = this.Skip((CurrentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            PageView = new ObservableCollection<T>(itemsOnPage);
        }
    }
}
