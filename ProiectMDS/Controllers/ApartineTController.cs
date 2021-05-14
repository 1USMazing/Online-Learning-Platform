using ProiectMDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Collections.ObjectModel;

namespace ProiectMDS.Controllers
{
    public class ApartineTController : Controller
    {
        // GET: ApartineT

        private ProiectMDS.Models.ApplicationDbContext db = new ProiectMDS.Models.ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New(int? id)
        {

            var projown = (from tsk in db.Tasks
                          where tsk.Id == id
                          select tsk.ProjectId).SingleOrDefault();

            var owner = (from own in db.Projects
                         where own.ProjectId == projown
                         select own.UserId).SingleOrDefault();

            if (owner == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                var selectList = new List<SelectListItem>();

                var inTask = from mem in db.ApartineTs
                                where mem.TaskId == id
                                select mem.MembruId;

                var inProject = from mem in db.Apartines
                                where mem.ProjectId == projown
                                select mem.MembruId;

                var users = from user in db.Users
                            where !inTask.Contains(user.Id) && inProject.Contains(user.Id)
                            select user;

                foreach (var usr in users)
                {


                    SelectListItem selectList2 = new SelectListItem()
                    {
                        Value = usr.Id.ToString(),
                        Text = usr.Email.ToString()
                    };
                    selectList.Add(selectList2);
                }



                ViewBag.UsersList = new SelectList(selectList, "Value", "Text");


                ApartineT apartine = new ApartineT();

                //  apartine.Userz = new Collection<ApplicationUser>();

                apartine.Usr = selectList;
                apartine.TaskId= Convert.ToInt32(id);


                return View(apartine);
            }
            else
            {
                TempData["edittask"] = "Nu aveți dreptul să faceți modificări asupra unui proiect ce nu vă aparține!";
                return RedirectToAction("../Task/Show/" + id);

            }
        }

        
        [HttpPost]
        public ActionResult New(ApartineT apartine)
        {
            try
            {
                if (true)
                {
                    var id = apartine.TaskId;

                    // apartine.Userz = new Collection<ApplicationUser>();
                    foreach (var selectedUser in apartine.SelectedUsers)
                    {
                        apartine.MembruId = selectedUser;
                        apartine.TaskId = id;
                        db.ApartineTs.Add(apartine);
                        db.SaveChanges();
                    }
                    db.SaveChanges();
                  
                    return RedirectToAction("../Task/Show/" + id);
                }
                else
                {
                    return RedirectToAction("../Project/Index/" );
                }    
            }

            catch (Exception e)
            {
                return View("Error");
            }

         
        }


        public ActionResult Remove(int? id)
        {

            var projown = (from tsk in db.Tasks
                           where tsk.Id == id
                           select tsk.ProjectId).SingleOrDefault();

            var owner = (from own in db.Projects
                         where own.ProjectId == projown
                         select own.UserId).SingleOrDefault();

            if (owner == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                var selectList = new List<SelectListItem>();

                var inTask = from mem in db.ApartineTs
                             where mem.TaskId == id
                             select mem.MembruId;


                var users = from user in db.Users
                            where (inTask.Contains(user.Id))
                            select user;

                foreach (var usr in users)
                {


                    SelectListItem selectList2 = new SelectListItem()
                    {
                        Value = usr.Id.ToString(),
                        Text = usr.Email.ToString()
                    };
                    selectList.Add(selectList2);
                }



                ViewBag.UsersList = new SelectList(selectList, "Value", "Text");


                ApartineT apartine = new ApartineT();

                //  apartine.Userz = new Collection<ApplicationUser>();

                apartine.Usr = selectList;
                apartine.TaskId = Convert.ToInt32(id);


                return View(apartine);
            }

            else
            {
                TempData["edittask"] = "Nu aveți dreptul să faceți modificări asupra unui proiect ce nu vă aparține!";
                return RedirectToAction("../Task/Show/" + id);

            }


        }

        [HttpDelete]
        public ActionResult Remove(ApartineT apartine)
        {

            {
                if (true)
                {
                    var id = apartine.TaskId;
                    var ok = 1;

                    // apartine.Userz = new Collection<ApplicationUser>();
                    foreach (var selectedUser in apartine.SelectedUsers)
                    {
                        var fordelete = (from dlt in db.ApartineTs
                                         where dlt.MembruId == selectedUser && dlt.TaskId == id
                                         select dlt.idTask).SingleOrDefault();



                        apartine = db.ApartineTs.Find(fordelete);

                        db.ApartineTs.Remove(apartine);
                        db.SaveChanges();
                    }

                    db.SaveChanges();
                    TempData["message"] = "Membrul a fost eliminat!";
                    return RedirectToAction("../Task/Show/" + id);
                }
                else
                {
                    return RedirectToAction("../Project/Index/");
                }
            }




        }


    }
}

