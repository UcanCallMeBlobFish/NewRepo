namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string ICNumber { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsPhoneVerified { get; set; }
        public string? Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool TermsAccepted { get; set; }
        public string? Pin { get; set; }
        public bool BiometricEnabled { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
