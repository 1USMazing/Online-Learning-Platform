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
    public class CourseController : Controller
    {

		private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
		{
			var courses = db.Tasks.Include("Project");
			ViewBag.Courses = courses;
			if (TempData.ContainsKey("message"))
			{
				ViewBag.Message = TempData["message"];
			}

			return View();
		}

        [HttpPost]
        [Authorize(Roles = "Membru, Organizator, Admin")]
        public ActionResult Show(Comment comm)
        {
            comm.Date = DateTime.Now;
            comm.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Comments.Add(comm);
                    db.SaveChanges();
                    return Redirect("/Course/Show/" + comm.TaskId);
                }

                else
                {

                    Task a = db.Tasks.Find(comm.TaskId);

                    SetAccessRights(a.Id);

                    return View(a);
                }

            }

            catch (Exception e)
            {

                Task a = db.Tasks.Find(comm.TaskId);

                SetAccessRights(a.Id);

                return View(a);
            } 

        }
        [Authorize(Roles = "Membru, Organizator, Admin")]
        public ActionResult Show(int id)
		{
			Task course = db.Tasks.Find(id);

            course.UserId = User.Identity.GetUserId();

            ViewBag.afisareButoane = false;

            if (User.IsInRole("Organizator") || User.IsInRole("Admin"))
            {
                ViewBag.afisareButoane = true;
            }
            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();

            DateTime oraExacta = DateTime.Now;

            if (course.DueDate < oraExacta && course.Status == "In Progress" )
            {
                course.Status = "In Progress - WARNING: DEADLINE HAS BEEN EXCEEDED!";
                db.SaveChanges();
            }

            var projown = (from tsk in db.Tasks
                           where tsk.Id == id
                           select tsk.ProjectId).Single();

            var owner = (from own in db.Projects
                         where own.ProjectId == projown
                         select own.UserId).Single();

            var inCourse = from mem in db.ApartineTs
                         where mem.TaskId == id
                         select mem.MembruId;

            var users = from user in db.Users
                        where (inCourse.Contains(user.Id))
                        select user.UserName;

            ViewBag.membrii = users.ToList();

            var apartine = from apar in db.ApartineTs
                           where apar.TaskId == id
                           select apar.MembruId;
            var ok = 0;

            if (apartine.Contains(User.Identity.GetUserId()) || User.IsInRole("Admin"))
            {
                ok = 1;
            }

            if (ok == 1)
            {
                return View(course);
            }
            else
            {
                TempData["coursemessage"] = "Nu ești membru în cadrul task-ului pe care ai încercat să îl accesezi!";
                return RedirectToAction("../Project/Show/" + course.ProjectId);
            }

		}

        [Authorize(Roles = "Membru, Organizator, Admin")]
        public ActionResult New(int? id)
		{
			if ( id == null )
			{
				return View("Error");
			}
            var owner = (from own in db.Projects
                         where own.ProjectId == id
                         select own.UserId).Single();

            if (owner == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                Task course = new Task();
                // task.Proj = GetAllProjects();
                course.ProjectId = Convert.ToInt32(id);
                return View(course);
                
            }
            else
            {
                TempData["message"] = "Nu aveți dreptul să faceți modificări asupra unui proiect ce nu vă aparține!";
                return RedirectToAction("../Project/Show/" + Convert.ToInt32(id));

            }
        }

		[HttpPost]
		public ActionResult New(Task course)
		{
			// task.Proj = GetAllProjects();
			try
			{
				if (ModelState.IsValid)
				{


                    course.Status = "Not started";
                    course.StartDate = null;
                    db.Tasks.Add(course);
                    var id = course.ProjectId;
                    ApartineT apartine = new ApartineT();
                    apartine.TaskId = course.Id;
                    apartine.MembruId = User.Identity.GetUserId();
                    db.ApartineTs.Add(apartine);
					db.SaveChanges();
					TempData["message"] = "Cursul a fost adăugat!";
					return RedirectToAction("../Subject/Show/" + id);
                 
				}
				else
				{
					return View(course);
				}
			}

			catch (Exception e)
			{
				return RedirectToAction("Index");
			}
		}

		public ActionResult Edit(int id)
		{

			Task course = db.Tasks.Find(id);
			// course.Proj = GetAllProjects();
			return View(course);
		}

		[HttpPut]
		public ActionResult Edit(int id, Task requestCourse)
		{
			try
			{
                var projown = (from tsk in db.Tasks
                               where tsk.Id == id
                               select tsk.ProjectId).SingleOrDefault();

                var owner = (from own in db.Projects
                             where own.ProjectId == projown
                             select own.UserId).SingleOrDefault();

                if (owner == User.Identity.GetUserId() || User.IsInRole("Admin"))
                {
                    if (ModelState.IsValid)
                    {
                        Task course = db.Tasks.Find(id);
                        if (TryUpdateModel(course))
                        {
                            course.Title = requestCourse.Title;
                            course.Description = requestCourse.Description;
                            course.DueDate = requestCourse.DueDate;
                            course.ProjectId = requestCourse.ProjectId;
                            db.SaveChanges();
                            TempData["message"] = "Coursul a fost modificat!";

                        }
                        return RedirectToAction("../Course/Show/" + course.Id);


                    }
                    else
                    {
                        return View(requestCourse);
                    }

                }
                else
                {
                        TempData["editcourse"] = "Nu aveți dreptul să faceți modificări asupra unui proiect ce nu vă aparține!";
                        return RedirectToAction("../Course/Show/" + Convert.ToInt32(id));

                }
				}

			catch (Exception e)
			{
				return View(requestCourse);
			}
		}

		[HttpDelete]
		public ActionResult Delete(int id)
		{
			Task course = db.Tasks.Find(id);
            var idp = course.ProjectId;
            var projown = (from tsk in db.Tasks
                           where tsk.Id == id
                           select tsk.ProjectId).SingleOrDefault();

            var owner = (from own in db.Projects
                         where own.ProjectId == projown
                         select own.UserId).SingleOrDefault();

            if (owner == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Tasks.Remove(course);
                db.SaveChanges();
                TempData["message"] = "Cursul a fost șters!";
                return RedirectToAction("../Subject/Show/" + idp);
            }
            else
            {
                TempData["editcourse"] = "Nu aveți dreptul să faceți modificări asupra unui curs la care nu predați!";
                return RedirectToAction("/Show/" + id);
            }
		}

        private void SetAccessRights(int id)
        {
            ViewBag.afisareButoane = false;
            if (User.IsInRole("Organizator") || User.IsInRole("Admin"))
            {
                ViewBag.afisareButoane = true;
            }

            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();

            var projown = (from tsk in db.Tasks
                           where tsk.Id == id
                           select tsk.ProjectId).Single();

            var owner = (from own in db.Projects
                         where own.ProjectId == projown
                         select own.UserId).Single();

            var inCourse = from mem in db.ApartineTs
                         where mem.TaskId == id
                         select mem.MembruId;

            var users = from user in db.Users
                        where (inCourse.Contains(user.Id))
                        select user.UserName;

            ViewBag.membrii = users.ToList();
        }

        public ActionResult StartCourse(int id)
        {

            Task course = db.Tasks.Find(id);
            var projown = (from tsk in db.Tasks
                           where tsk.Id == id
                           select tsk.ProjectId).SingleOrDefault();

            var owner = (from own in db.Projects
                         where own.ProjectId == projown
                         select own.UserId).SingleOrDefault();

            var inCourse = from mem in db.ApartineTs
                         where mem.TaskId == id
                         select mem.MembruId;

            var ok = 0;

            if (inCourse.Contains(User.Identity.GetUserId()))
            {
                ok = 1;
            }


            if (owner == User.Identity.GetUserId() || User.IsInRole("Admin") || ok == 1)
            {
                course.Status = "In Progress";
                course.StartDate = DateTime.Now;
                db.SaveChanges();
                TempData["status"] = "Course has started";
                return RedirectToAction("../Course/Show/" + id);
            }
            else
            {
                TempData["editcourse"] = "Nu sunteți participant în cadrul acestui course!";
                return RedirectToAction("/Show/" + id);
            }


        }

        public ActionResult FinishCourse(int id)
        {

            Task course = db.Tasks.Find(id);

            var projown = (from tsk in db.Tasks
                           where tsk.Id == id
                           select tsk.ProjectId).SingleOrDefault();

            var owner = (from own in db.Projects
                         where own.ProjectId == projown
                         select own.UserId).SingleOrDefault();

            var inCourse = from mem in db.ApartineTs
                         where mem.TaskId == id
                         select mem.MembruId;

            var ok = 0;
            
            if (inCourse.Contains(User.Identity.GetUserId()))
            {
                ok = 1;
            }
{
                if (owner == User.Identity.GetUserId() || User.IsInRole("Admin") || ok == 1)
            {

                if (course.Status == "In Progress")

                {
                    course.Status = "Finished";
                    db.SaveChanges();
                }
                else if (course.Status == "In Progress - WARNING: DEADLINE HAS BEEN EXCEEDED!")
                {

                    TimeSpan diffdate = DateTime.Now.Subtract(course.DueDate);
                    course.Status = "Finished with a delay of " + diffdate.Days.ToString() + " days";
                    db.SaveChanges();
                    TempData["status"] = "Course has been finished";
                }

                return RedirectToAction("../Course/Show/" + id);
            }
                else
                {
                    TempData["editcourse"] = "Nu sunteți participant în cadrul acestui course!";
                    return RedirectToAction("/Show/" + id);
                }
            }
        }


        /* [NonAction]
		public IEnumerable<SelectListItem> GetAllProjects()
		{
			// generam o lista goala
			var selectList = new List<SelectListItem>();

			// extragem toate proiectele din baza de date
			var projects = from proj in db.Projects
							 select proj;

			// iteram prin proiecte
			foreach (var project in projects)
			{
				// adaugam in lista elementele necesare pentru dropdown
				selectList.Add(new SelectListItem
				{
					Value = project.ProjectId.ToString(),
					Text = project.ProjectName.ToString()
				});
			}


			// returnam lista de categorii
			return selectList;
		} */

    }
}
