using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIMS_IT0602.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SIMS_IT0602.Controllers
{
    public class CourseController : Controller
    {
        static List<Course> courses = new List<Course>();
        public IActionResult Delete(int Id)
        {
            var courses = LoadCourseFromFile("course.json");

            //find teacher in an array
            var searchCourse = courses.FirstOrDefault(t => t.Id == Id);
            courses.Remove(searchCourse);

            //save to file
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(courses, options);
            //Save file
            using (StreamWriter writer = new StreamWriter("course.json"))
            {
                writer.Write(jsonString);
            }
            return RedirectToAction("ManageCourse");

        }
        [HttpPost]
        public IActionResult CreateCourse(Course course)
        {
            courses.Add(course);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(courses, options);
            //Save file
            using (StreamWriter writer = new StreamWriter("course.json"))
            {
                writer.Write(jsonString);
            }

            return RedirectToAction("ManageCourse", new { courses = jsonString });
        }
        [HttpGet]
        public IActionResult CreateCourse()
        {
            var teachers = LoadTeachersFromFile("teacher.json");// Đọc danh sách giáo viên
            var classes = LoadClassesFromFile("class.json");
            ViewBag.Teachers = teachers; // Truyền danh sách giáo viên tới view
            ViewBag.Classes = classes;
            return View();
        }
        public List<Class> LoadClassesFromFile(string fileName)
        {
            string readText = System.IO.File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<List<Class>>(readText);
        }
        public List<Teacher> LoadTeachersFromFile(string fileName)
        {
            string readText = System.IO.File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<List<Teacher>>(readText);
        }
        [HttpPost]
        public IActionResult Edit(Course course)
        {
            var existingCourse = courses.FirstOrDefault(t => t.Id == course.Id);
            if (existingCourse == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy giáo viên
            }
            existingCourse.Name = course.Name;
            existingCourse.Class = course.Class;
            existingCourse.Major = course.Major;
            existingCourse.Lecturer = course.Lecturer;

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(courses, options);
            // Lưu thông tin mới vào file
            System.IO.File.WriteAllText("course.json", jsonString);
            // Chuyển hướng về trang quản lý giáo viên
            return RedirectToAction("ManageCourse");
        }
        [HttpPost]
        public IActionResult Save(Course course)
        {
            var existingCourse = courses.FirstOrDefault(t => t.Id == course.Id);
            if (existingCourse == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin khoa hoc
            existingCourse.Name = course.Name;
            existingCourse.Class = course.Class;
            existingCourse.Major = course.Major;
            existingCourse.Lecturer = course.Lecturer;

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(courses, options);

            // Lưu thông tin mới vào file
            System.IO.File.WriteAllText("course.json", jsonString);

            // Chuyển hướng về trang quản lý giáo viên
            return RedirectToAction("ManageCourse");
        }
        [HttpPost]
        public ActionResult Cancel(string returnUrl)
        {
            // Redirect to the ManageCourse action method
            return RedirectToAction("ManageCourse");
        }
        [HttpGet] //click hyperlink
        public IActionResult Save()
        {
            return View();
        }
        [HttpGet] //click hyperlink
        public IActionResult EditCourse(int id)
        {
            var course = courses.FirstOrDefault(s => s.Id == id);
            if (course == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy sinh viên
            }
            var teachers = LoadTeachersFromFile("teacher.json");// Đọc danh sách giáo viên
            var classes = LoadClassesFromFile("class.json");
            ViewBag.Teachers = teachers; // Truyền danh sách giáo viên tới view
            ViewBag.Classes = classes;
            return View(course);
        }
        public List<Course>? LoadCourseFromFile(string fileName)
        {
            string readText = System.IO.File.ReadAllText("course.json");
            return JsonSerializer.Deserialize<List<Course>>(readText);
        }
        public IActionResult ManageCourse()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Role = HttpContext.Session.GetString("Role");
            // Read a file
            courses = LoadCourseFromFile("course.json");
            return View(courses);
            // Trả về view Managestudent.cshtml
            // return View("Managestudent");//
        }
        // GET: /<controller>/
        public IActionResult ViewCourse()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Role = HttpContext.Session.GetString("Role");
            // Read a file
            courses = LoadCourseFromFile("course.json");
            return View(courses);
        }
        public IActionResult ViewCourseStudent()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Role = HttpContext.Session.GetString("Role");
            // Read a file
            courses = LoadCourseFromFile("course.json");
            return View(courses);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

