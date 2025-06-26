using AutoMapper;
using GRCAuthServicesMVC.Models;
using GRCAuthServicesMVC.Models.DTO;

namespace GRCAuthServicesMVC.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ApplicationInventory,ApplicationInventoryRepository>().ReverseMap();
            CreateMap<ApplicationRole, ApplicationRoleRepository>().ReverseMap();
            



        }
    }
}
