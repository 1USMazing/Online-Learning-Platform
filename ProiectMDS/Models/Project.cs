using ProiectMDS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProiectMDS.Models
{
	public class Project
	{
		[Key]
		public int ProjectId { get; set; }

		[Required(ErrorMessage = "Numele proiectului este obligatoriu")]
		public string ProjectName { get; set; }

		public string ProjectDescription { get; set; }

		public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

		public virtual ICollection<Task> Tasks { get; set; }


        public virtual ICollection<ApplicationUser> Membrii { get; set; }

    }
}