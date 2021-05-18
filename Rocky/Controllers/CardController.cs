using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;
using Rocky.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    [Authorize]
    public class CardController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }

        public CardController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<ShoppingCard> shoppingCardList = new List<ShoppingCard>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCard>>(WC.SessionCard) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCard>>(WC.SessionCard).Count() > 0)
            {
                shoppingCardList = HttpContext.Session.Get<IEnumerable<ShoppingCard>>(WC.SessionCard).ToList();
            }

            List<int> proInCard = shoppingCardList.Select(i => i.ProductId).ToList();

            IEnumerable<Product> prodList = _db.Product.Where(u => proInCard.Contains(u.Id));

            return View(prodList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //var userId = User.FindFirstValue(ClaimTypes.Name);

            List<ShoppingCard> shoppingCardList = new List<ShoppingCard>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCard>>(WC.SessionCard) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCard>>(WC.SessionCard).Count() > 0)
            {
                shoppingCardList = HttpContext.Session.Get<IEnumerable<ShoppingCard>>(WC.SessionCard).ToList();
            }

            List<int> proInCard = shoppingCardList.Select(i => i.ProductId).ToList();

            IEnumerable<Product> prodList = _db.Product.Where(u => proInCard.Contains(u.Id));

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = _db.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value),
                ProductList = prodList
            };

            return View(ProductUserVM);
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingCard> shoppingCardList = new List<ShoppingCard>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCard>>(WC.SessionCard) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCard>>(WC.SessionCard).Count() > 0)
            {
                shoppingCardList = HttpContext.Session.Get<IEnumerable<ShoppingCard>>(WC.SessionCard).ToList();
            }

            shoppingCardList.Remove(shoppingCardList.FirstOrDefault(u => u.ProductId == id));
            HttpContext.Session.Set(WC.SessionCard, shoppingCardList);
            return RedirectToAction(nameof(Index));
        }
    }
}
