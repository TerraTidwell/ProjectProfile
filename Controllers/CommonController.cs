using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LMS.Controllers
{
    public class CommonController : Controller
    {
        //If your context class is named differently, fix this
        //and the constructor parameter
        private readonly LMSContext db;

        public CommonController(LMSContext _db)
        {
            db = _db;
        }

        /*******Begin code to modify********/

        /// <summary>
        /// Retreive a JSON array of all departments from the database.
        /// Each object in the array should have a field called "name" and "subject",
        /// where "name" is the department name and "subject" is the subject abbreviation.
        /// </summary>
        /// <returns>The JSON array</returns>
        public IActionResult GetDepartments()
        {

            var deptQuery =
                from Depart in db.Departments
                select new
                {
                    Depart.Name,
                    Depart.Subject
                };

            return Json(deptQuery.ToArray());

        }



        /// <summary>
        /// Returns a JSON array representing the course catalog.
        /// Each object in the array should have the following fields:
        /// "subject": The subject abbreviation, (e.g. "CS")
        /// "dname": The department name, as in "Computer Science"
        /// "courses": An array of JSON objects representing the courses in the department.
        ///            Each field in this inner-array should have the following fields:
        ///            "number": The course number (e.g. 5530)
        ///            "cname": The course name (e.g. "Database Systems")
        /// </summary>
        /// <returns>The JSON array</returns>
        public IActionResult GetCatalog()
        {
            var depts = from d in db.Departments
                        select new {
                         subject = d.Subject,
                         dname = d.Name,
                         courses =
                            (from c in d.Courses
                            //where c.DepartmentSubject == d.Subject
                            select new {
                                       number = c.Num, cname = c.Name }).AsEnumerable()
                        };

            return Json(depts.ToArray());
        }

        /// <summary>
        /// Returns a JSON array of all class offerings of a specific course.
        /// Each object in the array should have the following fields:
        /// "season": the season part of the semester, such as "Fall"
        /// "year": the year part of the semester
        /// "location": the location of the class
        /// "start": the start time in format "hh:mm:ss"
        /// "end": the end time in format "hh:mm:ss"
        /// "fname": the first name of the professor
        /// "lname": the last name of the professor
        /// </summary>
        /// <param name="subject">The subject abbreviation, as in "CS"</param>
        /// <param name="number">The course number, as in 5530</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetClassOfferings(string subject, int number)
        {
            var query =
             from cl in db.Classes
             join p in db.Professors
             on cl.ProfUid equals p.ProfUId
             where cl.ClassSubj == subject && cl.CatalogNum == number

             select new
             {
                 season = cl.Season,
                 year = cl.Year,
                 location = cl.Location,
                 start = cl.Start,
                 end = cl.End,
                 fname = p.FName,
                 lname = p.LName
             };

            return Json(query);
        }

        /// <summary>
        /// This method does NOT return JSON. It returns plain text (containing html).
        /// Use "return Content(...)" to return plain text.
        /// Returns the contents of an assignment.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment in the category</param>
        /// <returns>The assignment contents</returns>
        public IActionResult GetAssignmentContents(string subject, int num, string season, int year, string category, string asgname)
        {

            var contents =
                from Assignment in db.Assignments.DefaultIfEmpty()
                where Assignment.Ac.Class.Catalog.Name != null
                && Assignment.Ac.Class.Catalog.Name == subject
                && Assignment.Ac.Class.CatalogNum == num
                && Assignment.Ac.Class.Season == season
                && Assignment.Ac.Class.Year == year
                && Assignment.Name == asgname
                select Assignment.Contents;

            string contents1 = contents.ToString();

            //var classID = from co in db.Courses
            //              join cl in db.Classes on co.ClassId equals cl.ClassId
            //              where subject == co.DeptSubject && num.ToString() == co.Num.ToString()
            //                  && season == cl.Semester.ToString() && year == cl.Year//need to add year
            //              select cl.ClassId;

            //var query =
            //    from ac in db.AssignmentCategories
            //    join a in db.Assignments on ac.AcId equals a.AcId
            //    where ac.ClassId == a.AcId && ac.Name == category && a.Name == asgname //not right
            //      select a.Contents;

            return Content(contents1);

         
        }


        /// <summary> 
        /// This method does NOT return JSON. It returns plain text (containing html).
        /// Use "return Content(...)" to return plain text.
        /// Returns the contents of an assignment submission.
        /// Returns the empty string ("") if there is no submission.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment in the category</param>
        /// <param name="uid">The uid of the student who submitted it</param>
        /// <returns>The submission text</returns>
        public IActionResult GetSubmissionText(string subject, int num, string season, int year, string category, string asgname, string uid)
        {
            var classID = from co in db.Courses
                          join cl in db.Classes on co.Num equals cl.CatalogNum
                          where subject == co.DepartmentSubject && num == co.Num
                              && season == cl.Season && year == cl.Year
                          select cl.ClassId;
            var query =
                from ac in db.AssignmentCategories
                join a in db.Assignments on ac.AcId equals a.AcId
                join s in db.Submissions on a.AId equals s.AId
                where ac.ClassId == a.AcId && ac.Name == category //not right
                    && a.Name == asgname && uid == s.UId
                select s.Contents;

            if (query.Count() == 0)
                return Content("");

            return Content(query.ToArray()[0]);
        }


        /// <summary>
        /// Gets information about a user as a single JSON object.
        /// The object should have the following fields:
        /// "fname": the user's first name
        /// "lname": the user's last name
        /// "uid": the user's uid
        /// "department": (professors and students only) the name (such as "Computer Science") of the department for the user. 
        ///               If the user is a Professor, this is the department they work in.
        ///               If the user is a Student, this is the department they major in.    
        ///               If the user is an Administrator, this field is not present in the returned JSON
        /// </summary>
        /// <param name="uid">The ID of the user</param>
        /// <returns>
        /// The user JSON object 
        /// or an object containing {success: false} if the user doesn't exist
        /// </returns>
        public IActionResult GetUser(string uid)
        {

            
            var query =
                from s in db.Students
                join d in db.Departments on s.Major equals d.Subject
                where uid == s.UId
                select new { fname = s.FName, lname = s.LName, uid, department = d.Name };
            if (query.Count() == 1)
                return Json(query.ToArray()[0]);

            // Check if the user is a professor
            var query1 =
                from p in db.Professors
                join d in db.Departments on p.Abbrev equals d.Subject 
                where uid == p.ProfUId
                select new { fname = p.FName, lname = p.LName, uid, department = d.Name };
            if (query1.Count() == 1)
                return Json(query1.ToArray()[0]);

            // Check if the user is an administrator
            var query2 =
                from a in db.Administrators
                where uid == a.UId
                select new { fname = a.FName, lname = a.LName, uid };
            if (query.Count() == 1)
            {
                return Json(query2.ToArray()[0]);
            }
            return Json(new { success = false });
        }


        /*******End code to modify********/
    }
}

