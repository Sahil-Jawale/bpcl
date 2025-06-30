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
        public async Task<IActionResult> AddInventory(ApplicationInventoryRepository appInventory)
        {
            if (!ModelState.IsValid)
                return View(appInventory);

            try
            {
                var domainModel = mapper.Map<ApplicationInventory>(appInventory);
                await repository.AddInventoryAsync(domainModel);
                return RedirectToAction("GetAll");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding inventory.");
                return View(appInventory);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? FilterOn, string? filterQuery, int pagenumber = 1, int pagesize = 10)
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

        [HttpPost]
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
                return RedirectToAction("GetAll");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to add or update inventory.");
                return BadRequest("An error occurred: " + ex.Message);
            }
        }
    }
}
