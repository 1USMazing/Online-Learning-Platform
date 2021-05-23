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

            /*List<int> Iduri = db.Apartines.Where(a => a.MembruId == id).Select(a => a.SubjectId).ToList(); // id-urile proiectelor unde apartin
            var proiecte = db.Subjects.Where(p => Iduri.Contains(p.SubjectId));
            ViewBag.Subjects = proiecte;
            ViewBag.MembruId = id;*/

            var proiecteleLui = User.Identity.GetUserId();

            var subjects = from proj in db.Apartines
                           where proj.MembruId == proiecteleLui
                           select proj.SubjectId;

            var lengthproj = new List<int>(subjects);

            var numeProiekte = from projj in db.Subjects
                               where lengthproj.Contains(projj.SubjectId)
                               select projj;


            ViewBag.Proiektzele = numeProiekte;

            return View();

        }


        public ActionResult New(int? id)
        {

            var owner = (from own in db.Subjects
                        where own.SubjectId == id
                        select own.UserId).SingleOrDefault();

            if (owner == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                var selectList = new List<SelectListItem>();

                var inSubject = from mem in db.Apartines
                                where mem.SubjectId == id
                                select mem.MembruId;

                var users = from user in db.Users
                            where !(inSubject.Contains(user.Id))
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
                apartine.SubjectId = Convert.ToInt32(id);

                    
                return View(apartine);
            }
            else
            {
                TempData["message"] = "Nu aveți dreptul să faceți modificări asupra unui proiect ce nu vă aparține!";
                return RedirectToAction("../Subject/Show/" + id);

            }

            
        }

        [HttpPost]
        public ActionResult New(Apartine apartine)
        {
            try
            {
                if (true)
                {
                    var id = apartine.SubjectId;

                    // apartine.Userz = new Collection<ApplicationUser>();
                    foreach (var selectedUser in apartine.SelectedUsers)
                    {
                        apartine.MembruId = selectedUser;
                        apartine.SubjectId = id;
                        db.Apartines.Add(apartine);
                        db.SaveChanges();
                    }
                    db.SaveChanges();
                    TempData["message"] = "Membrul a fost adăugat!";
                    return RedirectToAction("../Subject/Show/" + id);
                }
                else
                {
                    return RedirectToAction("../Subject/Index/" );
                }    
            }

            catch (Exception e)
            {
                return View("Error");
            }

         
        }

        public ActionResult Remove(int? id)
        {

            var owner = (from own in db.Subjects
                         where own.SubjectId == id
                         select own.UserId).SingleOrDefault();

            if (owner == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                var selectList = new List<SelectListItem>();

                var inSubject = from mem in db.Apartines
                                where mem.SubjectId == id && mem.MembruId != owner
                                select mem.MembruId;

                var users = from user in db.Users
                            where (inSubject.Contains(user.Id))
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
                apartine.SubjectId = Convert.ToInt32(id);


                return View(apartine);
            }
            else
            {
                TempData["message"] = "Nu aveți dreptul să faceți modificări asupra unui proiect ce nu vă aparține!";
                return RedirectToAction("../Subject/Show/" + id);

            }


        }

        [HttpDelete]
        public ActionResult Remove(Apartine apartine)
        {
            
            {
                if (true)
                {
                    var id = apartine.SubjectId;
                    var ok = 1;

                    // apartine.Userz = new Collection<ApplicationUser>();
                    foreach (var selectedUser in apartine.SelectedUsers)
                    {
                        var fordelete = (from dlt in db.Apartines
                                        where dlt.MembruId == selectedUser && dlt.SubjectId == id
                                        select dlt.idEchipa).SingleOrDefault(); 

                        

                        apartine = db.Apartines.Find(fordelete);
                        
                        db.Apartines.Remove(apartine);
                        db.SaveChanges();
                    }
                    db.SaveChanges();
                    TempData["message"] = "Membrul a fost eliminat!";
                    return RedirectToAction("../Subject/Show/" + id);
                }
                else
                {
                    return RedirectToAction("../Subject/Index/");
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