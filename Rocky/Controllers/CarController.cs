using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System.Collections.Generic;

namespace Rocky.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CarController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Car> objList = _db.Car;
            return View(objList);
        }

        // Get for create
        public IActionResult Create()
        {
            return View();
        }

        // Post for create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Car obj)
        {
            if (ModelState.IsValid)
            {
                _db.Car.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("obj");
        }

        // Get for edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Car.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        // Post for edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Car obj)
        {
            if (ModelState.IsValid)
            {
                _db.Car.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("obj");
        }


        
        // Get for delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Car.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        // Post for delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Car.Find(id);
            if (ModelState.IsValid)
            {
                _db.Car.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("obj");
        }


    }
}
