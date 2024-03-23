using BestStoreMVC.Models;
using BestStoreMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace BestStoreMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDBContext _context;
		private readonly IWebHostEnvironment environment;

		public ProductsController(AppDBContext context, IWebHostEnvironment environment)
        {
            this._context = context;
			this.environment = environment;
		}

        public IActionResult Index()
        {
            var products = _context.Products.OrderByDescending(p => p.Id).ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
		public IActionResult Create(ProductDto productDto)
		{

            if(productDto.ImageFile == null) 
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            if(!ModelState.IsValid)
            {
                return View(productDto);
            }

            //Save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDto.ImageFile.CopyTo(stream);
            }

            //Save the new product in the db
            Product product = new Product()
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                Category = productDto.Category,
                Price = productDto.Price,
                Description = productDto.Description,
                ImageFile = newFileName,
                CreatedAt = DateTime.Now,
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index", "Products");
		}

        
		public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);

            if(product is null)
				return RedirectToAction("Index", "Products");

			var productDto = new ProductDto()
			{
				Name = product.Name,
				Brand = product.Brand,
				Category = product.Category,
				Price = product.Price,
				Description = product.Description,
			};

            ViewData["ProductId"] = product.Id;
            ViewData["ImageFile"] = product.ImageFile;
            ViewData["CreatedAt"] = product.CreatedAt;


            return View(productDto);
		}

        [HttpPost]
		public IActionResult Edit(int id, ProductDto productDto)
        {
			var product = _context.Products.Find(id);

			if (product is null)
				return RedirectToAction("Index", "Products");

            if (!ModelState.IsValid)
            {
				ViewData["ProductId"] = product.Id;
				ViewData["ImageFile"] = product.ImageFile;
				ViewData["CreatedAt"] = product.CreatedAt;
				return View(productDto);
            }

            //Update the image file if we have a new image file
            string newFileName = product.ImageFile;
            if(productDto.ImageFile != null)
            {
				newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
				newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

				string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
				using (var stream = System.IO.File.Create(imageFullPath))
				{
					productDto.ImageFile.CopyTo(stream);
				}

                //Delete the old image
                string oldImageFullPath = environment.WebRootPath + "/products/" + product.ImageFile;
                System.IO.File.Delete(oldImageFullPath);
			}

            //Update the product in the database
            product.Name = productDto.Name;
            product.Brand = productDto.Brand;
            product.Description = productDto.Description;
            product.Category = productDto.Category;
            product.ImageFile = newFileName;

            _context.SaveChanges();

			return RedirectToAction("Index", "Products");
		}

		public IActionResult Delete(int id)
        {
			var product = _context.Products.Find(id);

			if (product is null)
				return RedirectToAction("Index", "Products");

			//Delete the image
			string imageFullPath = environment.WebRootPath + "/products/" + product.ImageFile;
			System.IO.File.Delete(imageFullPath);

			_context.Products.Remove(product);
            _context.SaveChanges(true);

			return RedirectToAction("Index", "Products");

		}
	}
}
