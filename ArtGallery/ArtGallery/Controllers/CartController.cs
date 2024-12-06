using ArtGallery.Core.Abstraction;
using ArtGallery.Models.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ArtGallery.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }


        public async Task<IActionResult> AddItem(int productId, int quantity = 1, int redirect = 0)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var cartCount = await _cartService.AddItem(productId, quantity);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction(nameof(GetUserCart));
        }

        public async Task<IActionResult> RemoveItem(int productId)
        {
            var cartCount = await _cartService.RemoveItem(productId);
            return RedirectToAction(nameof(GetUserCart));
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartService.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartService.GetCartItemsCount();
            return Ok(cartItem);
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutVM model)
        {
            if (!ModelState.IsValid)
                return View(model);
            bool isCheckedOut = await _cartService.DoCheckout(model);
            if (!isCheckedOut)
                return RedirectToAction(nameof(OrderFailure));
            return RedirectToAction(nameof(OrderSuccess));
        }
        public IActionResult OrderSuccess()
        {
            return View();
        }
        public IActionResult OrderFailure()
        {
            return View();
        }


    }
}
