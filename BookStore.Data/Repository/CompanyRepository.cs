using BookStore.Models;
using BookStore.DataAcess.Data;
using BookStore.DataAcess.Models;
using BookStore.DataAcess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAcess.Repository
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
