using Practive1.DAL;
using Practive1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Practive1.Controllers
{
    public class ProductController : Controller
    {
        private ProductContext db = new ProductContext();

        // GET: Product
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductName, ProductDesc")]Product product)
        {
            //Factory mode example
            String loggerType = "database";
            ILogger logger;
            switch (loggerType)
            {
                case "database":
                    logger = new DatabaseLogger();
                    break;

                default:
                    logger = new TextLogger();
                    break;
            }

            try
            {
                if (ModelState.IsValid)
                {
                    product.UpdateDate = DateTime.Now;
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException ex/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                logger.Log(ex.ToString());
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(product);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productToUpdate = db.Products.Find(id);
            productToUpdate.UpdateDate = DateTime.Now;
            if (TryUpdateModel(productToUpdate, "",
               new string[] { "ProductName", "ProductDesc" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(productToUpdate);
        }

        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
            }
            catch (RetryLimitExceededException)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Factory example
        interface ILogger
        {
            void Log(String message);
        }

        class LogManager
        {
            private ILogger _logger;
            public LogManager(ILogger logger)
            {
                _logger = logger;
            }

            public void Log(String message)
            {
                _logger.Log(message);
            }
        }

        class TextLogger : ILogger
        {
            public void Log(String message)
            {
                Console.WriteLine("Logged to text file: " + message);
            }
        }

        class DatabaseLogger : ILogger
        {
            public void Log(String message)
            {
                Console.WriteLine("Logged to Sql database: " + message);
            }
        }
    }
}