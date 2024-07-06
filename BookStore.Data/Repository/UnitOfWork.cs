using BookStore.DataAccess.Repository.IRepository;
using BookStore.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        public ICategoryRepository iCategoryRepository { get; set; }
        public IProductRepository iProductRepository { get; set; }
        public ICompanyRepository iCompanyRepository { get; set; }
        public IShoppingCartRepository iShoppingCartRepository { get; set; }
        public IApplicationUserRepository iApplicationUserRepository { get; set; }
		public IOrderHeaderRepository OrderHeader { get; private set; }
		public IOrderDetailRepository OrderDetail { get; private set; }


		private ApplicationDbContext _db;

         public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            iCategoryRepository=new CategoryRepository(db);
            iProductRepository = new ProductRepository(db);
            iCompanyRepository = new CompanyRepository(db);
            iShoppingCartRepository = new ShoppingCartRepository(db);
            iApplicationUserRepository=new ApplicationUserRepository(db);
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
