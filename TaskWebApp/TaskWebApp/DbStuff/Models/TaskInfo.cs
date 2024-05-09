namespace TaskWebApp.DbStuff.Models
{
    public class TaskInfo : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public virtual User? Owner { get; set; }
    }
}
