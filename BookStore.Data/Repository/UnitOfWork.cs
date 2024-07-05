using BookStore.DataAcess.Data;
using BookStore.DataAcess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAcess.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        public ICategoryRepository iCategoryRepository { get; set; }
        public IProductRepository iProductRepository { get; set; }
        public ICompanyRepository iCompanyRepository { get; set; }
        private ApplicationDbContext _db;

         public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            iCategoryRepository=new CategoryRepository(db);
            iProductRepository = new ProductRepository(db);
            iCompanyRepository = new CompanyRepository(db);
        }
        /*this save function is not part of model or controller, so we move it to unitOfwork.
      public void Save()
      {
          _db.SaveChanges();
      }*/
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
