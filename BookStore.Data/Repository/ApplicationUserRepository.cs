using BookStore.DataAcess.Data;
using BookStore.DataAcess.Models;
using BookStore.DataAcess.Repository.IRepository;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAcess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db;

        //when we get this implementation , we want to pass this implementation to all base class
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;   
        }
        /*this save function is not part of model or controller, so we move it to unitOfwork.
        public void Save()
        {
            _db.SaveChanges();
        }*/
      

    }
}
