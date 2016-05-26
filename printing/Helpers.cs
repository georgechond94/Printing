using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using printing.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace printing
{
    public static class Helpers
    {
        public static PrintViewModel GetPrintById(int id)
        {
            using (ApplicationDbContext dbc = new ApplicationDbContext())
            {
                return dbc.Prints.Find(id);
            }
        }
        public static List<PrintViewModel> GetAllStoppedPrints()
        {
            using (ApplicationDbContext dbc = new ApplicationDbContext())
            {
                return dbc.Prints.Where(x => x.Stopped).ToList();
            }
        }
        public static List<PrintViewModel> GetAllPrints(bool? finished)
        {
            using (ApplicationDbContext dbc = new ApplicationDbContext())
            {
                return finished.HasValue ? dbc.Prints.Where(x => x.Finished == finished.Value).ToList() : dbc.Prints.ToList();

            }
        }
        public static async Task<PrintViewModel> GetUserPrintByIdAsync(string userId,int printId)
        {
            var findByIdAsync = await HttpContext.Current?.GetOwinContext()?
               .GetUserManager<ApplicationUserManager>()?
               .FindByIdAsync(userId);

            return findByIdAsync?.Prints.Find(x => x.ID == printId);
        }

        public static IEnumerable<PrintViewModel> GetUsersStoppedPrints(string userId)
        {
            var findByIdAsync = HttpContext.Current?.GetOwinContext()?
               .GetUserManager<ApplicationUserManager>()?
               .FindById(userId);

            return findByIdAsync.Prints.FindAll(x => x.Stopped && !x.Finished);

        }

        public static async Task<IEnumerable<PrintViewModel>> GetUsersPrintsAsync(string userId,bool? finished)
        {

            var findByIdAsync =  HttpContext.Current?.GetOwinContext()?
                .GetUserManager<ApplicationUserManager>()?
                .FindByIdAsync(userId);
            if (findByIdAsync != null)
            {
                var list = finished.HasValue ? (await findByIdAsync)?.Prints?.Where(x => x.Finished == finished.Value) : (await findByIdAsync)?.Prints;

                return list;
            }
            return null;
        }

        public static int GetUsersActivePrintsCount(string userId)
        {
            var findById = HttpContext.Current?.GetOwinContext()?
                .GetUserManager<ApplicationUserManager>()?
                .FindById(userId);
            if (findById != null)
            {
                return findById.Prints.Count(x => !x.Finished && !x.Stopped);
            }
            return 0;
        }
        public static string GetOwnerId(this PrintViewModel print)
        {
            string id;
            using (ApplicationDbContext adc = new ApplicationDbContext())
            {
                var q = from e in adc.Users
                    where (e.Prints.FirstOrDefault(x => x.ID == print.ID) !=null)
                    select e;
                id = q.FirstOrDefault()?.Id;
            }
            return id;

        }

        public static bool isFileValidPNG(this HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength <= 0 || file.ContentType != "image/png")
                return false;
            return true;
        }

    }
}