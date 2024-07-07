using BookStore.DataAccess.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.ViewModels
{
    public class ProductVM
    {
        public Product Products { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> categoryList { get; set; }
    }
}
