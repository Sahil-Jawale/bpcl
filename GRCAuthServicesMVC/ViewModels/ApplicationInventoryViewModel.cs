using GRCAuthServicesMVC.Models;
using GRCAuthServicesMVC.Models.DTO;

namespace GRCAuthServicesMVC.ViewModels;

public class ApplicationInventoryViewModel
{
  
    public string? FilterOn { get; set; }
    public string? FilterQuery { get; set; }
    public List<ApplicationInventoryRepository> ApplicationInventoryList { get; set; } = new();

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }


}
