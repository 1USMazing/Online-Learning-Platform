using ProiectMDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ProiectMDS.Models;

namespace ProiectMDS.Controllers
{
    public class QuizzController : Controller
    {
        private DBEntities1 dbContext = new DBEntities1();

        // GET: Quizz
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult SelectQuizz()
        {
            QuizVM quiz = new QuizVM();
            quiz.ListOfQuizz = dbContext.Quizs.Select(q => new SelectListItem
            {
                Text = q.QuizName,
                Value = q.QuizID.ToString()

            }).ToList();

            return View(quiz);
        }

        [HttpPost]
        public ActionResult SelectQuizz(QuizVM quiz)
        {
            QuizVM quizSelected = dbContext.Quizs.Where(q => q.QuizID == quiz.QuizID).Select(q => new QuizVM
            {
                QuizID = q.QuizID,
                QuizName = q.QuizName,

            }).FirstOrDefault();

            if (quizSelected != null)
            {
                Session["SelectedQuiz"] = quizSelected;

                return RedirectToAction("QuizTest");
            }

            return View();
        }

        [HttpGet]
        public ActionResult QuizTest()
        {
            QuizVM quizSelected = Session["SelectedQuiz"] as QuizVM;
            IQueryable<QuestionVM> questions = null;

            if (quizSelected != null)
            {
                questions = dbContext.Questions.Where(q => q.QuizID == quizSelected.QuizID)
                   .Select(q => new QuestionVM
                   {
                       QuestionID = q.QuestionID,
                       QuestionText = q.QuestionText, 
                       Choices = dbContext.Choices.Where(c => c.QuestionID == q.QuestionID).Select(x => new ChoiceVM
                       {
                           ChoiceID = x.ChoiceID,
                           ChoiceText = x.ChoiceText
                       }).ToList()
                       /*
                       Choices = q.Choices.Select(c => new ChoiceVM
                       {
                           ChoiceID = c.ChoiceID,
                           ChoiceText = c.ChoiceText
                       }).ToList() */

                   }).AsQueryable();


            }

            return View(questions);
        }

        [HttpPost]
        public ActionResult QuizTest(List<QuizAnswersVM> resultQuiz)
        {
            List<QuizAnswersVM> finalResultQuiz = new List<QuizAnswersVM>();

            foreach (QuizAnswersVM answser in resultQuiz)
            {
                QuizAnswersVM result = dbContext.Answers.Where(a => a.QuestionID == answser.QuestionID).Select(a => new QuizAnswersVM
                {
                    QuestionID = a.QuestionID.Value,
                    AnswerQ = a.AnswerText,
                    isCorrect = (answser.AnswerQ.ToLower().Equals(a.AnswerText.ToLower()))

                }).FirstOrDefault();

                finalResultQuiz.Add(result);
            }

            return Json(new { result = finalResultQuiz }, JsonRequestBehavior.AllowGet);
        }
    }

}