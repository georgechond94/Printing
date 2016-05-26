using printing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace printing.App_Start
{
    public class DbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);

            context.Roles.Add(new IdentityRole {Id="admin",Name="Admin"});
            context.Roles.Add(new IdentityRole { Id = "user", Name = "User" });

            //context.Prints.Add(new FormViewModel { ID = 0, Mass = 0, Field2 = "Test", Field3 = "Test", File = null, PrintPriority = Enums.Priority.LOW, FileUrl="http://google.gr"});
            //context.SaveChanges();
        }
    }
}