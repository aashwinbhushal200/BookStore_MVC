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
        private ApplicationDbContext _db;

         public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            iCategoryRepository=new CategoryRepository(db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
