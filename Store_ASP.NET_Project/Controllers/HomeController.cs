﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store_ASP.NET_Project.Models;

namespace Store_ASP.NET_Project.Controllers
{
    public class HomeController : Controller
    {
        private StoreDatabase db = new StoreDatabase();
        // GET: Home
        public ActionResult Index()
        {
            if(Session["loggedIn"] == null || (bool)Session["loggedIn"] == false)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpGet, Route("Login")]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost, Route("Login")]
        public ActionResult Login(User user)
        {
            User ActualUser = db.Users.SingleOrDefault(u => u.Username.Equals(user.Username, StringComparison.InvariantCultureIgnoreCase));
            if(ActualUser != null)
            {
                if(ActualUser.Password == user.Password)
                {
                    Session["loggedIn"] = true;
                    Session["userID"] = ActualUser.Id;
                    Session["userName"] = ActualUser.Username;
                    Session["userType"] = ActualUser.Type;
                    return RedirectToAction("Index", "Home", new { area = "" });         
                }
            }
            return View();
        }
        [Route("Logout")]
        public ActionResult Logout()
        {
            Session.Clear();
            TempData.Clear();
            return RedirectToAction("Login");
        }
        [HttpGet, Route("Register")]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost, Route("Register")]
        public ActionResult Register(User user)
        {
            user.Type = 0;
            if (ModelState.IsValid)
            {

                List<User> userList = db.Users.Where(i => i.Username == user.Username).ToList();

                User check = userList.Count > 0 ? userList.First() : null;

                if (check == null)
                {
                    ViewBag.AlertMessage = null;
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                } else
                {
                    ViewBag.AlertMessage = "username already exists";
                    return View();
                }
            }
            return RedirectToAction("Login");
        }
    }
}