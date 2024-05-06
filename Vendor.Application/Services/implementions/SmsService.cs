using RestSharp;
using System.Net.Http;
using Vendor.Application.Services.interfaces;

namespace Vendor.Application.Services.implementions;

public class SmsService: ISmsService
{
    private string _apiKey = "XZh5yNh3GgxU0vRh5wpk2VxUaulK5t9h2azem4xeEhw=";
    private string _patternId = "77uyjnndc7cyhwx";
    private string _numberOfSender = "+9810004223";

    public async Task<HttpResponseMessage> SendVerificationSms(string mobileNumber, string activationCode)
    {
        HttpClient httpClient = new HttpClient();
        var url = $"http://ippanel.com:8080/?apikey={_apiKey}&pid={_patternId}&fnum={_numberOfSender}&tnum={mobileNumber}&p1=active_code&v1={activationCode}";
        return await httpClient.GetAsync(url);
    }
}