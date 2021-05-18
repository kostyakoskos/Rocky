using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;
using Rocky.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVm = new HomeVM
            {
                Products = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType),
                Categories = _db.Category
            };
            return View(homeVm);
        }

        public IActionResult Details(int id)
        {
            List<ShoppingCard> shoppingCarsList = new List<ShoppingCard>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCard>>(WC.SessionCard) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCard>>(WC.SessionCard).Count() > 0)
            {
                shoppingCarsList = HttpContext.Session.Get<List<ShoppingCard>>(WC.SessionCard);
            }
            DetailsVM DetailsVM = new DetailsVM()
            {
                Product = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType)
                .Where(u => u.Id == id).FirstOrDefault(),
                ExistInCard = false
            };

            foreach(var item in shoppingCarsList)
            {
                if(item.ProductId == id)
                {
                    DetailsVM.ExistInCard = true;
                }
            }

            return View(DetailsVM);
        }

        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShoppingCard> shoppingCarsList = new List<ShoppingCard>();
            if(HttpContext.Session.Get<IEnumerable<ShoppingCard>>(WC.SessionCard) != null 
                && HttpContext.Session.Get<IEnumerable<ShoppingCard>>(WC.SessionCard).Count() > 0)
            {
                shoppingCarsList = HttpContext.Session.Get<List<ShoppingCard>>(WC.SessionCard);
            }

            shoppingCarsList.Add(new ShoppingCard { ProductId = id });
            HttpContext.Session.Set(WC.SessionCard, shoppingCarsList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCard(int id)
        {
            List<ShoppingCard> shoppingCarsList = new List<ShoppingCard>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCard>>(WC.SessionCard) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCard>>(WC.SessionCard).Count() > 0)
            {
                shoppingCarsList = HttpContext.Session.Get<List<ShoppingCard>>(WC.SessionCard);
            }

            var itemToRemove = shoppingCarsList.SingleOrDefault(r => r.ProductId == id);

            if(itemToRemove != null)
            {
                shoppingCarsList.Remove(itemToRemove);
            }


            HttpContext.Session.Set(WC.SessionCard, shoppingCarsList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
