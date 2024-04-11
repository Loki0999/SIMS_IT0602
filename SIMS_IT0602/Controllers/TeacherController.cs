using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SIMS_IT0602.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SIMS_IT0602.Controllers
{
    public class TeacherController : Controller
    {
        static List<Teacher> teachers = new List<Teacher>();

        public IActionResult Delete(int Id)
        {
            var teachers = LoadTeacherFromFile("teacher.json");

            //find teacher in an array
            var searchTeacher = teachers.FirstOrDefault(t => t.Id == Id);
            teachers.Remove(searchTeacher);

            //save to file
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(teachers, options);
            //Save file
            using (StreamWriter writer = new StreamWriter("teacher.json"))
            {
                writer.Write(jsonString);
            }
            return RedirectToAction("ManageTeacher");

        }
        [HttpPost]
        public IActionResult Edit(int Id,Teacher teacher)
        {
            var existingTeacher = teachers.FirstOrDefault(t => t.Id == teacher.Id);
            if (existingTeacher == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy giáo viên
            }
            teacher.Id = existingTeacher.Id;
            existingTeacher.Name = teacher.Name;
            existingTeacher.DoB = teacher.DoB;
            existingTeacher.Major = teacher.Major;

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(teachers, options);

            // Lưu thông tin mới vào file
            System.IO.File.WriteAllText("teacher.json", jsonString);

           
            // Chuyển hướng về trang quản lý giáo viên
            return RedirectToAction("ManageTeacher");
        }

        [HttpPost]
        public IActionResult Save(Teacher teacher)
        {
            var existingTeacher = teachers.FirstOrDefault(t => t.Id == teacher.Id);
            if (existingTeacher == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy giáo viên
            }

            // Cập nhật thông tin giáo viên
            existingTeacher.Name = teacher.Name;
            existingTeacher.DoB = teacher.DoB;
            existingTeacher.Major = teacher.Major;

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(teachers, options);

            // Lưu thông tin mới vào file
            System.IO.File.WriteAllText("teacher.json", jsonString);

            // Chuyển hướng về trang quản lý giáo viên
            return RedirectToAction("ManageTeacher");
        }

        [HttpPost] //submit new Teacherasdsadasd
        public IActionResult NewTeacher(Teacher teacher)
        {
            if (teacher.Id == 0 || string.IsNullOrEmpty(teacher.Name) || teacher.DoB == null || string.IsNullOrEmpty(teacher.Major))
            {
                ModelState.AddModelError(string.Empty, "You need to fill out all information in all fields.");
                return View("NewTeacher", teacher);
            }
            teachers.Add(teacher);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(teachers, options);
            //Save file
            using (StreamWriter writer = new StreamWriter("teacher.json"))
            {
                writer.Write(jsonString);
            }

            return RedirectToAction("ManageTeacher", new { teachers = jsonString });
        }
        [HttpGet] //click hyperlink
        public IActionResult Save()
        {
            return View();
        }
        [HttpGet] //click hyperlink
        public IActionResult EditTeacher(int id)
        { 
            var teacher = teachers.FirstOrDefault(s => s.Id == id);
            if (teacher == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy sinh viên
            }
            return View(teacher);
        }
    
        [HttpGet] //click hyperlink
        public IActionResult NewTeacher()
        {
            return View();
        }
        public List<Teacher>? LoadTeacherFromFile(string fileName)
        {
            string readText = System.IO.File.ReadAllText("teacher.json");
            return JsonSerializer.Deserialize<List<Teacher>>(readText);
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Role = HttpContext.Session.GetString("Role");
            // Read a file
            teachers = LoadTeacherFromFile("teacher.json");
            return View(teachers);
        }
        
        public IActionResult ManageTeacher()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Role = HttpContext.Session.GetString("Role");
          //  Read a file
           teachers = LoadTeacherFromFile("teacher.json");
            return View(teachers);
           // Trả về view Manageteacher.cshtml
         return View("Manageteacher");
        }
        public IActionResult ViewTeacher()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Role = HttpContext.Session.GetString("Role");
            //  Read a file
            teachers = LoadTeacherFromFile("teacher.json");
            return View(teachers);
        }
        public IActionResult ViewClass()
        {
            // Xử lý logic để hiển thị thông tin lớp học
            return View();
        }

        

        public IActionResult ViewCourse()
        {
            // Xử lý logic để hiển thị thông tin khóa học
            return View();
        }

    }
}

