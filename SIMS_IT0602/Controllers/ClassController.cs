using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIMS_IT0602.Models;
using System.Text.Json;

namespace SIMS_IT0602.Controllers
{
    public class ClassController : Controller
    {
        static List<Class> classes = new List<Class>();
        public List<Class>? LoadClassFromFile(string fileName)
        {
            string readText = System.IO.File.ReadAllText("class.json");
            return JsonSerializer.Deserialize<List<Class>>(readText);
        }
        public IActionResult ViewClass()
        {

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Role = HttpContext.Session.GetString("Role");
            // Read a file
            classes = LoadClassFromFile("class.json");
            return View(classes);
            // Trả về view Managestudent.cshtml
            // return View("Managestudent");//
        }
        public IActionResult ViewClassStudent()
        {

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Role = HttpContext.Session.GetString("Role");
            // Read a file
            classes = LoadClassFromFile("class.json");
            return View(classes);
            // Trả về view Managestudent.cshtml
            // return View("Managestudent");//
        }
        public IActionResult ManageClass()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Role = HttpContext.Session.GetString("Role");
            // Read a file
            classes = LoadClassFromFile("class.json");
            return View(classes);
            // Trả về view Managestudent.cshtml
            // return View("Managestudent");//
        }
        public IActionResult Delete(int Id)
        {
            var classes = LoadClassFromFile("class.json");

            //find teacher in an array
            var searchClass = classes.FirstOrDefault(t => t.Id == Id);
            classes.Remove(searchClass);

            //save to file
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(classes, options);
            //Save file
            using (StreamWriter writer = new StreamWriter("class.json"))
            {
                writer.Write(jsonString);
            }
            return RedirectToAction("ManageClass");
        }
        [HttpPost] //submit new Teacher
        public IActionResult NewClass(Class @class)
        {
            classes.Add(@class);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(classes, options);
            //Save file
            using (StreamWriter writer = new StreamWriter("class.json"))
            {
                writer.Write(jsonString);
            }

            return RedirectToAction("ManageClass", new { classes = jsonString });
        }
        [HttpGet] //click hyperlink
        public IActionResult NewClass()
        {
            var teachers = LoadTeachersFromFile("teacher.json"); // Đọc danh sách giáo viên
            ViewBag.Teachers = new SelectList(teachers, "Name", "Name");//Truyền danh sách giáo viên tới view
            return View();
        }
        public List<Teacher> LoadTeachersFromFile(string fileName)
        {
            string readText = System.IO.File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<List<Teacher>>(readText);
        }
        [HttpPost]
        public IActionResult Edit(Class @class)
        {
            var existingClass = classes.FirstOrDefault(t => t.Id == @class.Id);
            if (existingClass == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy giáo viên
            }
            existingClass.Name = @class.Name;
            existingClass.Major = @class.Major;
            existingClass.Lecturer = @class.Lecturer;

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(classes, options);
            // Lưu thông tin mới vào file
            System.IO.File.WriteAllText("class.json", jsonString);
            // Chuyển hướng về trang quản lý giáo viên
            return RedirectToAction("ManageClass");
        }
        [HttpPost]
        public IActionResult Save(Class @class)
        {
            var existingClass = classes.FirstOrDefault(t => t.Id == @class.Id);
            if (existingClass == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy giáo viên
            }

            // Cập nhật thông tin giáo viên
            existingClass.Name = @class.Name;
            existingClass.Major = @class.Major;
            existingClass.Lecturer = @class.Lecturer;

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(classes, options);

            // Lưu thông tin mới vào file
            System.IO.File.WriteAllText("class.json", jsonString);

            // Chuyển hướng về trang quản lý giáo viên
            return RedirectToAction("ManageClass");
        }
        [HttpGet] //click hyperlink
        public IActionResult Save()
        {
            return View();
        }
        [HttpGet] //click hyperlink
        public IActionResult EditClass(int id)
        {
            var @class = classes.FirstOrDefault(s => s.Id == id);
            if (@class == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy sinh viên
            }
            var teachers = LoadTeachersFromFile("teacher.json"); // Đọc danh sách giáo viên
            ViewBag.Teachers = teachers; // Truyền danh sách giáo viên tới view
            return View(@class);

        }
    }
}
