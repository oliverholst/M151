using Garage.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace Garage.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Admin()
        {
            return View();
        }        

        private IAccount db = new MSSqlAccount();
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LogoutUser()
        {
            HttpContext.Session.SetInt32("IsAdmin", 0);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new Account());
        }

        /// <summary>
        /// Login 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UserCreate()
        {
            return View(new Account());
        }

        [HttpPost]
        public IActionResult LoginUser(Login login)
        {
            try
            {
                MD5 md5 = MD5.Create();

                var connection = db.Connect();
                string commandText = "SELECT PK_Account,Password FROM account WHERE Email = @Email";
                    using (var command = new SqlCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@Email", login.Email);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        byte[] hashPassword = Encoding.Default.GetBytes(login.Password);
                        byte[] hashStore = md5.ComputeHash(hashPassword);
                        
                        if (reader.Read())
                        {
                            Console.WriteLine(String.Format("{0}", reader["password"]));

                            if (!String.IsNullOrEmpty((string)reader["password"]))
                            {
                                var testPassword =(string)reader["password"];
                                var ConvertedHashPassword = Convert.ToBase64String(hashStore);
             
                                if (testPassword == ConvertedHashPassword)
                                {
                                    HttpContext.Session.SetInt32("Account PK", (int)reader["PK_Account"]);
                                    HttpContext.Session.SetInt32("IsAdmin", 1);
                                    return RedirectToAction("Admin");
                                }
                            }
                        }
                    }
                        TempData["Message"] = "Login failed.User name or password supplied doesn't exist.";

                    db.Close();
                    }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Login failed.Error - " + ex.Message;
            }
            return RedirectToAction("Login");
        }

        /// <summary>
        /// User erzeugen
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UserCreate(Account account)
        {
            try
            {
                db.Connect();
                db.Create(account);

            }
            catch (Exception ex)
            {
                return RedirectToAction("Groesse", new { msg = ex.Message });
            }
            finally
            {
                db.Close();
            }
            return RedirectToAction("Login");
        }
    }
}
