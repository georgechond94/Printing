using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using printing.Enums;
using printing.Models;

namespace printing.Comparers
{
    public class PrintComparer : IComparer<PrintViewModel>
    {
        public int Compare(PrintViewModel x, PrintViewModel y)
        {
            if (x.ID == y.ID)
                return 0;
            if (x.ID == MvcApplication.Queue?.CurrentPrint?.ID)
                return -20;
            if (y.ID == MvcApplication.Queue?.CurrentPrint?.ID)
                return 20;

            if (x.PrintPriority == Priority.HIGH && y.PrintPriority == Priority.HIGH)
            {
                if (x.Mass < 6 && y.Mass < 6)
                {
                    return (x.PostedDate < y.PostedDate) ? -10 : 10;
                }
                if (x.Mass < 6)
                {
                    return -10;
                }
                if (y.Mass < 6)
                {
                    return 10;
                }
            }
            else if (x.PrintPriority == Priority.HIGH)
            {
                return -5;
            }
            else if (y.PrintPriority == Priority.HIGH)
            {
                return 5;
            }
            return (x.PostedDate < y.PostedDate) ? -1 : 1;
        }
    }
}