using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIMS_IT0602.Models;

namespace SIMS_IT0602.Controllers
{
    public class AuthenticationController : Controller
    {
        
        [HttpPost]
        public IActionResult Login(User user)
        {

            // Check if username and password are provided
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Pass))
            {
                ViewBag.error = "Please enter both username and password.";
                return View("Login", user);
            }


            // Đọc thông tin người dùng từ file users.json
            List<User> users = LoadUsersFromFile("users.json");
            var result = users.Find(u => u.UserName == user.UserName && u.Pass == user.Pass);

            if (result != null)
            {
                // Lưu thông tin người dùng vào session
                HttpContext.Session.SetString("UserName", result.UserName);
                HttpContext.Session.SetString("Role", result.Role);

                // Chuyển hướng đến trang tương ứng với vai trò của người dùng
                switch (result.Role)
                {
                    case "Admin":
                        return RedirectToAction("Index", "Admin");
                    case "Teacher":
                        return RedirectToAction("Index", "Teacher");
                    case "Student":
                        return RedirectToAction("Index", "Student");
                    default:
                        return RedirectToAction("Index", "Home"); // Chuyển hướng mặc định nếu không phân quyền
                }
            }
            else
            {
                // Thông báo lỗi nếu tài khoản không hợp lệ
                ViewBag.error = "Invalid user!";
                return View("Login");
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Phương thức này đọc dữ liệu người dùng từ file JSON
        public List<User> LoadUsersFromFile(string fileName)
        {
            // Đảm bảo rằng file tồn tại
            if (System.IO.File.Exists(fileName))
            {
                string readText = System.IO.File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<List<User>>(readText);
            }
            else
            {
                return new List<User>(); // Trả về danh sách trống nếu file không tồn tại
            }
        }
        [HttpPost] //submit new Teacher
        public IActionResult Register(User user)
        {
            string role = Request.Form["Role"];
            string userName = user.UserName;
            string password = user.Pass;

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                // Nếu không, trả về thông báo lỗi và không thực hiện đăng ký
                ViewBag.error = "Please fill in all required fields.";
                return View();
            }

            // Gán giá trị Role vào đối tượng user
            user.Role = role;
            List<User> users = LoadUsersFromFile("users.json");
            users.Add(user);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(users, options);
            //Save file
            using (StreamWriter writer = new StreamWriter("users.json"))
            {
                writer.Write(jsonString);
            }

            return RedirectToAction("Login", new { students = jsonString });
        }
        [HttpGet] //click hyperlink
        public IActionResult Register()
        {
            return View();
        }
    }
}
