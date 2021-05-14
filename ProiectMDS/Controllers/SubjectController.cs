using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProiectMDS.Models;

namespace ProiectMDS.Controllers
{   
    [Authorize]
    public class SubjectController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Subject

        [Authorize(Roles = "Membru, Organizator, Admin")]
        public ActionResult Index()
		{

            var users = from user in db.Users
                        orderby user.UserName
                        select user;

            ViewBag.UsersList = users;


            if (User.IsInRole("Membru") || User.IsInRole("Organizator"))
            {
                var proiecteleLui = User.Identity.GetUserId();

                var subjects = from proj in db.Apartines
                               where proj.MembruId == proiecteleLui
                               select proj.ProjectId;

                var lengthproj = new List<int>(subjects);

                var numeProiekte = from projj in db.Projects
                                   where lengthproj.Contains(projj.ProjectId)
                                   select projj;


                ViewBag.Proiektzele = numeProiekte;

                return View();
            }
            else
            {
                var subjects = from subject in db.Projects
                               orderby subject.ProjectName
                               select subject;
                ViewBag.Proiektzele = subjects;

                return View();
            }
        }

        public ActionResult Show(int id)
        {
            Project subject = db.Projects.Find(id);


            var owner = (from own in db.Projects
                         where own.ProjectId == id
                         select own.UserId).SingleOrDefault();

            var inSubject = from mem in db.Apartines
                            where mem.ProjectId == id
                            select mem.MembruId;

            var users = from user in db.Users
                        where (inSubject.Contains(user.Id))
                        select user;



            ViewBag.membrii = users;

            var ownerr = (from own in db.Users
                          where own.Id == owner
                          select own.UserName).SingleOrDefault();

            ViewBag.owner = ownerr;

            var apartine = from apar in db.Apartines
                           where apar.ProjectId == id
                           select apar.MembruId;
            var ok = 0;

            if (apartine.Contains(User.Identity.GetUserId()) || User.IsInRole("Admin"))
            {
                ok = 1;
            }

            if (ok == 1)
            {
                return View(subject);
            }
            else
            {
                TempData["message"] = "Nu ești membru în cadrul proiectului pe care ai încercat să îl accesezi!";
                return RedirectToAction("/Index");
            }
        }



        [Authorize(Roles = "Membru, Organizator, Admin")]
        public ActionResult New()
		{
            
			return View();
            // cat.UserId = User.Identity.GetUserId(); 
            // de facut update in db User la Role Id

        }

        [HttpPost]
		public ActionResult New(Project cat)
		{
			try
			{
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                cat.UserId = User.Identity.GetUserId();

                if (User.IsInRole("Membru"))
                {
                    UserManager.RemoveFromRole(cat.UserId, "Membru");
                    UserManager.AddToRole(cat.UserId, "Organizator");
                }
                Apartine apartine = new Apartine();
                apartine.ProjectId = cat.ProjectId;
                apartine.MembruId = cat.UserId;
                db.Projects.Add(cat);
               // db.SaveChanges();
                db.Apartines.Add(apartine);
				db.SaveChanges();
                
				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				return View();
			}
		}

        public ActionResult Edit(int id)
        {
            Project subject = db.Projects.Find(id);
            ViewBag.Subject = subject;


            if (subject.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                TempData["message"] = "Nu aveți dreptul să faceți modificări asupra unui proiect ce nu vă aparține!";
                return RedirectToAction("../Subject/Show/" + id);

            }
		}

		[HttpPut]
		public ActionResult Edit(int id, Project requestSubject)
		{
			try
			{
				Project subject = db.Projects.Find(id);
				if (TryUpdateModel(subject))
				{
					subject.ProjectName = requestSubject.ProjectName;
					db.SaveChanges();
				}

				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				return View();
			}
		}

		[HttpDelete]
		public ActionResult Delete(int id)
		{
            Project subject = db.Projects.Find(id);
            if (subject.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                var deletedid = subject.UserId;
                db.Projects.Remove(subject);
                db.SaveChanges();

                var nrOfproj = from proj in db.Projects
                               where proj.UserId == deletedid
                               select proj.ProjectName;


                var lengthproj = new List<string>(nrOfproj);



                int result = lengthproj.Count();

                if (result == 0)
                {
                    ApplicationDbContext context = new ApplicationDbContext();
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    if (User.IsInRole("Organizator"))
                    {
                        UserManager.RemoveFromRole(deletedid, "Organizator");
                        UserManager.AddToRole(deletedid, "Membru");
                        db.SaveChanges();
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveți dreptul să faceți modificări asupra unui proiect ce nu vă aparține!";
                return RedirectToAction("../Subject/Show/" + id);

            }
        }
	}
}