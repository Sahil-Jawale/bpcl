using AutoMapper;
using GRCAuthServicesMVC.Models;
using GRCAuthServicesMVC.Models.DTO;
using GRCAuthServicesMVC.ViewModels;
using GRCAuthServicesMVC.Views.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GRCAuthServicesMVC.Controllers
{

    //[Authorize(Roles = "Admin")]
    public class ApplicationRoleController : Controller
    {
        private ILogger<ApplicationRoleController> logger;
        private ApplicationDbContext appDbContext;
        private IApplicationInventoryRepository repository;
        private IApplicationRole roleRepo;
        private IMapper mapper;

        public ApplicationRoleController(ILogger<ApplicationRoleController> logger, ApplicationDbContext appDbContext, IApplicationInventoryRepository repository, IApplicationRole roleRepo, IMapper mapper
           )
        {
            this.logger = logger;
            this.appDbContext = appDbContext;
            this.repository = repository;
            this.roleRepo = roleRepo;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("AddRole")]

        public IActionResult AddApplicationRole()
        {
            var allAppIds = appDbContext.ApplicationRoles
              .Select(r => r.ApplicationId)
              .Distinct()
              .ToList() ?? new List<string>();

            var viewModel = new ApplicationRoleViewModel
            {
                ApplicationIds = allAppIds
            };

            return View("~/Views/Home/AddApplicationRole.cshtml", viewModel);
        }
        //    return View("~/Views/Home/AddApplicationRole.cshtml"); // or wherever your add form view is
        //}

        public IActionResult AddApplicationInventory()
        {
            return View();
        }
        //public IActionResult AddApplicationInvetory()
        //{

        //    return View();
        //}


        [HttpGet]
        public async Task<IActionResult> GetAllRoles(
    string? FilterOn = null,
    string? filterQuery = null,
    int pagenumber = 1,
    int pagesize = 10)
        {
            // Get only the paginated data
            var roles = await roleRepo.GetAllAsync(FilterOn, filterQuery, pagenumber, pagesize);
            var totalCount = await roleRepo.GetCountAsync(FilterOn, filterQuery);
            int totalPages = (int)Math.Ceiling((double)totalCount / pagesize);

            var selectedRole = roles.FirstOrDefault();

            var viewModel = new ApplicationRoleViewModel
            {
                FormData = selectedRole != null ? mapper.Map<ApplicationRoleRepository>(selectedRole) : new ApplicationRoleRepository(),
                Roles = mapper.Map<List<ApplicationRoleRepository>>(roles),
                CurrentPage = pagenumber,
                TotalPages = totalPages
            };

            return View("~/Views/Home/ApplicationRole.cshtml", viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AddRoleAsync(
            ApplicationRoleRepository appRole)
        {
            var DTO = mapper.Map<ApplicationRole>(appRole);

            if (ModelState.IsValid)
            {
                await roleRepo.AddRoleAsync(DTO);
                return RedirectToAction("Index");
            }
            return View(DTO);
        }



        [HttpPost]
        public async Task<IActionResult> AddOrUpdateRoles( ApplicationRoleViewModel model, string action)
        {
            var appRole = model.FormData;

            //if (appRole.UpdatedOn == null)
            //    appRole.UpdatedOn = DateTime.UtcNow;


            logger.LogInformation($"Submitted RoleId = {appRole.RoleId}, Action = {action}");

            

            foreach (var key in ModelState.Keys)
            {
                logger.LogWarning($"ModelState Key: {key}");
            }



            if (!ModelState.IsValid)
            {

                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        logger.LogError($"ModelState Error - Key: {entry.Key}, Error: {error.ErrorMessage}");
                    }
                }


                var roles = await roleRepo.GetAllAsync(null, null, 1, 10);
                var viewModel = new ApplicationRoleViewModel
                {
                    FormData = appRole,
                    Roles = mapper.Map<List<ApplicationRoleRepository>>(roles),
                    CurrentPage = 1,    
                    TotalPages = 1
                };
                return View("~/Views/Home/ApplicationRole.cshtml", viewModel);
            }

            var domainModel = mapper.Map<ApplicationRole>(appRole);
            await roleRepo.AddOrUpdateRoles(domainModel, action.ToLower());
            logger.LogInformation($"SQL repo called successfully.");
            return RedirectToAction("GetAllRoles");
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

        [HttpGet]
        public async Task<IActionResult> EditApplicationRole(int id)
        {
            var role = await appDbContext.ApplicationRoles.FindAsync(id);
            if (role == null)
                return NotFound();

            ViewBag.ApplicationIds = await appDbContext.ApplicationInventories
                .Select(a => a.ApplicationId)
                .Distinct()
                .ToListAsync();

            return View("~/Views/Home/EditApplicationRole.cshtml", role);
        }





    }
}
