using API.Classes.Email;
using API.Scaffolding;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Cors;

/**
 * Controller for Users
 */
namespace API.Controllers
{
    [Route("api/[controller]"), EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        SendEmails email = new SendEmails();

        // GET api/users
        [HttpGet]
        public List<User> Query()
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            email.SendMessage("Somebody just called api/users with a GET header", "The call was from: " + remoteIpAddress);

            var users = db.User.ToList();

            return users;
        }

        // GET api/users/1
        [HttpGet("{id}")]
        public User Query(int id)
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            email.SendMessage("Somebody just called api/users with a GET header", "The call was for user: " + id + " from: " + remoteIpAddress);

            var user = db.User.Where(u => u.Id.Equals(id)).FirstOrDefault();

            return user;
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
        public void Add([FromBody]User json)
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            email.SendMessage("Somebody just called api/users with a POST header", "The call was:\n\n" + JsonConvert.SerializeObject(json, Formatting.Indented) + "\n\nfrom: " + remoteIpAddress);

            db.User.Add(json);

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
        public void Update(int id, [FromBody]User json)
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            // If the ID matches, and it isn't deleted
            var user = db.User.Where(u => u.Id.Equals(id)).FirstOrDefault();

            email.SendMessage("Somebody just called api/users with a PUT header", "The call was to update \n\n" + JsonConvert.SerializeObject(user, Formatting.Indented) +
                "\n\nwith:\n\n" + JsonConvert.SerializeObject(json, Formatting.Indented) + "\n\nfrom: " + remoteIpAddress);

            // Update record
            user.Id = json.Id;
            user.Name = json.Name;
            user.Email = json.Email;
            // We can't just change the day the original record was created
            //user.CreatedOn = input.CreatedOn;

            db.SaveChanges();
        }

        // DELETE api/users/1
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            var user = db.User.Where(u => u.Id.Equals(id)).FirstOrDefault();

            if (user != null)
            {
                db.User.Remove(user);
            }

            email.SendMessage("Somebody just called api/users with a DELETE header", "The call was to delete:\n\n" + JsonConvert.SerializeObject(user, Formatting.Indented) + "\n\nfrom: " + remoteIpAddress);

            db.SaveChanges();
        }
    }
}
