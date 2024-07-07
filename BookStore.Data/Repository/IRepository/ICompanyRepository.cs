using BookStore.DataAccess.Models;
using BookStore.Models;


namespace BookStore.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    { 
        //T - Category
        void  Update(Company company);
        
        
    }   
}
