using System.Collections.Generic;
using System.Linq;
using API.Scaffolding;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        DatabaseContext db = new DatabaseContext();

        // GET api/notes
        [HttpGet]
        public List<Notes> Query()
        {
            return db.Notes.Where(n => n.IsDeleted.Equals(false)).ToList();
        }

        // GET api/notes/1
        [HttpGet("{id}")]
        public string Query(int id)
        {

            string result = "Error: No note found";

            // If the ID matches, and it isn't deleted
            var notes = db.Notes.Where(n => n.Id.Equals(id) && n.IsDeleted.Equals(false));

            foreach (var note in notes)
            {
                result = $"{note.Title}:\n{note.Note}";
            }

            return result;
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
        public void Add([FromBody]string json)
        {
            var input = JsonConvert.DeserializeObject<Notes>(json);

            db.Notes.Add(input);
        }

        /*
         * UT api/notes/1
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
        public void Update(int id, [FromBody]string json)
        {
            var input = JsonConvert.DeserializeObject<Notes>(json);

            // If the ID matches, and it isn't deleted
            var notes = db.Notes.Where(n => n.Id.Equals(id) && n.IsDeleted.Equals(false));

            foreach (var note in notes)
            {
                note.Id = input.Id;
                note.Title = input.Title;
                note.Note = input.Note;
                note.CreatedOn = input.CreatedOn;
                note.Categoryid = input.Categoryid;
                note.Userid = input.Userid;
                note.Category = input.Category;
                note.User = input.User;
            }
        }

        // DELETE api/notes/1
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var notes = db.Notes.Where(n => n.Id.Equals(id));

            foreach (var note in notes)
            {
                // I'm guessing we're supposed to keep the record?
                note.IsDeleted = true;
            }
        }
    }
}
