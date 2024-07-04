
using BookStore.DataAcess.Data;
using BookStore.DataAcess.Models;
using BookStore.DataAcess.Repository;
using BookStore.DataAcess.Repository.IRepository;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.DataAcess.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        /*  replace by UnitOfWork
          private readonly ICategoryRepository _categoryRepo;*/
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            ////include properties
            List<Product> objProductList = _unitOfWork.iProductRepository.GetAll(includeProperties: "Category").ToList();
            //unitOfWork implementation:
            //List<Product> products = _unitOfWork.iProductRepository.GetAll().ToList();
            /*  repo pattern implementation
              List<Category> categories = _categoryRepo.GetAll().ToList();*/
            /*   ApplicationDbContext implementation
                  List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();*/
            //using projects to get listOfCategory
            return View(objProductList);
        }
        //Convert to upsert
        /*  public IActionResult Create()
          {
              ProductVM productVM = new()
              {
                  Products = new Product(),
                 categoryList = _unitOfWork.iCategoryRepository.GetAll().Select(
                  u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() })
              };
              return View(productVM);
          }*/
        //
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Products = new Product(),
                categoryList = _unitOfWork.iCategoryRepository.GetAll().Select(
                u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() })
            };
            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Products = _unitOfWork.iProductRepository.Get(u => u.Id == id);
                return View(productVM);
            }

        }
        [HttpPost]
        //after data is filled
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                //pahthonly to wwwwroot folder
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                //image upload 
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\products");
                    string finalPath = Path.Combine(productPath, fileName);
                    //if image is not null then its update image.
                    if(!string.IsNullOrEmpty(productVM.Products.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath =
                                   Path.Combine(_webHostEnvironment.WebRootPath,
                                   productVM.Products.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(finalPath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Products.ImageUrl = @"images\products\" + fileName;
                }
                if (productVM.Products.Id == 0)
                {
                    _unitOfWork.iProductRepository.Add(productVM.Products);
                }
                else
                {
                    _unitOfWork.iProductRepository.Update(productVM.Products);
                }
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.categoryList = _unitOfWork.iCategoryRepository.GetAll().Select(
                        u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() });
                return View(productVM);

            }
        }



        //converted to Upsert
        /*public IActionResult Create(ProductVM productVM, IFormFile? file)
        {

            *//* _db.Categories.Add(obj);
             _db.SaveChanges();*/
        /* convert into Repo pattern
         _categoryRepo.Add(obj);*//*

        //unitOfWork

        if (ModelState.IsValid)
        {
            _unitOfWork.iProductRepository.Add(productVM.Products);
            _unitOfWork.Save();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }
        else
        {
            productVM.categoryList = _unitOfWork.iCategoryRepository.GetAll().Select(
                    u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() });
            return View(productVM);

        }

        //save is only of UnitOfWork not icategory

    }*/
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


        #region api call
        [HttpGet]
        public IActionResult GetAll(int id)
        {
            List<Product> products = _unitOfWork.iProductRepository.GetAll(includeProperties: "Category").ToList();

            return Json(new { Data = products });
        }
        //added after ajax call made withs sweetalert
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.iProductRepository.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }


            _unitOfWork.iProductRepository.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion

    }
}