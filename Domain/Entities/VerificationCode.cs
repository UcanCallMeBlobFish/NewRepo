namespace Domain.Entities
{
    public class VerificationCode
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "PHONE" or "EMAIL"
        public bool IsUsed { get; set; }
        public DateTime? UsedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
