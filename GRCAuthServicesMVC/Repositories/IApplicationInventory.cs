using GRCAuthServicesMVC.Models;

namespace GRCAuthServicesMVC.Views.Repositories
{
    public interface IApplicationInventoryRepository
    {
        Task<ApplicationInventory> AddInventoryAsync(ApplicationInventory appInventory);
        //Task<IEnumerable<ApplicationInventory>> GetAllAsync();
        Task<ApplicationInventory> AddOrUpdateAsync(ApplicationInventory appInventory, string action);

        //Task<IEnumerable<ApplicationInventory>> GetAllAsync(string? FilterOn = null, string? filterQuery = null,  int pagenumber = 1, int pagesize = 100);
        Task<int> GetCountAsync(string? filterOn = null, string? filterQuery = null);
        Task<ApplicationInventory> GetByIdAsync(int id);
        Task<IEnumerable<ApplicationInventory>> GetAllAsync(
    string? FilterOn = null,
    string? filterQuery = null,
    int pagenumber = 1,
    int pagesize = 10);
    


    //Task EditAsync(ApplicationInventory updatedItem); 
    }
}
