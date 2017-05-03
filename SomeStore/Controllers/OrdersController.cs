using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SomeStore.Infrustructure.DbEntities;
using SomeStore.Infrustructure.Services;

namespace SomeStore.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ShoppingCartService _cartService;

        public OrdersController(ShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        public ActionResult Index()
        {
            var orderList = _cartService.GetOrders(User.Identity.GetUserId());
            return View(orderList);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = _cartService.GetOrder(id.Value);
            if (shoppingCart == null || !shoppingCart.IsOrdered)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }
    }
}
