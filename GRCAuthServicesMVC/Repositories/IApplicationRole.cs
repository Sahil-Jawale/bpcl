using GRCAuthServicesMVC.Models;
using GRCAuthServicesMVC.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GRCAuthServicesMVC.Repositories
{
    public interface IApplicationRole
    {
        //Task<IEnumerable<ApplicationRole>> GetAllRoles();
        Task<ApplicationRole> AddRoleAsync(ApplicationRole appRole);
        Task<ApplicationRole> AddOrUpdateRoles(ApplicationRole appRole, string action);
        Task<IEnumerable<ApplicationRole>> GetAllAsync(string? FilterOn = null, string? filterQuery = null,int pagenumber = 1,
    int pagesize = 10);
        Task<int> GetCountAsync(string? filterOn = null, string? filterQuery = null);


    }
}
