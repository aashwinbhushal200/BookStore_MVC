using BookStore.DataAccess.Data;
using BookStore.DataAccess.Models;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;

namespace BookStore.DataAccess.Repository
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
