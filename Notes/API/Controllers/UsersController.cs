using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Cors;
using API.Scaffolding;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

/**
 * Controller for Users
 */
namespace API.Controllers
{
    [Route("api/[controller]"), EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : Controller
    {
        DatabaseContext db = new DatabaseContext();

        // GET api/users
        [HttpGet]
        public List<User> Query()
        {
            return db.User.ToList();
        }

        // GET api/users/1
        [HttpGet("{id}")]
        public User Query(int id)
        {
            return db.User.Where(u => u.Id.Equals(id)).FirstOrDefault();
        }

        /*
         * POST api/users
         * 
         * Example Input Json?:
         * 
         * {
         *   "User": 
         *    {
         *       "Id":"2",
         *       "Email":"Example@person.edu",
         *       "Name":"Example Mann",
         *       "CreatedOn":"0001-01-01T00:00:00"
         *    }
         * }
         */
        [HttpPost]
        public void Add([FromBody]string json)
        {
            db.User.Add(JsonConvert.DeserializeObject<User>(json));

            db.SaveChanges();
        }

        /*
         * UT api/users/1
         * 
         * Example Input Json?:
         * 
         * {
         *   "User": 
         *    {
         *       "Id":"2",
         *       "Email":"Example@person.edu",
         *       "Name":"Example Mann",
         *       "CreatedOn":"0001-01-01T00:00:00"
         *    }
         * }
         */
        [HttpPut("{id}")]
        public void Update(int id, [FromBody]string json)
        {
            var input = JsonConvert.DeserializeObject<User>(json);

            // If the ID matches, and it isn't deleted
            var user = db.User.Where(u => u.Id.Equals(id)).FirstOrDefault();

            // Update record
            if (user != null)
            {
                user.Id = input.Id;
                user.Name = input.Name;
                user.Email = input.Email;
                // We can't just change the day the original record was created
                //user.CreatedOn = input.CreatedOn;
            }
            else // Add record
            {
                db.User.Add(input);
            }

            db.SaveChanges();
        }

        // DELETE api/users/1
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var user = db.User.Where(u => u.Id.Equals(id)).FirstOrDefault();

            if (user != null)
            {
                db.User.Remove(user);
            }

            db.SaveChanges();
        }
    }
}
