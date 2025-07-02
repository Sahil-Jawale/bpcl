using System;
using System.Diagnostics;
using GRCAuthServicesMVC.Models;
using GRCAuthServicesMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using AutoMapper;
using GRCAuthServicesMVC.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using GRCAuthServicesMVC.Views.Repositories;

namespace GRCAuthServicesMVC.Controllers
{

    //[Authorize]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ApplicationDbContext appDbContext;
        private readonly IApplicationInventoryRepository repository;
        private readonly IApplicationRole roleRepo;
        private readonly IMapper mapper;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext appDbContext, IApplicationInventoryRepository repository, IApplicationRole roleRepo,IMapper mapper
            )
        {
            this.logger = logger;
            this.appDbContext = appDbContext;
            this.repository = repository;
            this.roleRepo = roleRepo;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ApplicationInventory()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //public IActionResult ApplicationRoles()
        //{
        //    return View();
        //}

        public IActionResult AddApplicationInvetory()
        {
            return View();
        }

        public IActionResult ViewApplicationInventory()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ApplicationRole()
        {
            var model = new ApplicationRoleViewModel
            {
                FormData = new ApplicationRoleRepository(),
                Roles = new List<ApplicationRoleRepository>() // Empty initially
            };

            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        
    }
}
