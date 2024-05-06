namespace Vendor.Application.Services.interfaces;

public interface ISmsService
{
    Task<HttpResponseMessage> SendVerificationSms(string mobileNumber, string activationCode);
}