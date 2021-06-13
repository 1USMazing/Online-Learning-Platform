using ProiectMDS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProiectMDS.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Numele proiectului este obligatoriu")]
        public string SubjectName { get; set; }

        public string SubjectDescription { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Course> Courses { get; set; }


        public virtual ICollection<ApplicationUser> Membrii { get; set; }

    }
}