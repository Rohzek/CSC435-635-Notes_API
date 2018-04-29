using System;
using System.Collections.Generic;

namespace API.Scaffolding
{
    /**
     * Created using Database-First EntityFramework scaffolding.
     */
    public partial class Notes
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Categoryid { get; set; }
        public bool IsDeleted { get; set; }
        public int Userid { get; set; }

        public Category Category { get; set; }
        public User User { get; set; }
    }
}
