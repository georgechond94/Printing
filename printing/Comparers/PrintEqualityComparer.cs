using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using printing.Models;

namespace printing.Comparers
{
    public class PrintEqualityComparer : IEqualityComparer<PrintViewModel>
    {
        public bool Equals(PrintViewModel x, PrintViewModel y)
        {
            return x?.ID == y?.ID;
        }

        public int GetHashCode(PrintViewModel obj)
        {
            return obj.ID ^ obj.Mass;
        }
    }
}