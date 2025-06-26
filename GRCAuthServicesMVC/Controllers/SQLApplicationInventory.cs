using GRCAuthServicesMVC.Models;
using GRCAuthServicesMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace GRCAuthServicesMVC.Repositories
{
    public class SqlApplicationInventoryRepository : IApplicationInventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public SqlApplicationInventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationInventory> AddInventoryAsync(ApplicationInventory appInventory)
        {
            _context.ApplicationInventories.Add(appInventory);
            await _context.SaveChangesAsync();
            return appInventory;
        }

        //public async Task<IEnumerable<ApplicationInventory>> GetAllAsync()
        //{
        //    return await _context.ApplicationInventories.ToListAsync();
        //}




        public async Task<ApplicationInventory> AddOrUpdateAsync(ApplicationInventory appInventory, string action)
        {
            if (action == "add")
            {
                _context.ApplicationInventories.Add(appInventory);
            }
            else if (action == "edit")
            {
                var existing = await _context.ApplicationInventories.FindAsync(appInventory.AppInvId);
                if (existing == null) throw new Exception("Item not found");



                existing.ApplicationDesc = appInventory.ApplicationDesc;
                existing.ApplicationId = appInventory.ApplicationId;
                existing.ProcessOwner = appInventory.ProcessOwner;
                existing.UpdatedBy = appInventory.UpdatedBy;
                existing.UpdatedOn = appInventory.UpdatedOn;
                existing.ActiveStatus = appInventory.ActiveStatus;
            }
            else
            {
                throw new ArgumentException("Invalid action type");
            }

            await _context.SaveChangesAsync();
            return appInventory;
        }

        //public async Task<IEnumerable<ApplicationInventory>> GetAllAsync(string? FilterOn = null, string? filterQuery = null, int pagenumber = 1, int pagesize = 100)
        //{
        //    pagenumber = pagenumber <= 0 ? 1 : pagenumber;
        //    pagesize = pagesize <= 0 ? 10 : pagesize;



        //    IQueryable<ApplicationInventory> query = _context.ApplicationInventories.AsQueryable();

        //    if (!string.IsNullOrWhiteSpace(FilterOn) && !string.IsNullOrWhiteSpace(filterQuery))
        //    {
        //        if (FilterOn.Equals("ApplicationID", StringComparison.OrdinalIgnoreCase))
        //        {
        //            query = query.Where(x => x.ApplicationId.Contains(filterQuery));
        //        }

        //        else if (FilterOn.Equals("AppInvId", StringComparison.OrdinalIgnoreCase) && int.TryParse(filterQuery, out var id))
        //        {

        //            query = query.Where(x => x.AppInvId == id);
        //        }


        //        else if (FilterOn.Equals("ApplicationDesc", StringComparison.OrdinalIgnoreCase))
        //        {
        //            query = query.Where(x => x.ApplicationDesc.Contains(filterQuery));
        //        }

        //       else if (FilterOn.Equals("ProcessOwner", StringComparison.OrdinalIgnoreCase))
        //        {
        //            query = query.Where(x => x.ProcessOwner.Contains(filterQuery));
        //        }


        //        else if (FilterOn.Equals("ActiveStatus", StringComparison.OrdinalIgnoreCase))
        //        {
        //            query = query.Where(x => x.ActiveStatus.Contains(filterQuery));
        //        }



        //    }



        //    query = query.Skip((pagenumber - 1) * pagesize).Take(pagesize);



        //    return await query.Take(pagesize).ToListAsync();
        //}

        public async Task<IEnumerable<ApplicationInventory>> GetAllAsync(
     string? FilterOn = null,
     string? filterQuery = null,
     int pagenumber = 1,
     int pagesize = 10)
        {
            IQueryable<ApplicationInventory> query = _context.ApplicationInventories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(FilterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                switch (FilterOn.Trim().ToLower())
                {
                    case "appinvid":
                        if (int.TryParse(filterQuery, out int appInvId))    
                        {
                            query = query.Where(x => x.AppInvId == appInvId);
                        }
                        break;

                    case "applicationid":
                        query = query.Where(x => x.ApplicationId != null && x.ApplicationId.Contains(filterQuery));
                        break;

                    case "applicationdesc":
                        query = query.Where(x => x.ApplicationDesc != null && x.ApplicationDesc.Contains(filterQuery));
                        break;

                    case "processowner":
                        query = query.Where(x => x.ProcessOwner != null && x.ProcessOwner.Contains(filterQuery));
                        break;

                    case "activestatus":
                        query = query.Where(x => x.ActiveStatus != null && x.ActiveStatus.Contains(filterQuery));
                        break;
                }
            }

            // Apply pagination
            query = query
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize);

            return await query.ToListAsync();
        }



        public async Task<int> GetCountAsync(string? filterOn = null, string? filterQuery = null)
        {
            IQueryable<ApplicationInventory> query = _context.ApplicationInventories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                switch (filterOn.Trim().ToLower())
                {
                    case "applicationid":
                        query = query.Where(x => x.ApplicationId.Contains(filterQuery));
                        break;

                    case "appinvid":
                        if (int.TryParse(filterQuery, out var id))
                            query = query.Where(x => x.AppInvId == id);
                        break;

                    case "applicationdesc":
                        query = query.Where(x => x.ApplicationDesc.Contains(filterQuery));
                        break;

                    case "processowner":
                        query = query.Where(x => x.ProcessOwner.Contains(filterQuery));
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
