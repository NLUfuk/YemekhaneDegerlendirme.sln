namespace Yemekhane.Entities
{
    public class User
    {
        public int Id { get; set; }
        public  string UserName { get; set; } = string.Empty;
        public  string PasswordHash { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    }

}
