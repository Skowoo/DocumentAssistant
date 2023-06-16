﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Models.ViewModels
{
    class CustomerViewModel
    {
        public CustomerViewModel(Customer input)
        {
            this.CustomerID = input.CustomerID;
            this.CustomerName = input.CustomerName;
        }

        public string CustomerName { get; init; }

        public int CustomerID { get; init; }

        public override string ToString() => CustomerName;
    }
}
