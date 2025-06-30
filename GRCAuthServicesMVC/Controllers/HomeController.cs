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

        //[HttpPost]
        //public async Task<IActionResult> AddInventory([FromBody] ApplicationInventoryRepository appInventory)
        //{

        //    var DomainModel = mapper.Map<ApplicationInventory>(appInventory);

        //    if (!ModelState.IsValid)
        //    {
        //        return View(DomainModel);
        //    }

        //    try
        //    {
        //        await repository.AddInventoryAsync(DomainModel);
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        //logger.LogError(ex, "Error adding inventory.");
        //        return View(mapper.Map<ApplicationInventoryRepository>(DomainModel));
        //    }
        //}



        //[HttpGet]
        ////public async Task<IActionResult> GetAll()
        ////{


        ////    var domainModel = await repository.GetAllAsync();
        ////    if (domainModel == null)
        ////    {
        ////        throw new Exception("Inventory list is null");
        ////    }

        ////    var DTO = mapper.Map<List<ApplicationInventoryRepository>>(domainModel);

        ////    return View("~/Views/Home/ApplicationInventory.cshtml", DTO);
        ////}

        //public async Task<IActionResult> GetAll([FromQuery] string? FilterOn, [FromQuery] string? filterQuery, [FromQuery] int pagenumber = 1, [FromQuery] int pagesize = 100)
        //{


        //    var domainModel = await repository.GetAllAsync(FilterOn, filterQuery, pagenumber, pagesize);
        //    if (domainModel == null)
        //    {
        //        throw new Exception("Inventory list is null");
        //    }

        //    var totalCount = await repository.GetCountAsync(FilterOn, filterQuery); // you’d implement this

        //    ViewBag.CurrentPage = pagenumber;
        //    ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pagesize);

        //    var DTO = mapper.Map<List<ApplicationInventoryRepository>>(domainModel);

        //    return View("~/Views/Home/ApplicationInventory.cshtml", DTO);
        //}





        //[HttpPost]
        //public async Task<IActionResult> AddOrUpdate([FromBody] ApplicationInventoryRepository appInventory, string action)
        //{
        //    var DomainModel = mapper.Map<ApplicationInventory>(action);

        //    if (!ModelState.IsValid)
        //    {
        //        return View("~/Views/Home/ApplicationInventory.cshtml", DomainModel);
        //    }

        //    try
        //    {
        //        await repository.AddOrUpdateAsync(DomainModel, action);
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, "Failed to add or update inventory.");
        //        return BadRequest("An error occurred: " + ex.Message);
        //    }
        //}



        //[HttpGet]
        //public async Task<IActionResult> GetAllRoles(string? FilterOn = null, string? filterQuery = null)
        //{
        //    var domainModel = await roleRepo.GetAllRoles(FilterOn, filterQuery);

        //    if (domainModel == null)
        //    {
        //        throw new Exception("Roles list is null");
        //    }

        //    var viewModel = new ApplicationRoleViewModel
        //    {
        //        FormData = new ApplicationRoleRepository(),
        //        Roles = mapper.Map<List<ApplicationRoleRepository>>(domainModel)
        //    };

        //    //var dto = mapper.Map<List<ApplicationRoleRepository>>(domainModel);

        //    return View("~/Views/Home/ApplicationRole.cshtml", viewModel);
        //}


        //[HttpPost]
        //public async Task<IActionResult> AddRoleAsync([FromBody] ApplicationRoleRepository appRole)
        //{
        //    var DTO = mapper.Map<ApplicationRole>(appRole);

        //    if (ModelState.IsValid)
        //    {
        //        await roleRepo.AddRoleAsync(DTO);
        //        return RedirectToAction("Index");
        //    }

        //    return View(DTO);
        //}


        //[HttpPost]
        //public async Task<IActionResult> AddOrUpdateRoles([FromBody] ApplicationRoleRepository appRole, string action)
        //{
        //    var DomainModel = mapper.Map<ApplicationRole>(appRole);

        //    if (!ModelState.IsValid)
        //    {
        //        return View(appRole);
        //    }

        //    try
        //    {
        //        await roleRepo.AddOrUpdateRoles(DomainModel, action);
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, "Failed to add or update role.");
        //        return BadRequest("An error occurred");
        //    }
        //}


        //[HttpGet]
        //public IActionResult ApplicationRole()
        //{
        //    var model = new ApplicationRoleViewModel
        //    {
        //        FormData = new ApplicationRoleRepository(),
        //        Roles = new List<ApplicationRoleRepository>() // Empty initially
        //    };

        //    return View(model);
        //}
    }
}
