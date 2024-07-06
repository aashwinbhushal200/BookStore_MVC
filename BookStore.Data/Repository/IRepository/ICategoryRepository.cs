using BookStore.DataAccess.Models;


namespace BookStore.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository :IRepository<Category>
    { 
        //T - Category
        void  Update(Category category);
        
        
    }   
}
