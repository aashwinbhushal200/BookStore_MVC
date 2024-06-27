
using BookStore.Data;
using BookStore.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
                _db=db;
        }
        public IActionResult Index()
        {
            List<Category> categories = _db.Categories.ToList();
            //  List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        //after data is filled
        public IActionResult Create(Category obj)
        {
            //custom validation
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }
            _db.Categories.Add(obj);
            _db.SaveChanges();
            //added for toaster notification 
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
            
            /* if (ModelState.IsValid)
             {
                 _unitOfWork.Category.Add(obj);
                 _unitOfWork.Save();
                 TempData["success"] = "Category created successfully";
                 return RedirectToAction("Index");
             }*/
          

        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _db?.Categories?.FirstOrDefault(u => u.Id == id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _db?.Categories?.FirstOrDefault(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _db?.Categories?.FirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _db?.Categories?.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }


    }
}