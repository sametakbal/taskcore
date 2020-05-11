namespace taskcore.Models
{
    public class UserProjects
    {
        public int Id { get; set; }
        public User User { get; set; }

        public int UserId { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }

        public bool IsAccept { get; set; } = false;
    }

}
