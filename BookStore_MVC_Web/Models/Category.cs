using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BookStore_MVC_Web.Models
{
    public class Category
    {
        //data annotations
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        
        public string Name { get; set; }
        //server side validation with annotations 
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1-100")]
        public int DisplayOrder { get; set; }
    }
}
