using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Cors;
using API.Scaffolding;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

/**
 * Controller for Categories
 */
namespace API.Controllers
{
    [Route("api/[controller]"), EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoriesController : Controller
    {
        DatabaseContext db = new DatabaseContext();

        // GET api/categories
        [HttpGet]
        public List<Category> Query()
        {
            return db.Category.ToList();
        }

        // GET api/categories/1
        [HttpGet("{id}")]
        public Category Query(int id)
        {
            return db.Category.Where(c => c.Id.Equals(id)).FirstOrDefault();
        }

        /*
         * POST api/categories
         * 
         * Example Input Json?:
         * 
         * {
         *    "Category":
         *    {
         *       "Id":"2",
         *       "Name":"Example"
         *    }
         * }
         */
        [HttpPost]
        public void Add([FromBody]string json)
        {
            db.Category.Add(JsonConvert.DeserializeObject<Category>(json));

            db.SaveChanges();
        }

        /*
         * UT api/categories/1
         * 
         * Example Input Json?:
         * 
         * {
         *    "Category":
         *    {
         *       "Id":"2",
         *       "Name":"Example"
         *    }
         * }
         */
        [HttpPut("{id}")]
        public void Update(int id, [FromBody]string json)
        {
            var input = JsonConvert.DeserializeObject<Category>(json);

            // If category exists
            var category = db.Category.Where(c => c.Id.Equals(id)).FirstOrDefault();

            // Update record
            if (category != null)
            {
                category.Id = input.Id;
                category.Name = input.Name;
            }
            else // Add record
            {
                db.Category.Add(input);
            }

            db.SaveChanges();
        }

        // DELETE api/categories/1
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var category = db.Category.Where(c => c.Id.Equals(id)).FirstOrDefault();

            if (category != null)
            {
                db.Category.Remove(category);
            }

            db.SaveChanges();
        }
    }
}
