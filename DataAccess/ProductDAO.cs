using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProductDAO
    {
        public static List<Product> GetProducts()
        {
            var listProducts = new List<Product>();
            try
            {
                using (var context = new MyDbContext())
                {
                    listProducts = context.Products.Include(p => p.Category).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listProducts;
        }

        public static Product FindProductById(int productId)
        {
            Product p = new Product();
            try
            {
                using (var context = new MyDbContext())
                {
                    p = context.Products.Include(p => p.Category).SingleOrDefault(x => x.ProductId == productId);
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return p;
        }

        public static void SaveProduct(Product p)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Products.Add(p);
                    context.SaveChanges();
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void UpdateProduct(Product p)
        {
            try
            {
                using(var context = new MyDbContext())
                {
                    context.Products.Update(p);
                    context.SaveChanges();
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void DeleteProduct(Product p)
        {
            try
            {
                using( var context = new MyDbContext())
                {
                    context.Products.Remove(p);
                    context.SaveChanges();
                }
            }catch( Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
