using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProductManagementWebClient.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private string _productApiUrl = "";
        public ProductController()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            _productApiUrl = "http://localhost:5089/api/products";
        }
        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_productApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Product> listProducts = JsonSerializer.Deserialize<List<Product>>(strData, options);
            return View(listProducts);
        }

        // GET: ProductController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_productApiUrl+"/"+id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Product product = JsonSerializer.Deserialize<Product>(strData, options);
            HttpResponseMessage response2 = await _httpClient.GetAsync(_productApiUrl + "/category");
            string strData2 = await response2.Content.ReadAsStringAsync();
            List<Category> listCategory = JsonSerializer.Deserialize<List<Category>>(strData2, options);
            ViewData["CategoryId"] = new SelectList(listCategory, "CategoryId", "CategoryName");

            return View(product);
        }

        // GET: ProductController/Create
        public async Task<ActionResult>Create()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_productApiUrl+"/category");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Category> listCategory = JsonSerializer.Deserialize<List<Category>>(strData, options);
            ViewData["CategoryId"] = new SelectList(listCategory, "CategoryId", "CategoryName");
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product)
        {
            try
            {
                await Console.Out.WriteLineAsync(ModelState.IsValid.ToString());
                if (ModelState.IsValid)
                {
                    var productJson = JsonSerializer.Serialize(product);
                    var content = new StringContent(productJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _httpClient.PostAsync(_productApiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                return View(product);
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage productResponse = await _httpClient.GetAsync($"{_productApiUrl}/{id}");
            string productData = await productResponse.Content.ReadAsStringAsync();
            var productOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Product product = JsonSerializer.Deserialize<Product>(productData, productOptions);

            HttpResponseMessage categoryResponse = await _httpClient.GetAsync(_productApiUrl + "/category");
            string categoryData = await categoryResponse.Content.ReadAsStringAsync();
            var categoryOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Category> listCategory = JsonSerializer.Deserialize<List<Category>>(categoryData, categoryOptions);

            ViewData["CategoryId"] = new SelectList(listCategory, "CategoryId", "CategoryName", product.CategoryId);

            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var productJson = JsonSerializer.Serialize(product);
                    var content = new StringContent(productJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _httpClient.PutAsync($"{_productApiUrl}/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                return View(product);
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage productResponse = await _httpClient.GetAsync($"{_productApiUrl}/{id}");
            string productData = await productResponse.Content.ReadAsStringAsync();
            var productOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Product product = JsonSerializer.Deserialize<Product>(productData, productOptions);

            HttpResponseMessage categoryResponse = await _httpClient.GetAsync(_productApiUrl + "/category");
            string categoryData = await categoryResponse.Content.ReadAsStringAsync();
            var categoryOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Category> listCategory = JsonSerializer.Deserialize<List<Category>>(categoryData, categoryOptions);

            ViewData["CategoryId"] = new SelectList(listCategory, "CategoryId", "CategoryName", product.CategoryId);

            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            HttpResponseMessage productResponse = await _httpClient.DeleteAsync($"{_productApiUrl}/{id}");

            return RedirectToAction(nameof(Index));

        }
    }
}
