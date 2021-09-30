using onsoft.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap;


namespace MODEOUTLED.Controllers
{

    public class CacheRepositoryController : Controller
    {
        CachedProductRepository _productRepository = new CachedProductRepository();
        public ActionResult Index()
        {
            var products = _productRepository.GetProducts();
            return View(products);
        }
    }


    // Step 1
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
    }

    // Step 2
    public class EfProductRepository : IProductRepository
    {
         wwwEntities db = new wwwEntities();
       
        public virtual IEnumerable<Product> GetProducts()
        {
            return db.Products
                .ToList();
        }
    }

    // Step 3
    public class CachedProductRepository : EfProductRepository
    {
        private static readonly object CacheLockObject = new object();
        public override IEnumerable<Product> GetProducts()
        {
            string cacheKey = "Products";
            var result = HttpRuntime.Cache[cacheKey] as List<Product>;
            if (result == null)
            {
                lock (CacheLockObject)
                {
                    result = HttpRuntime.Cache[cacheKey] as List<Product>;
                    if (result == null)
                    {
                        result = base.GetProducts().ToList();
                        HttpRuntime.Cache.Insert(cacheKey, result, null,
                            DateTime.Now.AddSeconds(60), TimeSpan.Zero);
                    }
                }
            }
            return result;
        }
    }

    public static class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
                x.For<IProductRepository>().Use<EfProductRepository>();
            });
            return ObjectFactory.Container;
        }
    }

}
