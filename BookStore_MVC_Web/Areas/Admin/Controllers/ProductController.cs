
using BookStore.DataAcess.Data;
using BookStore.DataAcess.Models;
using BookStore.DataAcess.Repository;
using BookStore.DataAcess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.DataAcess.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        /*  replace by UnitOfWork
          private readonly ICategoryRepository _categoryRepo;*/
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //unitOfWork implementation:
            List<Product> products = _unitOfWork.iProductRepository.GetAll().ToList();
            /*  repo pattern implementation
              List<Category> categories = _categoryRepo.GetAll().ToList();*/
            /*   ApplicationDbContext implementation
                  List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();*/
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        //after data is filled
        public IActionResult Creat(Product obj)
        {
            //custom validation
            if (obj.Title == obj.Description.ToString())
            {
                ModelState.AddModelError("name", "The Title cannot exactly match the Description.");
            }
            /* _db.Categories.Add(obj);
             _db.SaveChanges();*/
            /* convert into Repo pattern
             _categoryRepo.Add(obj);*/

            //unitOfWork
            _unitOfWork.iProductRepository.Add(obj);
            //save is only of UnitOfWork not icategory
            _unitOfWork.Save();

            //added for toaster notification 
            TempData["success"] = "Products created successfully";
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
            Product? productFromDb = _unitOfWork.iProductRepository.Get(u => u.Id == id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();

            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.iProductRepository.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
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
            Product? productFromDb = _unitOfWork.iProductRepository.Get(u => u.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.iProductRepository.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.iProductRepository.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }


    }
}