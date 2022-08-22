using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json;
using NuGet.Protocol;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LMS.Controllers
{
    public class AdministratorController : Controller
    {

        //If your context class is named something different,
        //fix this member var and the constructor param
        private readonly LMSContext db;

        public AdministratorController(LMSContext _db)
        {
            db = _db;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Department(string subject)
        {
            ViewData["subject"] = subject;
            return View();
        }

        public IActionResult Course(string subject, string num)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            return View();
        }

        /*******Begin code to modify********/

        /// <summary>
        /// Create a department which is uniquely identified by it's subject code
        /// </summary>
        /// <param name="subject">the subject code</param>
        /// <param name="name">the full name of the department</param>
        /// <returns>A JSON object containing {success = true/false}.
        /// false if the department already exists, true otherwise.</returns>
        public IActionResult CreateDepartment(string subject, string name)
        {
            Department thisDepartment = new Department();
            thisDepartment.Subject = subject;
            thisDepartment.Name = name;

            db.Departments.Add(thisDepartment);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            return Json(new { success = true });
        }



        /// <summary>
        /// Returns a JSON array of all the courses in the given department.
        /// Each object in the array should have the following fields:
        /// "number" - The course number (as in 5530)
        /// "name" - The course name (as in "Database Systems")
        /// </summary>
        /// <param name="subjCode">The department subject abbreviation (as in "CS")</param>
        /// <returns>The JSON result</returns>
        public IActionResult GetCourses(string subject)
        {

            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("this is the subj code");
            Console.WriteLine(subject);
           
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            var courseQuery =

                from Course in db.Courses
                    //join Department in db.Departments
                    //on Department.Subject equals Course.DepartmentSubject
                where Course.DepartmentSubject == subject
                select new
            {
                number = Course.Num,
               name = Course.Name
            //    //   //Course.DepartmentSubject


            };

            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("this is the query");
            Console.WriteLine(courseQuery.ToJson());
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");

            return Json(courseQuery);
        }

        /// <summary>
        /// Returns a JSON array of all the professors working in a given department.
        /// Each object in the array should have the following fields:
        /// "lname" - The professor's last name
        /// "fname" - The professor's first name
        /// "uid" - The professor's uid
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <returns>The JSON result</returns>
        public IActionResult GetProfessors(string subject)
        {

            var profQuery =
                 from Department in db.Departments
                 join Professor in db.Professors
                 on Department.Subject equals Professor.Abbrev

                
                 where Department.Subject == subject
                 select new
                 {
                     lname = Professor.LName,
                     fname = Professor.FName,
                     
                     uid = Professor.ProfUId
                    

                 };


            return Json(profQuery);


        }



        /// <summary>
        /// Creates a course.
        /// A course is uniquely identified by its number + the subject to which it belongs
        /// </summary>
        /// <param name="subject">The subject abbreviation for the department in which the course will be added</param>
        /// <param name="number">The course number</param>
        /// <param name="name">The course name</param>
        /// <returns>A JSON object containing {success = true/false}.
        /// false if the course already exists, true otherwise.</returns>
        public IActionResult CreateCourse(string subject, int number, string name)
        {

            var doesItExist =
                from Course in db.Courses
                where subject == Course.DepartmentSubject
                && number == Course.Num
                && name == Course.Name
                select Course;

            if (doesItExist.Count() > 0)
            {
                return Json(new { success = false });
            }
            else
            {

                var lastCourse = db.Courses.OrderByDescending(Course => Course.CatalogID).FirstOrDefault();


                Course thisCourse = new Course();
                thisCourse.DepartmentSubject = subject;
                thisCourse.Name = name;
                thisCourse.Num = number;
                if (lastCourse != null)
                {
                    thisCourse.CatalogID = lastCourse.CatalogID + 1;
                }
                else
                {
                    thisCourse.CatalogID = 0;
                }

                db.Courses.Add(thisCourse);
                db.SaveChanges();
              

                //Course c = new Course();
                //c.DepartmentSubject = subject;
                //c.Name = name;
                //c.Num = number;
                //db.Courses.Add(c);
                //db.SaveChanges();

                return Json(new { success = true });

            }

        }


          



        /// <summary>
        /// Creates a class offering of a given course.
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <param name="number">The course number</param>
        /// <param name="season">The season part of the semester</param>
        /// <param name="year">The year part of the semester</param>
        /// <param name="start">The start time</param>
        /// <param name="end">The end time</param>
        /// <param name="location">The location</param>
        /// <param name="instructor">The uid of the professor</param>
        /// <returns>A JSON object containing {success = true/false}. 
        /// false if another class occupies the same location during any time 
        /// within the start-end range in the same semester, or if there is already
        /// a Class offering of the same Course in the same Semester,
        /// true otherwise.</returns>
        public IActionResult CreateClass(string subject, int number, string season, int year, DateTime start, DateTime end,
            string location, string instructor)
        {

            //var query =
            //   from co in db.Courses
            //   where subject == co.DepartmentSubject && number == co.Num
            //   select co;
            //int courseID = (int)query.ToArray()[0].CatalogId + 1;


            //check if it's already being offered

            var doesItExist =
                from Class in db.Classes
                where Class.ClassSubj == subject
                && Class.CatalogNum == number
                && Class.Season == season
                && Class.Year == year
                select Class;
            if (doesItExist.Count() > 0)
            {
                return Json(new { success = false });
            }

            //check if the location is available

            var locationCheck =
                from Class in db.Classes
                where Class.Season == season
                && Class.Year == year
                && Class.Location == location
                select Class;
            if (locationCheck.Count() > 0)
            {
                return Json(new { success = false });
            }






            Class thisClass = new Class();

            thisClass.ClassSubj = subject;
            thisClass.CatalogNum = number;
            thisClass.Season = season;
            thisClass.Year = year;
            thisClass.Start = start;
            thisClass.End = end;
            thisClass.Location = location;
            thisClass.ProfUid = instructor;


            var lastClass = db.Classes.OrderByDescending(Class => Class.ClassId).FirstOrDefault();



            if (lastClass != null)
            {
                thisClass.ClassId = lastClass.ClassId + 1;
            }
            else
            {
                thisClass.ClassId = 0;
            }

            //thisClass.ClassId = (int)courseID;
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("THIS IS THE CLASS:");
            Console.WriteLine(thisClass.ToJson());
            Console.WriteLine("\n");
            Console.WriteLine("\n");

            db.Classes.Add(thisClass);
            db.SaveChanges();
           


            return Json(new { success = true});
        }


        /*******End code to modify********/

    }
}

