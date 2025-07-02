using AutoMapper;
using GRCAuthServicesMVC.Models;
using GRCAuthServicesMVC.Models.DTO;
using GRCAuthServicesMVC.ViewModels;
using GRCAuthServicesMVC.Views.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GRCAuthServicesMVC.Controllers
{

    //[Authorize(Roles = "Admin")]
    [Route("ApplicationInventory")]
    public class ApplicationInventoryController : Controller
    {
        private readonly ILogger<ApplicationInventoryController> logger;
        private readonly ApplicationDbContext appDbContext;
        private readonly IApplicationInventoryRepository repository;
        private readonly IApplicationRole roleRepo;
        private readonly IMapper mapper;

        public ApplicationInventoryController(
            ILogger<ApplicationInventoryController> logger,
            ApplicationDbContext appDbContext,
            IApplicationInventoryRepository repository,
            IApplicationRole roleRepo,
            IMapper mapper)
        {
            this.logger = logger;
            this.appDbContext = appDbContext;
            this.repository = repository;
            this.roleRepo = roleRepo;
            this.mapper = mapper;
        }



        public IActionResult EditApplicationInventory()
        {   
            return View();
        }

       
       

        public IActionResult ApplicationInventory()
        {
            var entities = appDbContext.ApplicationInventories.ToList();
            var dtoList = mapper.Map<List<ApplicationInventoryRepository>>(entities);

            var viewModel = new ApplicationInventoryViewModel
            {
                ApplicationInventoryList = dtoList,
                CurrentPage = 1,
                TotalPages = 1
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("AddInventory")]
        public async Task<IActionResult> AddInventory(ApplicationInventoryRepository appInventory)
        {
            if (!ModelState.IsValid)
                return View(appInventory);

            try
            {
                var domainModel = mapper.Map<ApplicationInventory>(appInventory);
                await repository.AddInventoryAsync(domainModel);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding inventory.");
                return View(appInventory);
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index(string? FilterOn, string? filterQuery, int pagenumber = 1, int pagesize = 10)
        {
            var domainData = await repository.GetAllAsync(FilterOn, filterQuery, pagenumber, pagesize);
            if (domainData == null)
                throw new Exception("Inventory list is null");

            var totalCount = await repository.GetCountAsync(FilterOn, filterQuery);
            int totalPages = (int)Math.Ceiling((double)totalCount / pagesize);

            var dtoList = mapper.Map<List<ApplicationInventoryRepository>>(domainData);

            var viewModel = new ApplicationInventoryViewModel
            {
                ApplicationInventoryList = dtoList,
                CurrentPage = pagenumber,
                TotalPages = totalPages,
                FilterOn = FilterOn,
                FilterQuery = filterQuery
            };

            return View("~/Views/Home/ApplicationInventory.cshtml", viewModel);
        }


        [HttpGet]
        [Route("Add")]

        public IActionResult AddApplicationInventory()
        {
            return View("~/Views/Home/AddApplicationInventory.cshtml"); // or wherever your add form view is
        }


        [HttpPost]
        [Route("AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdate(ApplicationInventoryRepository appInventory, string action)
        {
            if (!ModelState.IsValid)
            {
                var fallbackViewModel = new ApplicationInventoryViewModel
                {
                    ApplicationInventoryList = new List<ApplicationInventoryRepository> { appInventory },
                    CurrentPage = 1,
                    TotalPages = 1
                };
                return View("~/Views/Home/ApplicationInventory.cshtml", fallbackViewModel);
            }

            try
            {
                var domainModel = mapper.Map<ApplicationInventory>(appInventory);
                await repository.AddOrUpdateAsync(domainModel, action);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to add or update inventory.");
                return BadRequest("An error occurred: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("EditApplicationInventory/{id}")]
        public async Task<IActionResult> EditApplicationInventory(int id)
        {
            try
            {
                var domainModel = await repository.GetByIdAsync(id); // Fetch from DB

                if (domainModel == null)
                {
                    return NotFound();
                }

                // Map domain model to repository/ViewModel object
                var viewModel = mapper.Map<ApplicationInventoryRepository>(domainModel);

                // Pass it into ApplicationInventoryViewModel which the View expects
                var wrapperViewModel = new ApplicationInventoryViewModel
                {
                    //ApplicationInventory = viewModel,
                    ApplicationInventoryList = new List<ApplicationInventoryRepository>(), // optional
                    CurrentPage = 1,
                    TotalPages = 1
                };

                return View("~/Views/Home/EditApplicationInventory.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load data for editing AppInvId: {AppInvId}", id);
                return BadRequest("Unable to load application inventory for editing.");
            }
        }


    }
}
