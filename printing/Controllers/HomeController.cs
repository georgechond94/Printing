using printing.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using printing.Comparers;
using printing.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebGrease.Css.Extensions;

namespace printing.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {   
            
            return View();
        }

        public async Task<ActionResult> ServiceDemo()
        {
            var queueurl =
                    $"{Request?.Url?.Scheme}://{Request?.Url?.Authority}/api{Url.Action("Get","Xml",new {type = "Queue"})}";
            var printsurl =
        $"{Request?.Url?.Scheme}://{Request?.Url?.Authority}/api{Url.Action("Get", "Xml", new { type = "Prints" })}";
            var usersurl =
                    $"{Request?.Url?.Scheme}://{Request?.Url?.Authority}/api{Url.Action("Get", "Xml", new { type = "Prints", UserId= User.Identity.GetUserId() })}";

            string reqq = $"GET {queueurl}";
            string reqp = $"GET {printsurl}";
            string requ = $"GET {usersurl}";

            HttpClient client = new HttpClient();

            ViewBag.Requestq = reqq;
            ViewBag.Requestp = reqp;
            ViewBag.Requestu = requ;


            XDocument doc = XDocument.Load(await client.GetStreamAsync(queueurl));

            ViewBag.Bodyq = doc.ToString();

            doc = XDocument.Load(await client.GetStreamAsync(printsurl));

            ViewBag.Bodyp = doc.ToString();

            doc = XDocument.Load(await client.GetStreamAsync(usersurl));

            ViewBag.Bodyu = doc.ToString();

            return View();
        }
        public async Task<ActionResult> History()
        {
            if (User.IsInRole("Admin"))
            {
                ViewBag.HistoryPrints = Helpers.GetAllPrints(true);
            }
            else
            {
                ViewBag.HistoryPrints = await Helpers.GetUsersPrintsAsync(User.Identity.GetUserId(), true);
            }
            return View();
        }
        public ActionResult Form()
        {
            return View();
        }
        public ActionResult Service()
        {
            return View();
        }    
        public ActionResult Details(int id=0)
        {
            if (id<1)
                return RedirectToAction("Index");

            PrintViewModel pvm = Helpers.GetPrintById(id);

            return View(pvm);
        }

        public async Task<ActionResult> ContinuePrint(int id = 0)
        {

            PrintViewModel print;
            if (User.IsInRole("Admin"))
                print = Helpers.GetPrintById(id);
            else
                print = await Helpers.GetUserPrintByIdAsync(User.Identity.GetUserId(), id);

            MvcApplication.Queue.ContinuePrint(print);

            return RedirectToAction("Service");
        }

        public async Task<ActionResult> PausePrint(int id = 0)
        {

            PrintViewModel print;
            if (User.IsInRole("Admin"))
                print = Helpers.GetPrintById(id);
            else
                print = await Helpers.GetUserPrintByIdAsync(User.Identity.GetUserId(), id);


            MvcApplication.Queue.PausePrint(print);
            
            return RedirectToAction("Service");
        }
        public async Task<ActionResult> DeletePrint(int id = 0)
        {

            PrintViewModel print;
            if (User.IsInRole("Admin"))
                print = Helpers.GetPrintById(id);
            else
                print = await Helpers.GetUserPrintByIdAsync(User.Identity.GetUserId(), id);

            MvcApplication.Queue.DeletePrint(print);

            return RedirectToAction("Service");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Form(PrintViewModel model)
        {


            if (!ModelState.IsValid)
            {
                model.SuccessMessage = false;
                model.Message = Resources.Resources.HomeController_Form_An_error_occurred_with_form_validation__Please_try_again_;
            }
            else if (!model.File.isFileValidPNG())
            {
                model.SuccessMessage = false;
                model.Message = Resources.Resources.FileError;

            }
            else
            {
                var fn = model.File.FileName.Split('/','\\');
                

                var path = Server.MapPath("~/Files/" + DateTime.Now.Millisecond +fn[fn.Length-1]);
                model.File.SaveAs(path);

                var pathurl =
                    $"{Request?.Url?.Scheme}://{Request?.Url?.Authority}{Url.Content("~/Files/" + fn[fn.Length - 1])}";

                model.FileUrl = pathurl;

                model.PostedDate = DateTime.Now;

                try
                {

                        ApplicationUser user =
                            await HttpContext.GetOwinContext()
                                .GetUserManager<ApplicationUserManager>()
                                .FindByIdAsync(User.Identity.GetUserId());
                        //appdbc.Prints.Add(model);
                        user.Prints.Add(model);
                        await HttpContext.GetOwinContext()
                            .GetUserManager<ApplicationUserManager>().UpdateAsync(user);


                    //Session["Prints"] = user.Prints.Where(x => x.Finished == false);

                    MvcApplication.Queue.AddPrint(model);

                    EmailService service = new EmailService();
                    service.SendAsync(new IdentityMessage {Body = "<h3>3D Printing Service - Νέα εκτύπωση</h3><h4>Ευχαριστούμε πολύ!</h4><hr><br><p>Εκτύπωση: " + model.ID+ "<br>Eκτιμώμενος Xρόνος: " + MvcApplication.Queue.WaitTimeOf(model) + " λεπτά" + "<br><a href='" + @Url.Action("Details", "Home", new { id = model.ID }, Request.Url.Scheme) + "'>Πάτησε εδώ για να δεις πληροφορίες για την εκτύπωση σου!</a></p><br><hr>3D Printing Service", Subject = "Η 3D εκτύπωσή σου.",Destination = User.Identity.Name});
                                        

                    model.SuccessMessage = true;
                    model.Message = Resources.Resources.HomeController_Form_Print_successfully_saved__You_can_invoke_the_Service_with_your_ID__;
                     
                }
                catch (DbUpdateException e)
                {
                    model.SuccessMessage = false;
                    model.Message = "Exception: "+e.Message;

                }

                

            }
            return View(model);
        }

        /// <summary>
        /// Gets an XML file from a print view model.
        /// </summary>
        /// <param name="id">The print view model id.</param>
        /// <returns></returns>
        public ActionResult PrintXML(int id)
        {
            XDocument doc = new XDocument();


            var pvm = Helpers.GetPrintById(id);
            using (var writer = doc.CreateWriter())
            {

                // write xml into the writer
                var serializer = new DataContractSerializer(typeof (PrintViewModel));
                serializer.WriteObject(writer, pvm);
            }
            var elem = doc.Elements().FirstOrDefault();

            elem?.Add(new XElement("User", User.Identity.GetUserName()));




            return Content(doc.ToString(), "application/xml");
        }

        /// <summary>
        /// Gets every print as XML.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ServiceAll()
        {
            XDocument doc = new XDocument();
            PrintViewModel[] query;

            if (User.IsInRole("Admin"))
            {
                query = Helpers.GetAllPrints(true).ToArray();
            }
            else
            {
                query = (await Helpers.GetUsersPrintsAsync(User.Identity.GetUserId(), true)).ToArray();

            }


            using (var writer = doc.CreateWriter())
                {

                    // write xml into the writer
                    var serializer = new DataContractSerializer(typeof(PrintViewModel[]));
                    serializer.WriteObject(writer, query);
                }

                foreach (XElement el in doc.Elements().FirstOrDefault().Elements())
                {
                    el.Add(new XElement("User", User.Identity.GetUserName()));
                                        
                }


            

            return Content(doc.ToString(), "application/xml");
        }

    }
}