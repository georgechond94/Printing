using printing.Attributes;
using printing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace printing.Controllers
{
    [JSONConfig]
    public class JsonController : ApiController
    {

        public List<Print> Get(string type, string userId = null)
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



                            var p = new Print { print = prin, user = new Models.User { UserId = ownid, Username = usrname } };
                            if (MvcApplication.Queue.CurrentPrint.ID != p.print.ID)
                                p.print.EstimatedTime = MvcApplication.Queue.WaitTimeOf(p.print) + p.print.Mass*10;
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
