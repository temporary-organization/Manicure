﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Manicure.BusinessLogic.Authentication;
using Manicure.BusinessLogic.Services.Abstract;
using Manicure.Common.Auth;
using Manicure.Common.Domain;
using Manicure.Web.Models;

namespace Manicure.Web.Controllers
{
    [RoutePrefix("user")]
    public class UserController : Controller
    {
        private readonly IAuthProvider _authProvider;
        private readonly IUserService _userService;
        private readonly IClientService _clientService;
        private readonly IMasterService _masterService;

        public UserController(
            IAuthProvider authProvider,
            IUserService userService,
            IClientService clientService,
            IMasterService masterService)
        {
            _authProvider = authProvider;
            _userService = userService;
            _clientService = clientService;
            _masterService = masterService;
        }

        [HttpGet]
        [Route("login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Login(Login login)
        {
            _authProvider.Authenticate(login);

            return RedirectToAction("Main", "Home");
        }

        [HttpGet]
        [Route("register")]
        public ActionResult RegisterClient()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public ActionResult RegisterClient(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var userToAdd = Mapper.Map<UserViewModel, User>(user);

            _userService.Create(userToAdd);

            var clientToAdd = Mapper.Map<UserViewModel, Client>(user);

            _clientService.Add(clientToAdd, user.Login);

            return RedirectToAction("Main", "Home");
        }

        [HttpGet]
        [Route("master/register")]
        [Authorize(Roles = "Administrator")]
        public ActionResult RegisterMaster()
        {
            return View();
        }

        [HttpPost]
        [Route("master/register")]
        [Authorize(Roles = "Administrator")]
        public ActionResult RegisterMaster(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var userToAdd = Mapper.Map<UserViewModel, User>(user);

            _userService.Create(userToAdd);

            var masterToAdd = Mapper.Map<UserViewModel, Master>(user);

            using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
            {
                masterToAdd.Photo = binaryReader.ReadBytes(Request.Files[0].ContentLength);
            }

            _masterService.Add(masterToAdd, user.Login);

            return RedirectToAction("Main", "Home");
        }

        [HttpGet]
        [Route("logout")]
        public ActionResult Logout()
        {
            _authProvider.Deauthenticate();

            return RedirectToAction("Main", "Home");
        }

        [HttpGet]
        [Route("profile")]
        [Authorize]
        public ActionResult UserProfile()
        {
            var user = _userService.GetCurrent(User.Identity.Name);


            var userToShow = Mapper.Map<User, UserProfileViewModel>(user);

            if (string.Equals(user.Role, "Master", StringComparison.CurrentCultureIgnoreCase))
            {
                var procedureEntries = new List<ProcedureEntry>();
                procedureEntries.AddRange(
                    user.Master.Schedules.SelectMany(a => a.ProcedureEntries));

                userToShow.Diplomas = user.Master.Diplomas;
                userToShow.Description = user.Master.Description;
                userToShow.Photo = user.Master.Photo;
                userToShow.ExampleWorks = user.Master.ExampleWorks;
                userToShow.ProcedureEntries = procedureEntries;
            }
            else
            {
                if (string.Equals(user.Role, "Client", StringComparison.CurrentCultureIgnoreCase))
                {
                    userToShow.CourseEntries = user.Client.CourseEntries;
                    userToShow.ProcedureEntries = user.Client.ProcedureEntries;
                }
            }

            return View(userToShow);
        }
    }
}