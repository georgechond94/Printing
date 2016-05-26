using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Controllers;

namespace printing.Attributes
{
    public class XMLConfigAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings,
                               HttpControllerDescriptor controllerDescriptor)
        {
            controllerSettings.Formatters.Remove(controllerSettings.Formatters.JsonFormatter);
        }
    }

    public class JSONConfigAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings,
                               HttpControllerDescriptor controllerDescriptor)
        {
            controllerSettings.Formatters.Remove(controllerSettings.Formatters.XmlFormatter);

        }
    }
}