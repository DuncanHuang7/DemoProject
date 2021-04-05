using Practive1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practive1.DAL
{
    public class ProductInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ProductContext>
    {
        protected override void Seed(ProductContext context)
        {
            var products = new List<Product>
            {
                new Product { ProductName = "SuperCat", ProductDesc = "a little cat, color is white.", UpdateDate = DateTime.Now}
            };

            products.ForEach(s => context.Products.Add(s));
            context.SaveChanges();
        }
    }
}