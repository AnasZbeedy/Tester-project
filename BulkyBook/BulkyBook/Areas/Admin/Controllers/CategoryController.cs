using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using BulkyBook.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unit;
        public CategoryController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _unit.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder connet exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _unit.Category.Add(obj);
                _unit.Save();
                TempData["Success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            return View();

        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) { return View(); }
            Category? categoryFromDb = _unit.Category.Get(u => u.Id == id);
            if (categoryFromDb == null) { return NotFound(); }
            return View(categoryFromDb);

        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder connet exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _unit.Category.Update(obj);
                _unit.Save();
                TempData["Success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) { return View(); }
            Category? categoryFromDb = _unit.Category.Get(u => u.Id == id);
            if (categoryFromDb == null) { return NotFound(); }
            return View(categoryFromDb);

        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _unit.Category.Get(u => u.Id == id);
            if (obj == null) { return NotFound(); }
            _unit.Category.Remove(obj);
            _unit.Save();
            TempData["Success"] = "Category Removed Successfully";
            return RedirectToAction("Index");

        }
    }
}
