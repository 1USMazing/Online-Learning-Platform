using ProiectMDS.Models;
using ProiectMDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ProiectMDS.Controllers
{
    public class CommentController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();
		
		// GET: Comment
		public ActionResult Index()
		{
			return View();
		}

		[HttpDelete]
        [Authorize(Roles = "Membru, Organizator, Admin")]
        public ActionResult Delete(int id)
		{
			Comment comm = db.Comments.Find(id);
            if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Comments.Remove(comm);
                db.SaveChanges();
                return Redirect("/Task/Show/" + comm.TaskId);
            }
            else
            {
                TempData["comm"] = "Nu aveți dreptul să faceți modificări!";
                return RedirectToAction("../Task/Show/" + comm.TaskId);
            }
        }



        [Authorize(Roles = "Membru, Organizator, Admin")]
        public ActionResult Edit(int id)
		{
			Comment comm = db.Comments.Find(id);
            if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(comm);
            }
            else
            {
                TempData["comm"] = "Nu aveți dreptul să faceți modificări!";
                return RedirectToAction("../Task/Show/" + comm.TaskId);
            }
        }


		[HttpPut]
        [Authorize(Roles = "Membru, Organizator, Admin")]
        public ActionResult Edit(int id, Comment requestComment)
		{
			try
			{
				Comment comm = db.Comments.Find(id);
                if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                {
                    if (TryUpdateModel(comm))
                    {
                        comm.Content = requestComment.Content;
                        db.SaveChanges();
                    }
                    return Redirect("/Task/Show/" + comm.TaskId);
                }
                else
                {
                    TempData["comm"] = "Nu aveți dreptul să faceți modificări!";
                    return RedirectToAction("../Task/Show/" + comm.TaskId);
                }
            }

			catch (Exception e)
			{
				return View(requestComment);
			}

		}
	}
}