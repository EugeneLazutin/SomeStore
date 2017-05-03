using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SomeStore.Infrustructure.Services;
using SomeStore.Models;

namespace SomeStore.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartService _cartService;

        public ShoppingCartController(ShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        public ActionResult Index()
        {
            var items = _cartService.GetProducts(User.Identity.GetUserId());
            return View(items);
        }

        public ActionResult AddToCart(int id, int qty)
        {
            _cartService.AddItem(User.Identity.GetUserId(), id, qty);

            var returnUrl = Request.UrlReferrer?.ToString();

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Details", "Products", new { id });
            }
            return Redirect(returnUrl);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = _cartService.GetItem(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item.Product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _cartService.RemoveItem(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Update(List<CartItemUpdateViewModel> items)
        {
            _cartService.UpdateItems(items);
            return RedirectToAction("Index");
        }

        public ActionResult CreateOrder()
        {
            _cartService.OrderShoppingCart(User.Identity.GetUserId());
            return RedirectToAction("Index", "Products");
        }
    }
}