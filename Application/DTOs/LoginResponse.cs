namespace Application.DTOs
{
    public class LoginResponse : BasicResponse
    {
        public string? VerificationCodeDemo { get; set; }
        public LoginResponse() : base(true, "") { }
    }
}
