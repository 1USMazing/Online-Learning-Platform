using ProiectMDS.Models;
using ProiectMDS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProiectMDS.Models
{
	public class Comment
	{
		[Key]
		public int CommentId { get; set; }

        [Required(ErrorMessage = "Campul nu poate fi necompletat")]
        public string Content { get; set; }

		public DateTime Date { get; set; }
		public int CourseId { get; set; }

		public string UserId { get; set; }

		public virtual ApplicationUser User { get; set; }

		public virtual Course Course { get; set; }
	}
}