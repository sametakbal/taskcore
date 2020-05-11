namespace taskcore.Models
{
    public class ProjectsProgress
    {
        public int Id { get; set; }
        public Project Project { get; set; }

        public int Progress { get; set; }

    }
}