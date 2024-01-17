using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PetimOl.Models;
using System.Security.Claims;

namespace PetimOl.Controllers
{
    public class AccountController : Controller
    {
        private IDapperHelper _dapperHelper;
        public AccountController(IDapperHelper dapperHelper)
        {
            _dapperHelper = dapperHelper;
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserModel model)
        {
            model.CreatedDate = DateTime.Now;
            if (AddUserData(model))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Kayıt işlemi başarısız.";
                return View();
            }

        }

        public IActionResult Login()
        {
            ViewBag.message = "hoşgeldiniz";

            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (CheckUser(username, password))
            {
                var model = _dapperHelper.QueryFirst<UserModel>("select * from users where Email = @username", new { username });
                
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim("UserID",model.UserID.ToString())

                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {

                    AllowRefresh = true,
                    IsPersistent = true// beni hatırla
                };

                 HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Kullanıcı Adı Şifre Yanlış.";
                return View();
            }

        }
        public bool CheckUser(string username, string password)
        {
            string sql = @"select count (*) from Users where Email=@username and Password=@password";
            bool isuser = _dapperHelper.ExecuteScalar(sql, new
            {
                username,
                password
            });
            return isuser;
        }
        public bool AddUserData(UserModel model)
        {
            try
            {
                string sql = @"insert into Users ( Username,Lastname,Password, PhoneNumber, Address,Email,Sex, CreatedDate) values (@Username, @Lastname, @Password, @PhoneNumber, @Address, @Email, @Sex, @CreatedDate)";
                int data = _dapperHelper.Execute(sql, new
                {
                    model.Username,
                    model.LastName,
                    model.Password,
                    model.PhoneNumber,
                    model.Address,
                    model.Email,
                    model.Sex,
                    model.CreatedDate
                });
                return data > 0;

            }
            catch (Exception ex)
            {
                return false;

                throw;
            }
        }
    }
}
