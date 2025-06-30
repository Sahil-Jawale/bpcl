using GRCAuthServicesMVC.Models;
using GRCAuthServicesMVC.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GRCAuthServicesMVC.ViewModels
{
    public class ApplicationRoleViewModel
    {

        //[Bind(Prefix = "FormData")]
        public ApplicationRoleRepository FormData { get; set; } = new ApplicationRoleRepository();
        public List<ApplicationRoleRepository> Roles { get; set; } = new List<ApplicationRoleRepository>();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

    }
}
