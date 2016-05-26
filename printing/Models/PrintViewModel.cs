using printing.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace printing.Models
{

    /// <summary>
    /// Represents a Print.
    /// </summary>
    /// 
    [KnownTypeAttribute(typeof(PrintViewModel))]
    public class PrintViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "FormViewModel_Mass_Mass__1_60_")]
        [Range(1, 60, ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "FormViewModel_ID_Please_enter_a_valid_integer_number_")]
        public int Mass { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "FormViewModel_Field2_Field2")]
        public string Title { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "FormViewModel_Field3_Field3")]
        public string Description { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "FormViewModel_PrintPriority_Priority")]
        public Priority PrintPriority { get; set; }

        public string FileUrl { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "FormViewModel_PostedDate")]
        public DateTime? PostedDate { get; set; }

        public DateTime? StartedDate { get; set; }

        public bool Stopped { get; set; }
        public bool Finished { get; set; }


        [NotMapped]
        public int EstimatedTime { get; set; }

        //[Required]
        [NotMapped]
        [IgnoreDataMember]
        [Display(ResourceType = typeof (Resources.Resources), Name = "FormViewModel_File_File")]
        public HttpPostedFileBase File { get; set; }

        [IgnoreDataMember]
        [NotMapped]
        public bool? SuccessMessage { get; set; }

        [IgnoreDataMember]
        [NotMapped]
        public string Message { get; set; }
        
    }
}