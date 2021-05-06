using Microsoft.EntityFrameworkCore;

namespace DAL_EF.Entity
{
    [Index(nameof(GoogleId), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        public string GoogleId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PictureUrl { get; set; }

        public UserSettings Settings { get; set; }
    }
}
