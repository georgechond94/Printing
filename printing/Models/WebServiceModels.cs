using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace printing.Models
{
    [KnownTypeAttribute(typeof(User))]
    public class User
    {
        public string UserId { get; set; }
        public string Username { get; set; }
    }
    [KnownTypeAttribute(typeof(Print))]
    public class Print
    {
        public PrintViewModel print { get; set; }
        public User user{ get; set; }
    }
}