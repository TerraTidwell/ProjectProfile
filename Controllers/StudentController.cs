using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using NuGet.Protocol;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LMS.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        //If your context is named something else, fix this and the
        //constructor param
        private LMSContext db;
        public StudentController(LMSContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Catalog()
        {
            return View();
        }

        public IActionResult Class(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }


        public IActionResult ClassListings(string subject, string num)
        {
            System.Diagnostics.Debug.WriteLine(subject + num);
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            return View();
        }


        /*******Begin code to modify********/

        /// <summary>
        /// Returns a JSON array of the classes the given student is enrolled in.
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester
        /// "year" - The year part of the semester
        /// "grade" - The grade earned in the class, or "--" if one hasn't been assigned
        /// </summary>
        /// <param name="uid">The uid of the student</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {
            var classQuery =
                from Class in db.Classes
                join Student in db.Students
                on Class.ClassId equals Student.ClassId
          
                select new
                {
                    subject = Class.ClassSubj,
                    number = Class.CatalogNum,
                    name = Class.Catalog.Name,
                    season = Class.Season,
                    year = Class.Year,
                    grade = Student.Grade
                };
            return Json(classQuery);

        }

        /// <summary>
        /// Returns a JSON array of all the assignments in the given class that the given student is enrolled in.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The category name that the assignment belongs to
        /// "due" - The due Date/Time
        /// "score" - The score earned by the student, or null if the student has not submitted to this assignment.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="uid"></param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInClass(string subject, int num, string season, int year, string uid)
        {


            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("Start assignment query");
            Console.WriteLine("\n");
            Console.WriteLine("\n");


            var assignmentQ =


                from Student in db.Students
                join cl in db.Classes on Student.ClassId equals cl.ClassId
                join ac in db.AssignmentCategories on cl.ClassId equals ac.ClassId
                join ass in db.Assignments on ac.AcId equals ass.AcId
                join sb in db.Submissions on ass.AId equals sb.AId


                where Student.UId == uid 

                select new
                {
                    aname = ass.Name,
                    cname = ac.Name,
                    due = ass.Due,
                    score = sb.Score
                };

            ///////////////old stuff

            var assignmentQuery =
                from Assignment in db.Assignments
                join AssignmentCategory in db.AssignmentCategories
                on Assignment.AcId equals AssignmentCategory.AcId
                join Submission in db.Submissions
                on Assignment.AId equals Submission.AId
                join cl in db.Classes on AssignmentCategory.ClassId equals cl.ClassId




                where cl.ClassSubj == subject
                && cl.CatalogNum == num
                && cl.Season == season
                && cl.Year == year
                && Submission.UIdNavigation.UId == uid
                select new
                {
                    aname = Assignment.Name,
                    cname = AssignmentCategory.Name,
                    due = Assignment.Due,
                    score = Submission.Score
                };

               

            //Console.WriteLine("\n");
            //Console.WriteLine("\n");
            //Console.WriteLine("\n");
            //Console.WriteLine("Start assignment query");
            //Console.WriteLine("\n");
            //Console.WriteLine("\n");
            //var assignmentQuery =
//                from Assignment in db.Assignments
//                join AssignmentCategory in db.AssignmentCategories
//                on Assignment.AcId equals AssignmentCategory.AcId
//                join Submission in db.Submissions
//                on Assignment.AId equals Submission.AId
////<<<<<<< HEAD
//                where Assignment.Ac.Class.ClassSubj == subject
//                && Assignment.Ac.Class.CatalogNum == num
//                && Assignment.Ac.Class.Season == season
//                && Assignment.Ac.Class.Year == year
//                && AssignmentCategory.Class.Uid == uid
////=======
//                join cl in db.Classes on AssignmentCategory.ClassId equals cl.ClassId
         
//                where cl.ClassSubj == subject
//                && cl.CatalogNum == num
//                && cl.Season == season
//                && cl.Year == year
//                && Submission.UIdNavigation.UId == uid
////>>>>>>> refs/remotes/origin/main
//                select new
//                {
//                    aname = Assignment.Name,
//                    cname = AssignmentCategory.Name,
//                    due = Assignment.Due,
//                    score = Submission.Score
//                };
                
                //select Assignment;


           

            //Console.WriteLine("\n");
            //Console.WriteLine("\n");
            //Console.WriteLine("\n");
            //Console.WriteLine("This is the assignment: \n");
            //Console.WriteLine(assignmentQuery.ToJson());
            //Console.WriteLine("\n");
            //Console.WriteLine("\n");



            return Json(assignmentQ.ToArray());

            return Json(assignmentQ);

        }



        /// <summary>
        /// Adds a submission to the given assignment for the given student
        /// The submission should use the current time as its DateTime
        /// You can get the current time with DateTime.Now
        /// The score of the submission should start as 0 until a Professor grades it
        /// If a Student submits to an assignment again, it should replace the submission contents
        /// and the submission time (the score should remain the same).
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="uid">The student submitting the assignment</param>
        /// <param name="contents">The text contents of the student's submission</param>
        /// <returns>A JSON object containing {success = true/false}</returns>
        public IActionResult SubmitAssignmentText(string subject, int num, string season, int year,
          string category, string asgname, string uid, string contents)

        {
            var assignmentToSubmit =
                from Class in db.Classes
                join AssignmentCategory in db.AssignmentCategories
                on Class.ClassId equals AssignmentCategory.ClassId
                join Assignment in db.Assignments
                on AssignmentCategory.AcId equals Assignment.AcId
                where Class.ClassSubj == subject
                && Class.CatalogNum == num
                && Class.Season == season
                && Class.Year == year
                && AssignmentCategory.Name == category
                && Assignment.Name == asgname
                select Assignment;

            var thisAssignment = assignmentToSubmit.First();

            var doesitExist =
                 from Class in db.Classes
                 join AssignmentCategory in db.AssignmentCategories
                 on Class.ClassId equals AssignmentCategory.ClassId
                 join Assignment in db.Assignments
                 on AssignmentCategory.AcId equals Assignment.AcId
                 join Submission in db.Submissions
                 on Assignment.AId equals Submission.AId
                 where Class.ClassSubj == subject
                 && Class.CatalogNum == num
                 && Class.Season == season
                 && Class.Year == year
                 && AssignmentCategory.Name == category
                 && Assignment.Name == asgname
                 && Submission.UIdNavigation.UId == uid
                 select Submission;

            var oldSubmit = doesitExist.FirstOrDefault();

            if(oldSubmit == null)
            {

                Submission thisSubmit = new Submission();
                thisSubmit.Time = DateTime.Now;
                thisSubmit.UId = uid;
                thisSubmit.Contents = contents;
                thisSubmit.Score = 0;

                //db.Submissions.Add(thisSubmit);

                thisAssignment.Submissions.Add(thisSubmit);

            }
            else
            {
                oldSubmit.Time = DateTime.Now;
                oldSubmit.Contents = contents;
                oldSubmit.UId = uid;
                oldSubmit.Score = 0;
            }




            db.SaveChanges();
            


            return Json(new { success = true });
        }


        /// <summary>
        /// Enrolls a student in a class.
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester</param>
        /// <param name="year">The year part of the semester</param>
        /// <param name="uid">The uid of the student</param>
        /// <returns>A JSON object containing {success = {true/false}. 
        /// false if the student is already enrolled in the class, true otherwise.</returns>
        public IActionResult Enroll(string subject, int num, string season, int year, string uid)
        {
            var findStudent =
                from Student in db.Students
                where Student.UId == uid
                select Student;

            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("THIS IS THE Student: ");
            Console.WriteLine(findStudent.ToJson());
            Console.WriteLine("\n");
            Console.WriteLine("\n");

            var classToAdd =
                from Class in db.Classes
                where Class.ClassSubj == subject
                && Class.CatalogNum == num
                && Class.Season == season
                && Class.Year == year
                select Class;

            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("THIS IS THE CLASS TO ADD THE STUDENT IN: ");
            Console.WriteLine(classToAdd.ToJson());
            Console.WriteLine("\n");
            Console.WriteLine("\n");


            var thisClass = classToAdd.First();

            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("THIS IS THE CLASS TO ADD THE STUDENT IN: ");
            Console.WriteLine(thisClass.ToJson());
            Console.WriteLine("\n");
            Console.WriteLine("\n");

            var thisStudent = findStudent.First();

            if (thisClass.Students.Contains(thisStudent)) 
            {
                return Json(new { success = false });

            }
            else
            {
                thisClass.Students.Add(thisStudent);
                db.SaveChanges();
                return Json(new { success = true });


            }
            //Class thisClass = new Class();
            //thisClass.CatalogNum = num;
            //thisClass.Season = season;
            //thisClass.Year = year;
            //thisClass.ClassSubj = subject;
            //thisClass.Uid = uid;
            //thisClass.ClassId = classID.ToArray()[0];


            

            //Console.WriteLine("\n");
            //Console.WriteLine("\n");
            //Console.WriteLine("\n");
            //Console.WriteLine("THIS IS THE CLASS TO ADD THE STUDENT IN: ");
            //Console.WriteLine(thisClass.ToJson());
            //Console.WriteLine("\n");
            //Console.WriteLine("\n");

            //db.Classes.Add(thisClass);

            //db.SaveChanges();
            

            //return Json(new { success = true });

        }



        /// <summary>
        /// Calculates a student's GPA
        /// A student's GPA is determined by the grade-point representation of the average grade in all their classes.
        /// Assume all classes are 4 credit hours.
        /// If a student does not have a grade in a class ("--"), that class is not counted in the average.
        /// If a student is not enrolled in any classes, they have a GPA of 0.0.
        /// Otherwise, the point-value of a letter grade is determined by the table on this page:
        /// https://advising.utah.edu/academic-standards/gpa-calculator-new.php
        /// </summary>
        /// <param name="uid">The uid of the student</param>
        /// <returns>A JSON object containing a single field called "gpa" with the number value</returns>
        public IActionResult GetGPA(string uid)
        {
            Dictionary<string, double> grades = new Dictionary<string, double>()
            {
                {"A", 4.0 },
                {"A-", 3.7 },
                {"B+", 3.3 },
                {"B", 3.0 },
                {"B-", 2.7 },
                {"C+", 2.3 },
                {"C", 2.0 },
                {"C-", 1.7 },
                {"D+", 1.3 },
                {"D", 1.0 },
                {"D-", 0.7 },
                {"F", 0.0 }
            };

            var query = from s in db.Students
                        where s.UId == uid
                        join c in db.Classes
                        on s.UId equals c.Uid
                        into join1
                        from j1 in join1
                        select s.Grade;

            var count = query.Count();
            var sum = 0.0;

            foreach (var q in query)
            {
                if (q != null)
                {
                    var gradeVal = grades[q];
                    sum += gradeVal;
                }
                else
                {
                    count -= 1;
                }
            }

            var gpa = sum / count;

         
            return Json(gpa);
        }
                
        /*******End code to modify********/

    }
}

