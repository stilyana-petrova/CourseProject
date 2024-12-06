using ArtGallery.Core.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Controllers
{
    [Authorize]
    public class UserOrderController : Controller
    {
        private readonly IUserOrderService _userOrderService;
        public UserOrderController(IUserOrderService userOrderService)
        {
            _userOrderService = userOrderService;
        }
        //My Orders
        public async Task<IActionResult> UserOrders()
        {
            var orders = _userOrderService.UserOrders();
            return View(orders);
        }
    }
}
