using System;

namespace API.Scaffolding
{
    /**
     * Created using Database-First EntityFramework scaffolding.
     */
    public partial class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
