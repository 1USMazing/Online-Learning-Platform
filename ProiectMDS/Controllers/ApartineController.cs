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
    public class ApartineController : Controller
    {

        private ProiectMDS.Models.ApplicationDbContext db = new ProiectMDS.Models.ApplicationDbContext();
        // GET: Apartine

        public ActionResult Index(string id)
        {

            /*List<int> Iduri = db.Apartines.Where(a => a.MembruId == id).Select(a => a.ProjectId).ToList(); // id-urile proiectelor unde apartin
            var proiecte = db.Projects.Where(p => Iduri.Contains(p.ProjectId));
            ViewBag.Projects = proiecte;
            ViewBag.MembruId = id;*/

            var proiecteleLui = User.Identity.GetUserId();

            var projects = from proj in db.Apartines
                           where proj.MembruId == proiecteleLui
                           select proj.ProjectId;

            var lengthproj = new List<int>(projects);

            var numeProiekte = from projj in db.Projects
                               where lengthproj.Contains(projj.ProjectId)
                               select projj;


            ViewBag.Proiektzele = numeProiekte;

            return View();

        }


        public ActionResult New(int? id)
        {

            var owner = (from own in db.Projects
                        where own.ProjectId == id
                        select own.UserId).SingleOrDefault();

            if (owner == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                var selectList = new List<SelectListItem>();

                var inProject = from mem in db.Apartines
                                where mem.ProjectId == id
                                select mem.MembruId;

                var users = from user in db.Users
                            where !(inProject.Contains(user.Id))
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


                Apartine apartine = new Apartine();

               //  apartine.Userz = new Collection<ApplicationUser>();

                apartine.Usr = selectList;
                apartine.ProjectId = Convert.ToInt32(id);

                    
                return View(apartine);
            }
            else
            {
                TempData["message"] = "Nu aveți dreptul să faceți modificări asupra unui proiect ce nu vă aparține!";
                return RedirectToAction("../Project/Show/" + id);

            }

            
        }

        [HttpPost]
        public ActionResult New(Apartine apartine)
        {
            try
            {
                if (true)
                {
                    var id = apartine.ProjectId;

                    // apartine.Userz = new Collection<ApplicationUser>();
                    foreach (var selectedUser in apartine.SelectedUsers)
                    {
                        apartine.MembruId = selectedUser;
                        apartine.ProjectId = id;
                        db.Apartines.Add(apartine);
                        db.SaveChanges();
                    }
                    db.SaveChanges();
                    TempData["message"] = "Membrul a fost adăugat!";
                    return RedirectToAction("../Project/Show/" + id);
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

            var owner = (from own in db.Projects
                         where own.ProjectId == id
                         select own.UserId).SingleOrDefault();

            if (owner == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                var selectList = new List<SelectListItem>();

                var inProject = from mem in db.Apartines
                                where mem.ProjectId == id && mem.MembruId != owner
                                select mem.MembruId;

                var users = from user in db.Users
                            where (inProject.Contains(user.Id))
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


                Apartine apartine = new Apartine();

                //  apartine.Userz = new Collection<ApplicationUser>();

                apartine.Usr = selectList;
                apartine.ProjectId = Convert.ToInt32(id);


                return View(apartine);
            }
            else
            {
                TempData["message"] = "Nu aveți dreptul să faceți modificări asupra unui proiect ce nu vă aparține!";
                return RedirectToAction("../Project/Show/" + id);

            }


        }

        [HttpDelete]
        public ActionResult Remove(Apartine apartine)
        {
            
            {
                if (true)
                {
                    var id = apartine.ProjectId;
                    var ok = 1;

                    // apartine.Userz = new Collection<ApplicationUser>();
                    foreach (var selectedUser in apartine.SelectedUsers)
                    {
                        var fordelete = (from dlt in db.Apartines
                                        where dlt.MembruId == selectedUser && dlt.ProjectId == id
                                        select dlt.idEchipa).SingleOrDefault(); 

                        

                        apartine = db.Apartines.Find(fordelete);
                        
                        db.Apartines.Remove(apartine);
                        db.SaveChanges();
                    }
                    db.SaveChanges();
                    TempData["message"] = "Membrul a fost eliminat!";
                    return RedirectToAction("../Project/Show/" + id);
                }
                else
                {
                    return RedirectToAction("../Project/Index/");
                }
            }




        }


        [NonAction]
        public IEnumerable<SelectListItem> GetAllUsers()
        {
        
            var selectList = new List<SelectListItem>();
          
            var users = from user in db.Users
                             select user;
         
            foreach (var usr in users)
            {
                
                selectList.Add(new SelectListItem
                {
                    Value = usr.Id.ToString(),
                    Text = usr.Email.ToString()
                });
            }
            
            return selectList;
        }


    }
}