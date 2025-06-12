using Microsoft.AspNetCore.Mvc;
using ReaderDiary.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class AccountController : Controller
{
    private static List<User> _users = new List<User>(); // Simulace databáze (nahraď skutečnou DB)

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (_users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("", "E-mail již existuje.");
                return View(model);
            }

            var user = new User
            {
                Id = _users.Count + 1,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password) // Hashování hesla
            };

            _users.Add(user); // Uložení do "databáze"
            return RedirectToAction("Login");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _users.FirstOrDefault(u => u.Email == model.Email);
            if (user == null || user.PasswordHash != HashPassword(model.Password))
            {
                ModelState.AddModelError("", "Neplatný e-mail nebo heslo.");
                return View(model);
            }

            HttpContext.Session.SetString("UserEmail", user.Email);
            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("UserEmail");
        return RedirectToAction("Login");
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
