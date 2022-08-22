using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LMS_CustomIdentity.Controllers
{
    [Authorize(Roles = "Professor")]
    public class ProfessorController : Controller
    {

        //If your context is named something else, fix this
        //and the constructor param
        private readonly LMSContext db;

        public ProfessorController(LMSContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Students(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
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

        public IActionResult Categories(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult CatAssignments(string subject, string num, string season, string year, string cat)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
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

        public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            ViewData["uid"] = uid;
            return View();
        }

        /*******Begin code to modify********/


        /// <summary>
        /// Returns a JSON array of all the students in a class.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "dob" - date of birth
        /// "grade" - the student's grade in this class
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetStudentsInClass(string subject, int num, string season, int year)
        {
            Console.WriteLine("\n");
            Console.WriteLine("begin getstudentsinclass\n");
            //Console.WriteLine(query.ToJson());
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");

            var students =
                from Class in db.Classes
                join Student in db.Students
                on Class.ClassId equals Student.ClassId
                where Class.ClassSubj == subject
                && Class.CatalogNum == num
                && Class.Season == season
                && Class.Year == year

                           select new
                           {
                               fname = Student.FName,
                               lname = Student.LName,
                               uid = Student.UId,
                               dob = Student.Dob,
                               grade = Student.Grade

                           };

            var studentToGrade =
                from Class in db.Classes
                join Student in db.Students
                on Class.ClassId equals Student.ClassId
                where Class.ClassSubj == subject
                && Class.CatalogNum == num
                && Class.Season == season
                && Class.Year == year
                select Student;





            //var studentGrade = studentToGrade.FirstOrDefault();

            //var score = // points earned in class divided by possible points in class, times 100 

            //if (score >= 93)
            //    studentGrade.Grade = "A";
            //else if (score >= 90)
            //    studentGrade.Grade = "A-";
            //else if (score >= 87)
            //    studentGrade.Grade = "B+";
            //else if (score >= 83)
            //    studentGrade.Grade = "B";
            //else if (score >= 80)
            //    studentGrade.Grade = "B-";
            //else if (score >= 77)
            //    studentGrade.Grade = "C+";
            //else if (score >= 73)
            //    studentGrade.Grade = "C";
            //else if (score >= 70)
            //    studentGrade.Grade = "C-";
            //else if (score >= 67)
            //    studentGrade.Grade = "D+";
            //else if (score >= 63)
            //    studentGrade.Grade = "D";
            //else if (score >= 60)
            //    studentGrade.Grade = "D-";
            //else
            //    studentGrade.Grade = "E";

            //db.SaveChanges();



            Console.WriteLine("\n");
            Console.WriteLine("end of getstudentsinclass\n");
            Console.WriteLine(students.ToJson());
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            //var classID = from co in db.Courses
            //              join cl in db.Classes on co.ClassId equals cl.ClassId
            //              where subject == co.DeptSubject && num.ToString() == co.Num.ToString()
            //                  && season == cl.Season && year == cl.Year
            //              select cl.ClassId;

            //var query =
            //    from e in db.Enroll
            //    join s in db.Students on e.StudentId equals s.UId
            //    where classID.ToString() == e.ClassId.ToString()
            //    select new
            //    {
            //        fname = s.FName,
            //        lname = s.LName,
            //        uid = s.UId,
            //        dob = s.Dob,
            //        grade = e.Grade
            //    };

            return Json(students);

        }



        /// <summary>
        /// Returns a JSON array with all the assignments in an assignment category for a class.
        /// If the "category" parameter is null, return all assignments in the class.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The assignment category name.
        /// "due" - The due DateTime
        /// "submissions" - The number of submissions to the assignment
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class, 
        /// or null to return assignments from all categories</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category)
        {

            var query1 = from c in db.Courses
                         where c.DepartmentSubject == subject && c.Num == num
                         join cl in db.Classes
                         on c.Num equals cl.CatalogNum
                         into join1
                         from j1 in join1
                         where j1.Year == year && j1.Season == season
                         join ac in db.AssignmentCategories
                         on j1.ClassId equals ac.ClassId
                         into join2
                         from j2 in join2
                         select j2;

            if (category != null)
            {
                var query2 = from q in query1
                             where q.Name == category
                             select q;

                var query3 = from q2 in query2
                             join a in db.Assignments
                             on q2.AcId equals a.AcId
                             into join3
                             from j3 in join3
                             select new { aname = j3.Name, cname = q2.Name, due = j3.Due, submissions = (from s in db.Submissions where s.AId == j3.AId select s).Count() };
                return Json(query3);
            }
            else
            {
                var query2 = query1;

                var query3 = from q2 in query2
                             join a in db.Assignments
                             on q2.AcId equals a.AcId
                             into join3
                             from j3 in join3
                             select new { aname = j3.Name, cname = q2.Name, due = j3.Due, submissions = (from s in db.Submissions where s.AId == j3.AId select s).Count() };
                return Json(query3);
            }
        }


        /// <summary>
        /// Returns a JSON array of the assignment categories for a certain class.
        /// Each object in the array should have the folling fields:
        /// "name" - The category name
        /// "weight" - The category weight
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentCategories(string subject, int num, string season, int year)
        {
            var query = from c in db.Courses
                        where c.DepartmentSubject == subject && c.Num == num
                        join cl in db.Classes
                        on c.Num equals cl.CatalogNum
                        into join1
                        from j1 in join1
                        where j1.Season == season && j1.Year == (uint)year
                        join ac in db.AssignmentCategories
                        on j1.ClassId equals ac.ClassId
                        into join2
                        from j2 in join2
                        select new { name = j2.Name, weight = j2.Weight };
            return Json(query);
        }

        /// <summary>
        /// Creates a new assignment category for the specified class.
        /// If a category of the given class with the given name already exists, return success = false.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The new category name</param>
        /// <param name="catweight">The new category weight</param>
        /// <returns>A JSON object containing {success = true/false} </returns>
        public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight)
        {
            //if name already exists, return
            // return Json(new { success = false });

            //var semester = season + year.ToString();
            var thisClass =
                from Class in db.Classes
                where Class.ClassSubj == subject
                && Class.CatalogNum == num
                && Class.Season == season
                && Class.Year == year
                select Class;

            var classForAC = thisClass.FirstOrDefault();

            var catandweight =
                  from Class in db.Classes
                  join AssignmentCategory in db.AssignmentCategories
                  on Class.ClassId equals AssignmentCategory.ClassId
                  where AssignmentCategory.Name == category
                  && AssignmentCategory.Weight == catweight
                  select AssignmentCategory;

            var catAndWeight = catandweight.FirstOrDefault();

            var assignmentCatId =
                from AssignmentCategory in db.AssignmentCategories
                select AssignmentCategory;

            var lastID = assignmentCatId.OrderByDescending(AssignmentCategory => AssignmentCategory.AcId).FirstOrDefault();



            if (classForAC.AssignmentCategories.Contains(catAndWeight))
            {
                return Json(new { success = false });
            }
            else
            {
                AssignmentCategory thisAC = new AssignmentCategory();
                thisAC.Weight = (sbyte)catweight;
                thisAC.Name = category;
                thisAC.Class = classForAC;


                if (lastID.AcId >= 0)
                {
                    thisAC.AcId = lastID.AcId + 1;

                }

                else
                {
                    thisAC.AcId = 0;
                }


                db.AssignmentCategories.Add(thisAC);
                db.SaveChanges();

                return Json(new { success = true });

            }


           
        }

        /// <summary>
        /// Creates a new assignment for the given class and category.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="asgpoints">The max point value for the new assignment</param>
        /// <param name="asgdue">The due DateTime for the new assignment</param>
        /// <param name="asgcontents">The contents of the new assignment</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult CreateAssignment(string subject, int num, string season, int year, string category, string asgname, int asgpoints, DateTime asgdue, string asgcontents)
        {

            //var semester = season + year.ToString();
            var thisClass =
                from Class in db.Classes
                where Class.ClassSubj == subject
                && Class.CatalogNum == num
                && Class.Season == season
                && Class.Year == year
                select Class;

            var classForAssignment = thisClass.FirstOrDefault();

            var thisAC =
                from Class in db.Classes
                join AssignmentCategory in db.AssignmentCategories
                on Class.ClassId equals AssignmentCategory.ClassId
                where AssignmentCategory.Name == category
             
                select AssignmentCategory;

            var this_AC = thisAC.FirstOrDefault();

            var doesItExist =
                  from Class in db.Classes
                  join AssignmentCategory in db.AssignmentCategories
                  on Class.ClassId equals AssignmentCategory.ClassId
                  join Assignment in db.Assignments
                  on AssignmentCategory.AcId equals Assignment.AcId
                  where Assignment.Name == asgname
                  && Assignment.Points == asgpoints
                  && Assignment.Due == asgdue
                  && Assignment.Contents == asgcontents
                  select Assignment;

            var oldAssignment = doesItExist.FirstOrDefault();

            var assignmentId =

                from Assignment in db.Assignments
                select Assignment;

            var thiscategoryID =
                 from Class in db.Classes 
                 where Class.ClassSubj == subject
                 && Class.CatalogNum == num
                 && Class.Season == season
                 && Class.Year == year
                 join ac in db.AssignmentCategories on Class.ClassId equals ac.ClassId
                 where ac.Name == category
                select ac.AcId;

            var lastID = assignmentId.OrderByDescending(Assignments => Assignments.AId).FirstOrDefault();


            if (this_AC.Assignments.Contains(oldAssignment))
            {
                return Json(new { success = false });
            }
            else
            {
                Assignment newAssignment = new Assignment();
                newAssignment.Name = asgname;
                newAssignment.Points = asgpoints;
                newAssignment.Due = asgdue;
                newAssignment.Contents = asgcontents;
                newAssignment.AcId = thiscategoryID.First();

                if (lastID.AId >= 0)
                {
                    newAssignment.AId = lastID.AId + 1;

                }

                else
                {
                    newAssignment.AId = 0;
                }


                db.Assignments.Add(newAssignment);
                db.SaveChanges();

                return Json(new { success = true });

            }
       

        }


        /// <summary>
        /// Gets a JSON array of all the submissions to a certain assignment.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "time" - DateTime of the submission
        /// "score" - The score given to the submission
        /// 
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname)
        {
            var submissionQuery =
                from Class in db.Classes
                where Class.ClassSubj == subject
                && Class.CatalogNum == num
                && Class.Season == season
                && Class.Year == year
                join AssignmentCategory in db.AssignmentCategories
                on Class.ClassId equals AssignmentCategory.ClassId
                where AssignmentCategory.Name == category
                join Assignment in db.Assignments
                on AssignmentCategory.AcId equals Assignment.AcId
                where Assignment.Name == asgname
                join Submission in db.Submissions
                on Assignment.AId equals Submission.AId
                join Student in db.Students
                on Submission.UId equals Student.UId
                select new
                {
                    fname = Student.FName,
                    lname = Student.LName,
                    uid = Student.UId,
                    time = Submission.Time,
                    score = Submission.Score
                };


            return Json(submissionQuery);

        }


        /// <summary>
        /// Set the score of an assignment submission
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <param name="uid">The uid of the student who's submission is being graded</param>
        /// <param name="score">The new score for the submission</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult GradeSubmission(string subject, int num, string season, int year, string category, string asgname, string uid, int score)
        {

            var thisSubmission =
                from Class in db.Classes
                where Class.ClassSubj == subject
                && Class.CatalogNum == num
                && Class.Season == season
                && Class.Year == year
                join AssignmentCategory in db.AssignmentCategories
                on Class.ClassId equals AssignmentCategory.ClassId
                join Assignment in db.Assignments
                on AssignmentCategory.AcId equals Assignment.AcId
                join Submission in db.Submissions
                on Assignment.AId equals Submission.AId
                where Submission.UId == uid
                select Submission;

            var thisSubmit = thisSubmission.FirstOrDefault();

            thisSubmit.Score = score;

            var allSubmission =
               from Class in db.Classes
               where Class.ClassSubj == subject
               && Class.CatalogNum == num
               && Class.Season == season
               && Class.Year == year
               join AssignmentCategory in db.AssignmentCategories
               on Class.ClassId equals AssignmentCategory.ClassId
               join Assignment in db.Assignments
               on AssignmentCategory.AcId equals Assignment.AcId
               join Submission in db.Submissions
               on Assignment.AId equals Submission.AId
               where Submission.UId == uid
               select Submission.Score;

            var allPoints =
        from Class in db.Classes
        where Class.ClassSubj == subject
        && Class.CatalogNum == num
        && Class.Season == season
        && Class.Year == year
        join AssignmentCategory in db.AssignmentCategories
        on Class.ClassId equals AssignmentCategory.ClassId
        join Assignment in db.Assignments
        on AssignmentCategory.AcId equals Assignment.AcId
        join Submission in db.Submissions
        on Assignment.AId equals Submission.AId
        where Submission.UId == uid
        select Assignment.Points;

            var totalScore = allSubmission.Sum();
            var totalPoints = allPoints.Sum();



            var gradeScore = (totalScore/totalPoints) * 100;

            var letterGrade =
                from Student in db.Students
                join Class in db.Classes
                on Student.ClassId equals Class.ClassId
                where Student.UId == uid
                select Student;

            var studentGrade = letterGrade.FirstOrDefault();

            if (gradeScore >= 93)
                studentGrade.Grade = "A";
            else if(gradeScore >= 90)
                studentGrade.Grade = "A-";
            else if (gradeScore >= 87)
                studentGrade.Grade = "B+";
            else if (gradeScore >= 83)
                studentGrade.Grade = "B";
            else if (gradeScore >= 80)
                studentGrade.Grade = "B-";
            else if (gradeScore >= 77)
                studentGrade.Grade = "C+";
            else if (gradeScore >= 73)
                studentGrade.Grade = "C";
            else if (gradeScore >= 70)
                studentGrade.Grade = "C-";
            else if (gradeScore >= 67)
                studentGrade.Grade = "D+";
            else if (gradeScore >= 63)
                studentGrade.Grade = "D";
            else if (gradeScore >= 60)
                studentGrade.Grade = "D-";
            else
                studentGrade.Grade = "--";

            db.SaveChanges();
            //var classID = from cl in db.Classes
            //              where subject == cl.ClassSubj
            //              && num == cl.CatalogNum
            //              && season == cl.Season
            //              && year == cl.Year
            //              select cl.ClassId;

            ////update the score

            //var query =
            //   from ac in db.AssignmentCategories
            //   join a in db.Assignments on ac.AcId equals a.AcId
            //   join s in db.Submissions on a.AId equals s.AId
            //   where ac.Name == category && a.Name == asgname && s.UId == uid
            //   select s;

            //if(query.Count() != 0)
            //{
            //    var thisSubmission = query.First();
            //    score = (int)thisSubmission.Score;
            //    //divide total number of points from submissions by total asgpoints
            //    //use this to calculate the grade
            //    //if grade = null
            //    //then clgrade equals '--'
            //    //else if 90 all that
            //    db.SaveChanges();
                return Json(new { success = true });

            
        }


        /// <summary>
        /// Returns a JSON array of the classes taught by the specified professor
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester in which the class is taught
        /// "year" - The year part of the semester in which the class is taught
        /// </summary>
        /// <param name="uid">The professor's uid</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {




            var classQuery =
              
               //into table2

               //from Class in table2
               //join Submission in db.Submissions
               //on Class.ClassId equals Submission.AIdNavigation.Ac.ClassId
               //into table1
               //from Submission in table1
               from Class in db.Classes
               where Class.ProfUid == uid
               select new
               {
                   subject = Class.ClassSubj,
                   number = Class.CatalogNum,
                   name = Class.Catalog.Name,
                   season = Class.Season,
                   year = Class.Year,

               };
            return Json(classQuery);

        }


        //{
        //    var qry = from cl in db.Classes
        //              join co in db.Courses on cl.ClassId equals co.ClassId
        //              join d in db.Departments on co.DepartmentSubject equals d.Subject
        //              where cl.ProfUid == uid
        //              select new
        //              {
        //                  subject = d.Subject,
        //                  number = co.Num,
        //                  name = co.Name,
        //                  season = cl.Season,
        //                  year = cl.Year
        //              };
        //    return Json(qry.ToArray());
        //}

        //var query =
        //    from p in db.Professors
        //    where p.ProfUId == uid
        //    join cl in db.Classes on p.ProfUId equals cl.ProfUid
        //    join c in db.Courses on cl.ClassId equals c.ClassId
        //    select new
        //    {
        //    subject = c.DepartmentSubject,
        //        number = c.Num,
        //        name = c.Name,
        //        season = cl.Season,
        //        year = cl.Year
        //    };
        //Console.WriteLine("\n");
        //Console.WriteLine("end of getmyclasses\n");
        //Console.WriteLine(query.ToJson());
        //Console.WriteLine("\n");
        //Console.WriteLine("\n");
        //Console.WriteLine("\n");



        /*******End code to modify********/
    }
}

