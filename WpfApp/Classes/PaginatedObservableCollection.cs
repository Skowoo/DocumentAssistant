using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WpfApp.Classes
{
    public class PaginatedObservableCollection<T> : ObservableCollection<T>
    {
        public ObservableCollection<T> PageView { get; private set; }

        public int TotalPages { get; private set; }

        public int CurrentPage { get; private set; }

        public bool NextPageAvailable => CurrentPage < TotalPages;

        public bool PreviousPageAvailable => CurrentPage > 1;

        private int pageSize;

        public PaginatedObservableCollection(IList<T> values = null, int pageSize = 30)
        {
            PageView = new ObservableCollection<T>();
            if (values is null)
            {
                TotalPages = 1;
                this.pageSize = pageSize;
                return;
            }

            foreach (T item in values)
                this.Add(item);
            this.pageSize = pageSize;
            TotalPages = (int)Math.Ceiling(values.Count / (double)pageSize);
            CurrentPage = 1;
        }

        public void NextPage()
        {
            if (!NextPageAvailable) return;
            CurrentPage++;
            UpdatePageCollection();
        }

        public void PreviousPage()
        {
            if (!NextPageAvailable) return;
            CurrentPage--;
            UpdatePageCollection();
        }

        private void UpdatePageCollection()
        {
            List<T> itemsOnPage = this.Skip((CurrentPage - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToList();
            PageView.Clear();
            itemsOnPage.ForEach(item => PageView.Add(item));
        }
    }
}
