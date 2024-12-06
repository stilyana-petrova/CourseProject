using ArtGallery.Core.Abstraction;
using ArtGallery.Data;
using ArtGallery.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Core.Services
{
    public class UserOrderService:IUserOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserOrderService(ApplicationDbContext context, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Order>> UserOrders()
        {
            var userId=GetUserId();
            if (string.IsNullOrEmpty(userId)) throw new Exception("user is not logged in");

            var orders = await _context.Orders
                .Include(x => x.OrderStatus)
                .Include(x=>x.OrderDetail)
                .ThenInclude(x=>x.Product)
                .ThenInclude(x=>x.Artist)

                .Where(x => x.UserId == userId)
                .ToListAsync();

            return orders;
        }

        private string GetUserId()
        {
            var user = _contextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(user);
            return userId;
        }

    }
}
