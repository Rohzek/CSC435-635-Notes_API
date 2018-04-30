using API.Classes.Email;
using API.Scaffolding;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Cors;

/**
 * Controller for Categories
 */
namespace API.Controllers
{
    [Route("api/[controller]"), EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoriesController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        SendEmails email = new SendEmails();

        // GET api/categories
        [HttpGet]
        public List<Category> Query()
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            email.SendMessage("Somebody just called api/categories with a GET header", "The call was from: " + remoteIpAddress);

            var cats = db.Category.ToList();

            return cats;
        }

        // GET api/categories/1
        [HttpGet("{id}")]
        public Category Query(int id)
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            email.SendMessage("Somebody just called api/categories with a GET header", "The call was for note: " + id + " from: " + remoteIpAddress);

            var cat = db.Category.Where(c => c.Id.Equals(id)).FirstOrDefault();

            return cat;
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
        public void Add([FromBody]Category json)
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            email.SendMessage("Somebody just called api/categories with a POST header", "The call was:\n\n" + JsonConvert.SerializeObject(json, Formatting.Indented) + "\n\nfrom: " + remoteIpAddress);

            db.Category.Add(json);

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
        public void Update(int id, [FromBody]Category json)
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            // If category exists
            var category = db.Category.Where(c => c.Id.Equals(id)).FirstOrDefault();

            email.SendMessage("Somebody just called api/categories with a PUT header", "The call was to update \n\n" + JsonConvert.SerializeObject(category, Formatting.Indented) +
                "\n\nwith:\n\n" + JsonConvert.SerializeObject(json, Formatting.Indented) + "\n\nfrom: " + remoteIpAddress);

            // Update record
            category.Id = json.Id;
            category.Name = json.Name;

            db.SaveChanges();
        }

        // DELETE api/categories/1
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            var category = db.Category.Where(c => c.Id.Equals(id)).FirstOrDefault();

            if (category != null)
            {
                db.Category.Remove(category);
            }

            email.SendMessage("Somebody just called api/categories with a DELETE header", "The call was to delete:\n\n" + JsonConvert.SerializeObject(category, Formatting.Indented) + "\n\nfrom: " + remoteIpAddress);

            db.SaveChanges();
        }
    }
}
