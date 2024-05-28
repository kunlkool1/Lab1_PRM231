using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductRepository _repository = new ProductRepository();
        // GET: api/<ProductsController>
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            return _repository.GetProducts();
        }
        [HttpGet("category")]
        public ActionResult<IEnumerable<Category>> GetCategory()
        {
            return _repository.GetCategories();
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            return _repository.GetProductById(id);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public IActionResult Post(Product p)
        {
            _repository.SaveProduct(p);
            return NoContent();
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id,Product p)
        {
            var product = _repository.GetProductById(id);
            if (product == null)
                return NotFound();
            _repository.UpdateProduct(p);
            return NoContent();
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _repository.GetProductById(id);
            if(product == null)
            {
                return NotFound();
            }
            _repository.DeleteProduct(product);
            return NoContent();
        }
    }
}
