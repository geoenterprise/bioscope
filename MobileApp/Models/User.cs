

namespace MobileApp.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public ICollection<Discovery> Discoveries { get; set; } = new List<Discovery>();
    }
}
