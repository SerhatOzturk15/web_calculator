using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCalculator.Models;
using System.IO;
using System.Web.Routing;

namespace WebCalculator.Controllers
{
    public class HomeController : Controller
    {

        /// <summary>
        /// Index Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        /// <summary>
        /// login method
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View("Login");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Logout()
        {
            return Json(Url.Action("Login", "Home"));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserCredentials user)
        {
            if (ModelState.IsValid)
            {
                if (!System.IO.File.Exists("Content/LoginTextFile.txt"))
                {
                    var path = Server.MapPath(@"~/Content/LoginTextFile.txt");
                    var lines = System.IO.File.ReadAllLines(path);
                    if (user.UserName.Equals(lines[0]) && user.Password.Equals(lines[1]))
                    {
                        Session["UserID"] = lines[0];
                        ViewBag.UserName = lines[0];
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Message = "Your UserName or Password is wrong. Please enter your credentials again.";
                    }
                }
                    
            }

            return View(user);
        }



        /// <summary>
        /// All calculations are made here
        /// </summary>
        /// <param name="number"></param>
        /// <param name="operatorType"></param>
        /// <returns></returns>
        public string Calculate(string textArea, string nextOperator)
        {
            double result = 0;
            string operatorType = string.Empty;
            double firstNumber = 0;
            double secondNumber = 0;
            //seperate and find the operator type
            if (textArea.Contains("+"))
            {
                operatorType = "+";
                firstNumber = Convert.ToDouble(textArea.Split('+').First());
                secondNumber = Convert.ToDouble(textArea.Split('+').Last());
            }
            else if (textArea.Contains("-"))
            {
                operatorType = "-";
                firstNumber = Convert.ToDouble(textArea.Split('-').First());
                secondNumber = Convert.ToDouble(textArea.Split('-').Last());
            }
            else if (textArea.Contains("*"))
            {
                operatorType = "*";
                firstNumber = Convert.ToDouble(textArea.Split('*').First());
                secondNumber = Convert.ToDouble(textArea.Split('*').Last());
            }
            else if (textArea.Contains("/"))
            {
                operatorType = "/";
                firstNumber = Convert.ToDouble(textArea.Split('/').First());
                secondNumber = Convert.ToDouble(textArea.Split('/').Last());
            }
            else if(nextOperator != "=")
            {
                return textArea + nextOperator;
            }
            //calculation area
            switch (operatorType)
            {
                case "+":
                    result = firstNumber + secondNumber;
                    break;
                case "-":
                    result = firstNumber - secondNumber;
                    break;
                case "*":
                    result = firstNumber * secondNumber;
                    break;
                case "/":
                    result = firstNumber / secondNumber;
                    break;
                default:
                    break;
            }
            //if equals =, just return the result
            if(nextOperator == "=")
            {
                return result.ToString();
            }
            //else put the next operator sign to end of the string
            else
            {
                return result.ToString() + nextOperator;
            }
            
        }

    }
}
