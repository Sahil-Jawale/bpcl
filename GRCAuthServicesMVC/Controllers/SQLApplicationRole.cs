using GRCAuthServicesMVC.Models;
using GRCAuthServicesMVC.Views.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GRCAuthServicesMVC.Repositories
{
    public class SQLApplicationRole : IApplicationRole
    {
        private readonly ILogger<SQLApplicationRole> logger;
        private readonly ApplicationDbContext appDbContext;

        public SQLApplicationRole(ILogger<SQLApplicationRole> logger, ApplicationDbContext appDbContext)
        {
            this.logger = logger;
            this.appDbContext = appDbContext;
        }

        //public async Task<IEnumerable<ApplicationRole>> GetAllRoles()
        //{
        //    return await appDbContext.ApplicationRoles.ToListAsync();
        //}

        public async Task<ApplicationRole> AddRoleAsync(ApplicationRole appRole)
        {
            appDbContext.ApplicationRoles.Add(appRole);
            await appDbContext.SaveChangesAsync();
            return appRole;
        }

        public async Task<ApplicationRole> AddOrUpdateRoles(ApplicationRole appRole, string action)
        {
            if (action == "add")
            {
                appRole.RoleId = 0;
                appDbContext.ApplicationRoles.Add(appRole);
            }
            else if (action == "edit")
            {
                var existing = await appDbContext.ApplicationRoles.FindAsync(appRole.RoleId);
                if (existing == null) throw new Exception("Role not found");

                existing.ApplicationId = appRole.ApplicationId;
                existing.ApplicationRole1 = appRole.ApplicationRole1;
                existing.BusinessRole = appRole.BusinessRole;
                existing.AppAuthObject = appRole.AppAuthObject;
                existing.ActiveStatus = appRole.ActiveStatus;
                existing.UpdatedBy = appRole.UpdatedBy;
                existing.UpdatedOn = appRole.UpdatedOn;
            }
            else
            {
                throw new ArgumentException("Invalid action");
            }

            await appDbContext.SaveChangesAsync();
            return appRole;
        }

        //public async Task<IEnumerable<ApplicationRole>> GetAllRoles(string? FilterOn = null, string? filterQuery = null)
        //{
        //    IQueryable<ApplicationRole> query = appDbContext.ApplicationRoles.AsQueryable();

        //    if (!string.IsNullOrWhiteSpace(FilterOn) && !string.IsNullOrWhiteSpace(filterQuery))
        //    {
        //        switch (FilterOn.Trim().ToLower())
        //        {
        //            case "applicationid":
        //                query = query.Where(x => x.ApplicationId.Contains(filterQuery));
        //                break;

        //            case "applicationrole1":  // property ApplicationRole1 maps to column ApplicationRole
        //                query = query.Where(x => x.ApplicationRole1 != null && x.ApplicationRole1.Contains(filterQuery));
        //                break;

        //            case "businessrole":
        //                query = query.Where(x => x.BusinessRole.Contains(filterQuery));
        //                break;

        //            case "appauthobject":
        //                query = query.Where(x => x.AppAuthObject != null && x.AppAuthObject.Contains(filterQuery));
        //                break;

        //            case "activestatus":
        //                query = query.Where(x => x.ActiveStatus.Contains(filterQuery));
        //                break;
        //        }
        //    }

        //    return await query.ToListAsync();

        //}

        public async Task<IEnumerable<ApplicationRole>> GetAllAsync(
    string? FilterOn = null,
    string? filterQuery = null,
    int pagenumber = 1,
    int pagesize = 10)
        {
            IQueryable<ApplicationRole> query = appDbContext.ApplicationRoles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(FilterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                switch (FilterOn.Trim().ToLower())
                {

                    case "roleid":
                        if (int.TryParse(filterQuery, out int rollid))
                        {
                            query = query.Where(x => x.RoleId == rollid);
                        }
                        break;

                    case "applicationid":
                        query = query.Where(x => x.ApplicationId.Contains(filterQuery));
                        break;

                    case "applicationrole1":  // property ApplicationRole1 maps to column ApplicationRole
                        query = query.Where(x => x.ApplicationRole1 != null && x.ApplicationRole1.Contains(filterQuery));
                        break;

                    case "businessrole":
                        query = query.Where(x => x.BusinessRole.Contains(filterQuery));
                        break;

                    //case "appauthobject":
                    //    query = query.Where(x => x.AppAuthObject != null && x.AppAuthObject.Contains(filterQuery));
                    //    break;

                    case "activestatus":
                        query = query.Where(x => x.ActiveStatus.Contains(filterQuery));
                        break;
                }
            }

            // Apply paging
            query = query
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize);

            return await query.ToListAsync();
        }


        public async Task<int> GetCountAsync(string? filterOn = null, string? filterQuery = null)
        {
            IQueryable<ApplicationRole> query = appDbContext.ApplicationRoles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                switch (filterOn.Trim().ToLower())
                {
                    case "applicationid":
                        query = query.Where(x => x.ApplicationId.Contains(filterQuery));
                        break;

                    case "applicationrole1":
                        query = query.Where(x => x.ApplicationRole1 != null && x.ApplicationRole1.Contains(filterQuery));
                        break;

                    case "businessrole":
                        query = query.Where(x => x.BusinessRole.Contains(filterQuery));
                        break;

                    case "appauthobject":
                        query = query.Where(x => x.AppAuthObject != null && x.AppAuthObject.Contains(filterQuery));
                        break;

                    case "activestatus":
                        query = query.Where(x => x.ActiveStatus.Contains(filterQuery));
                        break;                  
                }
            }

            return await query.CountAsync();
        }







    }
}
