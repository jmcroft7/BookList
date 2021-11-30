using BookList.Models;
using BulkyBooks.Data;
using BulkyBooks.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;

namespace BulkyBooks.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        //task creation for api call
        private readonly HttpClient _Client = new HttpClient();

        public async Task<string> GetAsync(string url)
        {
            var response = await _Client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            //   var objCategoryList = _db.Categories.ToList();
            IEnumerable<Category> objCategoryList = _db.Categories;

            dynamic Mymodel = new ExpandoObject();
            Mymodel.Cat = objCategoryList;

            //actual instance of api call, logged in console
            var apiCall = GetAsync("https://www.officeapi.dev/api/quotes/random")
                .ContinueWith(task =>
                {
                    Console.WriteLine(task.IsCompleted);

                    // verify task.result and understand key/value pairs
                    //JsonTextReader reader = new JsonTextReader(new StringReader(task.Result));
                    //while (reader.Read())
                    //{
                    //    if (reader.Value != null)
                    //    {
                    //        Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
                    //
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("Token: {0}", reader.TokenType);
                    //    }
                    //}
                    // Console.WriteLine(task.Result.Length);

                    // deserialize task and utilize rootobject model methods.
                    var jsonDeserialized = JsonConvert.DeserializeObject<Rootobject>(task.Result);
                    string result = jsonDeserialized.data.character.firstname + " " + jsonDeserialized.data.character.lastname;
                    Console.WriteLine(result);
                    Mymodel.Dog = result;
                    return Task.CompletedTask;

                });

            return View(Mymodel);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {

            if (obj.Name == obj.Rating.ToString())
            {
                ModelState.AddModelError("name", "The Rating cannot exactly match the Name.");
            }
            if (obj.Rating > 10)
            {
                ModelState.AddModelError("rating", "The Rating cannot be above a 10!");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Book created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }



        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Categories.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.Rating.ToString())
            {
                ModelState.AddModelError("name", "The Rating cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Book updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Categories.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Book deleted successfully";
            return RedirectToAction("Index");

        }
    }
}
