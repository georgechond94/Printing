using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace printing.Models
{
    public class ServiceViewModel
    {
        [Required]
        [Display(ResourceType = typeof (Resources.Resources), Name = "ID")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "FormViewModel_ID_Please_enter_a_valid_integer_number_")]
        public int ID { get; set; }
    }
}