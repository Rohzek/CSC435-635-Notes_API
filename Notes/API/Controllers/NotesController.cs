using API.Classes.Email;
using API.Scaffolding;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Cors;

/**
 * Controller for Notes
 */
namespace API.Controllers
{
    [Route("api/[controller]"), EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NotesController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        SendEmails email = new SendEmails();

        // GET api/notes
        [HttpGet]
        public List<Notes> Query()
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            email.SendMessage("Somebody just called api/notes with a GET header", "The call was from: " + remoteIpAddress);

            var notes = db.Notes.Include(c => c.Category).Include(u => u.User).Where(n => n.IsDeleted.Equals(false)).ToList();

            return notes;
        }

        // GET api/notes/1
        [HttpGet("{id}")]
        public Notes Query(int id)
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            email.SendMessage("Somebody just called api/notes with a GET header", "The call was for note: " + id + " from: " + remoteIpAddress);

            var note = db.Notes.Include(c => c.Category).Include(u => u.User).Where(n => n.IsDeleted.Equals(false) && n.Id == id).FirstOrDefault();
            
            return note;
        }

        /*
         * POST api/notes
         * 
         * Example Input Json?:
         * 
         * {
         *    "Id": 2,
         *    "Title": "Note:",
         *    "Note": "This is a note.",
         *    "CreatedOn": "0001-01-01T00:00:00",
         *    "Categoryid": 2,
         *    "IsDeleted": false,
         *    "Userid": 2,
         *    "Category":
         *    {
         *       "Id":"2",
         *       "Name":"Example"
         *    },
         *    "User": 
         *    {
         *       "Id":"2",
         *       "Email":"Example@person.edu",
         *       "Name":"Example Mann",
         *       "CreatedOn":"0001-01-01T00:00:00",
         *       "Notes":["2"]
         *    }
         * }
         */
        [HttpPost]
        public void Add([FromBody]Notes json)
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            email.SendMessage("Somebody just called api/notes with a POST header", "The call was:\n\n" + JsonConvert.SerializeObject(json, Formatting.Indented) + 
                                                                                                                                    "\n\nfrom: " + remoteIpAddress);

            var cat = db.Category.Where(c => c.Id.Equals(json.Categoryid)).FirstOrDefault();
            var user = db.User.Where(u => u.Id.Equals(json.Userid)).FirstOrDefault();

            if (cat != null)
            {
                json.Category = cat;
            }
            if (user != null)
            {
                json.User = user;
            }

            json.IsDeleted = false;

            db.Notes.Add(json);

            db.SaveChanges();
        }

        /*
         * PUT api/notes/1
         * 
         * Example Input Json?:
         * 
         * {
         *    "Id": 2,
         *    "Title": "Note:",
         *    "Note": "This is a note.",
         *    "CreatedOn": "0001-01-01T00:00:00",
         *    "Categoryid": 2,
         *    "IsDeleted": false,
         *    "Userid": 2,
         *    "Category":
         *    {
         *       "Id":"2",
         *       "Name":"Example"
         *    },
         *    "User": 
         *    {
         *       "Id":"2",
         *       "Email":"Example@person.edu",
         *       "Name":"Example Mann",
         *       "CreatedOn":"0001-01-01T00:00:00",
         *       "Notes":["2"]
         *    }
         * }
         */
        [HttpPut("{id}")]
        public void Update(int id, [FromBody]Notes json)
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            // If the ID matches, and it isn't deleted
            var note = db.Notes.Include(c => c.Category).Include(u => u.User).Where(n => n.IsDeleted.Equals(false) && n.Id == id).FirstOrDefault();

            email.SendMessage("Somebody just called api/notes with a PUT header", "The call was to update \n\n" + JsonConvert.SerializeObject(note, Formatting.Indented) +
                "\n\nwith:\n\n" + JsonConvert.SerializeObject(json, Formatting.Indented) + "\n\nCall came from: " + remoteIpAddress);

            var cat = db.Category.Where(c => c.Id.Equals(json.Categoryid)).FirstOrDefault();
            var user = db.User.Where(u => u.Id.Equals(json.Userid)).FirstOrDefault();

            // No need to affect the id
            //note.Id = json.Id;
            note.Title = json.Title;
            note.Note = json.Note;
            // You can't just change the creation date
            //note.CreatedOn = json.CreatedOn;
            note.Categoryid = json.Categoryid;
            note.Userid = json.Userid;
            note.Category = cat;
            note.User = user;

            db.SaveChanges();
        }

        // DELETE api/notes/1
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            var note = db.Notes.Where(n => n.Id.Equals(id)).FirstOrDefault();

            email.SendMessage("Somebody just called api/notes with a DELETE header", "The call was to delete:\n\n" + JsonConvert.SerializeObject(note, Formatting.Indented) +
                                                                                                                                  "\n\nCall came from: " + remoteIpAddress);
            /**
             * Keeping the record by switching "isDeleted" to true, caused a lot of headache.
             * We're just going to actually delete it, instead.
             */
            //note.IsDeleted = true;
            db.Notes.Remove(note);

            db.SaveChanges();
        }
    }
}
