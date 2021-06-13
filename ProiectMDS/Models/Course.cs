using ProiectMDS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectMDS.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Cursul este obligatoriu")]
        [StringLength(30, ErrorMessage = "Denumirea Cursului nu poate avea mai mult de 30 caractere")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Descrierea Curs-ului este obligatorie")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }

        internal static Course FromResult(int v)
        {
            throw new NotImplementedException();
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }


        public string Status { get; set; }

        public string AssignTo { get; set; }

        [Required(ErrorMessage = "Proiectul este obligatoriu")]
        public int SubjectId { get; set; }

        public string UserId { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public IEnumerable<SelectListItem> Proj { get; set; }

    }

}