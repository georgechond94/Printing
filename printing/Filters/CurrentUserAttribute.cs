using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using printing.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace printing.Filters
{
    public class AsyncCurrentUserAttribute : FilterAttribute,IActionFilter
    {

        public async Task ExecuteActionFilterAsync(ActionExecutingContext filterContext, CancellationToken cancellationToken)
        {
            if (!filterContext.IsChildAction && !filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {

                filterContext.HttpContext.Session["Prints"] = (await HttpContext.Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindByIdAsync(HttpContext.Current.User.Identity.GetUserId())).Prints.Where(x => x.Finished);


            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            throw new NotImplementedException();
        }
    }
}