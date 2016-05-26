using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using printing.Models;
using printing.Attributes;

namespace printing.Controllers
{
    [XMLConfig]
    public class XmlController : ApiController
    {
        
        public List<Print> Get(string type,string userId=null)
        {

            List<Print> list = new List<Print>();
            
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                switch (type)
                {
                    case "Queue":

                        foreach (var prin in MvcApplication.Queue)
                        {
                            string usrname = "";
                            var ownid = "";

                            ownid = prin.GetOwnerId();
                            usrname = db.Users.Find(ownid).UserName;

                            


                            var p = new Print { print = prin, user = new Models.User{UserId = ownid, Username = usrname}};
                            if (MvcApplication.Queue.CurrentPrint.ID != p.print.ID)
                                p.print.EstimatedTime = MvcApplication.Queue.WaitTimeOf(p.print) + p.print.Mass * 10;
                            else
                                p.print.EstimatedTime = MvcApplication.Queue.RemainingTimeOfCurrentPrint();
                            list.Add(p);
                        }

                        break;
                    case "Prints":

                        if (userId == null)
                        {
                            var prints = db.Prints.ToList();
                            foreach (var prin in prints)
                            {

                                string usrname = "";
                                var ownid = "";

                                ownid = prin.GetOwnerId();

                                var usr = db.Users.Find(ownid);
                                usrname = usr.UserName;



                                var p = new Print { print = prin, user = new Models.User { UserId = ownid, Username = usrname } };
                                list.Add(p);
                            }

                        }
                        else
                        {
                            var x = db.Users.Find(userId);

                            string usrname = x.UserName;
                            var ownid = userId;

                            list.AddRange(x.Prints.Select(prin => new Print { print = prin, user = new Models.User { UserId = ownid, Username = usrname } }));
                        }

                        break;
                }
            }
            return list;
        }

    }
}
