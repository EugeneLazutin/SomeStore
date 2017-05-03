using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Management.Instrumentation;
using SomeStore.Infrustructure.DbEntities;
using SomeStore.Models;

namespace SomeStore.Infrustructure.Services
{
    public class ShoppingCartService
    {
        private readonly ApplicationDbContext _dbContext;

        public ShoppingCartService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddItem(string userId, int productId, int qty)
        {
            var product = _dbContext.Products.Find(productId);
            var user = GetUser(userId);

            if (product == null)
            {
                throw new InstanceNotFoundException($"Product ${productId} not found");
            }

            AddItem(product, user, qty);
        }

        public List<CartItem> GetProducts(string userId)
        {
            var cart = GetShoppingCart(userId);
            return cart.CartItems ?? new List<CartItem>();
        }

        public void RemoveItem(int id)
        {
            var cartItem = _dbContext.CartItems.Find(id);
            if (cartItem != null)
            {
                _dbContext.CartItems.Remove(cartItem);
                _dbContext.SaveChanges();
            }
        }

        public void UpdateItems(List<CartItemUpdateViewModel> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    var cartItem = _dbContext.CartItems.Find(item.Id);
                    if (cartItem != null && item.Quantity > 0 && item.Quantity != cartItem.Quantity)
                    {
                        cartItem.Quantity = item.Quantity;
                    }
                }

                _dbContext.SaveChanges();
            }
        }

        public CartItem GetItem(int id)
        {
            return _dbContext.CartItems.Find(id);
        }

        public void OrderShoppingCart(string userId)
        {
            var cart = GetShoppingCart(userId);

            if (cart.CartItems.Any())
            {
                cart.IsOrdered = true;
                cart.Created = DateTime.Now;
                _dbContext.SaveChanges();
            }
        }

        public List<ShoppingCart> GetOrders(string userId)
        {
            var user = GetUser(userId);
            if (user.ShoppingCarts != null)
            {
                return user.ShoppingCarts.Where(x => x.IsOrdered).ToList();
            }

            return new List<ShoppingCart>();
        }

        public ShoppingCart GetOrder(int id)
        {
            return _dbContext.ShoppingCarts.Find(id);
        }

        private void AddItem(Product product, ApplicationUser user, int qty)
        {
            var cart = GetShoppingCart(user);
            var cartItem = cart.CartItems.FirstOrDefault(x => x.Product.Id == product.Id);

            if (cartItem != null)
            {
                cartItem.Quantity += qty;
                _dbContext.Entry(cartItem).State = EntityState.Modified;
            }
            else
            {
                cartItem = new CartItem
                {
                    Product = product,
                    Quantity = qty
                };
                cart.CartItems.Add(cartItem);
            }

            _dbContext.SaveChanges();
        }

        private ApplicationUser GetUser(string userId)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var user = _dbContext.Users.Find(userId);
                if (user != null)
                {
                    return user;
                }
            }

            throw new InstanceNotFoundException($"User ${userId} not found");
        }

        private ShoppingCart GetShoppingCart(string userId)
        {
            var user = GetUser(userId);
            return GetShoppingCart(user);
        }

        private ShoppingCart GetShoppingCart(ApplicationUser user)
        {
            if (user.ShoppingCarts == null)
            {
                user.ShoppingCarts = new List<ShoppingCart>();
            }

            var cart = user.ShoppingCarts.FirstOrDefault(x => !x.IsOrdered);
            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    CartItems = new List<CartItem>(),
                    Created = DateTime.Now
                };
                user.ShoppingCarts.Add(cart);
                _dbContext.SaveChanges();
            }
            else if (cart.CartItems == null)
            {
                cart.CartItems = new List<CartItem>();
            }

            return cart;
        }
    }
}