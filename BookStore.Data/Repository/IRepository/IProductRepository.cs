using BookStore.DataAccess.Models;


namespace BookStore.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    { 
        //T - Category
        void  Update(Product product);
        
        
    }   
}
