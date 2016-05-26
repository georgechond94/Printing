using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using System.Timers;
using printing.Comparers;

namespace printing.Models
{
    //TODO: CHANGE SECONDS TO MINUTES
    public class PrintingQueue : SortedSet<PrintViewModel>
    {
        public bool IsPrinting { get; set; }
        private Timer timer;
        private Stopwatch sw;

        public PrintViewModel CurrentPrint { get; set; }
        public PrintingQueue(IEnumerable<PrintViewModel> collection, IComparer<PrintViewModel> comparer) : base(collection, comparer)
        {
            if (this.Count>0)
            {
                StartPrinting();

            }
            else
            {
                
            }
        }

        ~PrintingQueue()
        {
            sw?.Stop();
            timer?.Dispose();
            IsPrinting = false; 
            timer = null;

        }

        public void AddPrint(PrintViewModel print)
        {
            this.Add(print);

            if (timer == null)
            {
                StartPrinting();
            }
            
            //timer = new Timer()
        }

        private void StartPrinting()
        {
            CurrentPrint = this.First();
            CurrentPrint.StartedDate = DateTime.Now;
            
            UpdatePrintInDb(CurrentPrint);

            timer = new Timer(TimeSpan.FromSeconds(CurrentPrint.Mass * 10).TotalMilliseconds);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = false;
            timer.Start();
            sw = Stopwatch.StartNew();
            IsPrinting = true;
            //timer = new Timer(new TimerCallback(Target), null, TimeSpan.FromSeconds(CurrentPrint.Mass * 10), TimeSpan.FromSeconds(CurrentPrint.Mass * 10));

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();


            this.Remove(CurrentPrint);

            CurrentPrint.Finished = true;
            
            UpdatePrintInDb(CurrentPrint);
            

            if (this.Count > 0)
            {
                StartPrinting();


            }
            else
            {
                sw = null;
                timer.Dispose();
                timer = null;
            }
        }

        /// <summary>
        /// Returns the index of a print in the queue.
        /// </summary>
        /// <param name="print">The print to search for.</param>
        /// <returns>The index.</returns>
        public int IndexOf(PrintViewModel print)
        {
            int i = 0;
            foreach (var p in this)
            {
                if (p.ID == print.ID)
                    return i;
                i++;
            }
            return -1;
        }

        public int RemainingTimeOfCurrentPrint()
        {
            return (CurrentPrint.Mass*10) - (int) sw.Elapsed.TotalSeconds;
        }
        /// <summary>
        /// Returns the time in minutes for a print to start.
        /// </summary>
        /// <param name="print">The print to search for.</param>
        /// <returns>The wait time in minutes.</returns>
        public int WaitTimeOf(PrintViewModel print)
        {

            int min = 0;

            min += RemainingTimeOfCurrentPrint();

            min += this.Skip(1).TakeWhile(p => p.ID != print.ID).Sum(p => p.Mass*10);



            return min;
        }

        public void ContinuePrint(PrintViewModel print)
        {
            if (print != null && print.Stopped)
            {
                print.Stopped = false;
                UpdatePrintInDb(print);

                AddPrint(print);
            }
        }

        public void DeletePrint(PrintViewModel print)
        {
            if (print != null && CurrentPrint.ID != print.ID)
            {
                this.Remove(print);

                DeletePrintFromDb(print);
            }
        }

        public void PausePrint(PrintViewModel print)
        {
            if (print != null && CurrentPrint.ID != print.ID)
            {
                this.Remove(print);

                print.Stopped = true;
                UpdatePrintInDb(print);

            }
        }
        public int RemainingPrintsForUserPrints(IEnumerable<PrintViewModel> prints)
        {
            if (prints!=null)
                return this.Count(p => prints.Contains(p,new PrintEqualityComparer()));
            return 0;
        }

        private void DeletePrintFromDb(PrintViewModel pvm)
        {
            using (ApplicationDbContext con = new ApplicationDbContext())
            {

                con.Prints.Attach(pvm);
                con.Prints.Remove(pvm);
                con.SaveChanges();
            }
        }
        private void UpdatePrintInDb(PrintViewModel pvm)
        {
            using (ApplicationDbContext con = new ApplicationDbContext())
            {

                con.Prints.Attach(pvm);
                con.Entry(pvm).State = EntityState.Modified;
                con.SaveChanges();
            }
        }

    }

}