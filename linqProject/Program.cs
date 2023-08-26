using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace linqProject
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<Category> categories = new List<Category>()
            {
                new Category{CategoryId=1, CategoryName= "Bilgisayar"},
                new Category{CategoryId=2, CategoryName= "Telefon"}
            };

            List<Product> products = new List<Product>()
            {
                new Product{ProductId=1, CategoryId =1, ProductName="Acer", QuantityPerUnit="32 gb ram", UnitPerPrice= 10000, UnitePerStock=5},
                new Product{ProductId=2, CategoryId =1, ProductName="Asus", QuantityPerUnit="16 gb ram", UnitPerPrice= 10000, UnitePerStock=3},
                new Product{ProductId=3, CategoryId =1, ProductName="Dell", QuantityPerUnit="8 gb ram", UnitPerPrice= 6000, UnitePerStock=2},
                new Product{ProductId=4, CategoryId =2, ProductName="Apple", QuantityPerUnit="4 gb ram", UnitPerPrice= 5000, UnitePerStock=3},
                new Product{ProductId=5, CategoryId =2, ProductName="Samsung", QuantityPerUnit="4 gb ram", UnitPerPrice= 8000, UnitePerStock=1},

            };

            foreach(var product in products)
            {
                if(product.UnitPerPrice >5000)
                    Console.WriteLine(product.ProductName);
            }
        }
    }


    public class Product
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal UnitPerPrice { get; set; }
        public int UnitePerStock { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
