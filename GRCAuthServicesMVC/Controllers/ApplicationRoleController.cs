using AutoMapper;
using GRCAuthServicesMVC.Models;
using GRCAuthServicesMVC.Models.DTO;
using GRCAuthServicesMVC.ViewModels;
using GRCAuthServicesMVC.Views.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


        //[HttpPost]
        //public async Task<IActionResult> AddOrUpdateRoles( ApplicationRoleRepository appRole, string action)
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

        [HttpPost]
        //public async Task<IActionResult> AddOrUpdateRoles(ApplicationRoleRepository appRole, string action)
        //{
        //    var domainModel = mapper.Map<ApplicationRole>(appRole);

        //    if (!ModelState.IsValid)
        //    {
        //        // Prepare the view model expected by the form view
        //        var roles = await roleRepo.GetAllAsync(null, null, 1, 10);
        //        var rolesDto = mapper.Map<List<ApplicationRoleRepository>>(roles);

        //        var viewModel = new ApplicationRoleViewModel
        //        {
        //            FormData = new ApplicationRoleRepository(),
        //            Roles = rolesDto,
        //            CurrentPage = 1,
        //            TotalPages = 1
        //        };


        //        // Return the form view explicitly with the full view model
        //        return View("~/Views/Home/ApplicationRole.cshtml", viewModel);
        //    }

        //    try
        //    {
        //        await roleRepo.AddOrUpdateRoles(domainModel, action);
        //        return RedirectToAction("GetAllRoles"); // Redirect to your list page or index
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, "Failed to add or update role.");
        //        return BadRequest("An error occurred");
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddOrUpdateRoles(ApplicationRoleRepository appRole, string action)
        //{
        //    logger.LogInformation($"Action: {action}, RoleId: {appRole.RoleId}");
        //    logger.LogInformation($"Form Submitted. Action: {action}, RoleId: {appRole.RoleId}");




        //    // Default updated time if missing
        //    //if (domainModel.UpdatedOn == default)
        //    //    domainModel.UpdatedOn = DateTime.UtcNow;

        //    if (!ModelState.IsValid)
        //    {
        //        var roles = await roleRepo.GetAllAsync(null, null, 1, 10);
        //        var rolesDto = mapper.Map<List<ApplicationRoleRepository>>(roles);

        //        var viewModel = new ApplicationRoleViewModel
        //        {
        //            FormData = appRole,
        //            Roles = rolesDto,
        //            CurrentPage = 1,    
        //            TotalPages = 1
        //        };

        //        return View("~/Views/Home/ApplicationRole.cshtml", viewModel);
        //    }

        //    try
        //    {
        //        var domainModel = mapper.Map<ApplicationRole>(appRole);
        //        await roleRepo.AddOrUpdateRoles(domainModel, action.ToLower());
        //        return RedirectToAction("GetAllRoles");
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, "Error adding/updating role.");
        //        return BadRequest("Update failed.");
        //    }
        //}

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






    }
}
