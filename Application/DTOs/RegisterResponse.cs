namespace Application.DTOs
{
    public class RegisterResponse : BasicResponse
    {
        public bool UserAlreadyExists { get; set; }
        public string? VerificationCodePhoneDemo { get; set; }
        public RegisterResponse() : base(true, "") { }
    }
}
