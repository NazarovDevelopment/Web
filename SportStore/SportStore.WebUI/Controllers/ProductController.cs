using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SportStore.Domain.Entities;
using SportStore.Domain.Abstract;

namespace SportStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        private IProductRepository repository;

        public ProductController(IProductRepository productRepository){
            this.repository = productRepository;
        }
        public ActionResult List()
        {
            return View(repository.Products);
        }
    }
}