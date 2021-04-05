using System;
using System.Collections.Generic;

namespace Practive1.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public DateTime UpdateDate { get; set; }

        //public virtual ICollection<ProductSpec> ProductSpec { get; set; }
    }
}