namespace TaskWebApp.DbStuff.Models
{
    public class User : BaseModel
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Login { get; set; }

        public string? PreferLocale { get; set; }
        public string? Role { get; set; }
        public virtual List<TaskInfo>? OwnedTasks { get; set; }
        public virtual List<TaskInfo>? AssignedTasks { get; set; }
    }
}
