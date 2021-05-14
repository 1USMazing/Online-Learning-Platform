using ProiectMDS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectMDS.Models
{
    public class ApartineT
    {

        [Key]
        public int idTask { get; set; }

        [Required]
        public int TaskId { get; set; }


        public string Email { get; set; }

        [Required]
        public string MembruId { get; set; }

        public IEnumerable<SelectListItem> Usr { get; set; }

        public string[] SelectedUsers { get; set; }

        virtual public ICollection<ApplicationUser> Userz { get; set; }

        public virtual Task task { get; set; }
        public virtual ApplicationUser user { get; set; }
    }

}