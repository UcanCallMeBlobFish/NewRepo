using Application.DTOs;

namespace Application.Abstraction
{
    public interface IAccountService
    {
        Task<LoginResponse> LoginStep1Async(LoginStep1Request req);
        Task<BasicResponse> LoginStep2VerifyPhoneCodeAsync(LoginStep2VerifyPhoneRequest req);
        Task<BasicResponse> LoginStep3VerifyEmailCodeAsync(LoginStep3VerifyEmailRequest req);
        Task<BasicResponse> LoginStep4AcceptTermsAsync(LoginStep4TermsRequest req);
        Task<BasicResponse> LoginStep5CreatePinAsync(LoginStep5CreatePinRequest req);
        Task<BasicResponse> LoginStep6ConfirmPinAsync(LoginStep6ConfirmPinRequest req);
        Task<BasicResponse> LoginStep7EnableBiometricAsync(LoginStep7EnableBiometricRequest req);

        Task<RegisterResponse> RegisterStep1Async(RegisterStep1Request req);
        Task<BasicResponse> RegisterStep2PhoneCodeAsync(RegisterStep2PhoneRequest req);
        Task<BasicResponse> RegisterStep3EmailCodeAsync(RegisterStep3EmailRequest req);
        Task<BasicResponse> RegisterStep4AcceptTermsAsync(RegisterStep4TermsRequest req);
        Task<BasicResponse> RegisterStep5CreatePinAsync(RegisterStep5CreatePinRequest req);
        Task<BasicResponse> RegisterStep6ConfirmPinAsync(RegisterStep6ConfirmPinRequest req);
        Task<BasicResponse> RegisterStep7EnableBiometricAsync(RegisterStep7EnableBiometricRequest req);
    }
}
