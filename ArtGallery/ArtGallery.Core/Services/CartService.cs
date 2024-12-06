using ArtGallery.Core.Abstraction;
using ArtGallery.Data;
using ArtGallery.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Core.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public CartService(ApplicationDbContext context, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }
        public async Task<int> AddItem(int productId, int quantity)
        {
            string userId = GetUserId();
            using var transaction = _context.Database.BeginTransaction();
            try
            {

                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("user is not logged in");
                }

                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _context.ShoppingCarts.Add(cart);
                }
                _context.SaveChanges();
                //cart detail section
                var cartItem = _context.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.ProductId == productId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += quantity;
                }
                else
                {
                    var product = _context.Products.Find(productId);
                    cartItem = new CartDetail
                    {
                        ProductId = productId,
                        ShoppingCartId = cart.Id,
                        Quantity = quantity,
                        UnitPrice = product.Price
                    };
                    _context.CartDetails.Add(cartItem);
                }
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemsCount(userId);
            return cartItemCount;
        }

        public async Task<int> RemoveItem(int productId)
        {
            string userId = GetUserId();

            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("user is not logged in");
                }

                var cart = await GetCart(userId);
                if (cart is null)
                {
                    throw new Exception("Invalid cart");
                }

                //cart detail section
                var cartItem = _context.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.ProductId == productId);
                if (cartItem is null)
                {
                    throw new Exception("no items in the cart");
                }
                else if (cartItem.Quantity == 1)
                {
                    _context.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = cartItem.Quantity - 1;
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            { }
            var cartItemCount = await GetCartItemsCount(userId);
            return cartItemCount;
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return null;

            var shoppingCart = await _context.ShoppingCarts
                .Include(a => a.CartDetails)
                .ThenInclude(a => a.Product)
                .ThenInclude(a => a.Artist)
                .FirstOrDefaultAsync(a => a.UserId == userId);

            return shoppingCart ?? new ShoppingCart { CartDetails = new List<CartDetail>() };
        }

        public async Task<ShoppingCart> GetCart(string user)
        {
            //var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            //return cart;

            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid user id");

            var shoppingCart = await _context.ShoppingCarts
                .Include(a => a.CartDetails)
                .ThenInclude(a => a.Product)
                .ThenInclude(a => a.Artist)
                .Where(a => a.UserId == userId)
                .FirstOrDefaultAsync();

            return shoppingCart;


        }

        public async Task<int> GetCartItemsCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }

            var itemCount = await _context.CartDetails
                .Where(cd => _context.ShoppingCarts
                    .Any(cart => cart.Id == cd.ShoppingCartId && cart.UserId == userId))
                .CountAsync();

            return itemCount;


            //LINQ
            //if (string.IsNullOrEmpty(userId))
            //{
            //    userId = GetUserId();
            //}

            //var itemCount = await _context.CartDetails
            //    .Where(cd => _context.ShoppingCarts
            //        .Where(cart => cart.UserId == userId)
            //        .Select(cart => cart.Id)
            //        .Contains(cd.ShoppingCartId))
            //    .CountAsync();

            //return itemCount;
        }

        public async Task<bool> DoCheckout()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId)) throw new Exception("user is not logged in.");
                var cart = await GetCart(userId);
                if (cart is null) throw new Exception("invalid cart.");
                
                var cartDetail = _context.CartDetails
                    .Where(x => x.ShoppingCartId == cart.Id).ToList();
                
                if (cartDetail.Count == 0) throw new Exception("cart is empty");
                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    OrderStatusId = 1
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice=item.UnitPrice,
                    };
                    _context.OrderDetails.Add(orderDetail);
                }
                _context.SaveChanges();

                //removing cartDetails
                _context.CartDetails.RemoveRange(cartDetail);
                _context.SaveChanges();
                transaction.Commit();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        private string GetUserId()
        {
            var user = _contextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(user);
            return userId;
        }
    }
}
