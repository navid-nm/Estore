﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Estore.Data;
using Estore.Models;

namespace Estore.Controllers
{
    public class LocationController : Controller
    {
        private readonly EstoreDbContext _context;

        public LocationController(EstoreDbContext context)
        {
            _context = context;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult Index(string findcode)
        {
            if (new UserData(_context).UserHasLocation(User.Identity.Name)) 
            { 
                return Redirect("/");
            }
            if (!string.IsNullOrEmpty(findcode.Trim()))
            {
                HttpContext.Session.SetString("lastfc", findcode);
            }
            return View();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost]
        public IActionResult Index(Location location)
        {
            if (ModelState.IsValid)
            {
                new UserData(_context).SetLocation(User.Identity.Name, location);
                return Redirect("/buy/" + HttpContext.Session.GetString("lastfc"));
            }
            return View();
        }
    }
}
