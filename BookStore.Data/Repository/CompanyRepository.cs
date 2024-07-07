using BookStore.Models;
using BookStore.DataAccess.Data;
using BookStore.DataAccess.Models;
using BookStore.DataAccess.Repository.IRepository;

namespace BookStore.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _db;

        //when we get this implementation , we want to pass this implementation to all base class
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;   
        }
        /*this save function is not part of model or controller, so we move it to unitOfwork.
        public void Save()
        {
            _db.SaveChanges();
        }*/
        public void Update(Company company)
        {
            _db.Companies.Update(company);
        }

    }
}
