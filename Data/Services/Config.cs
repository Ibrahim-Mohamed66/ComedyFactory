using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;


namespace Data
{
    public static class Config
    {
        static Config()
        {

        }

        public static PipelineEnvironment Environment { get; set; } = PipelineEnvironment.Local;
        public static string Placeholder { get; set; } = "images/front/placeholder.png";
        public static string AssetsDomain { get; set; }
        public static string Assets { get; set; }
        public static string ImageflowS3Key { get; set; } = "/images/";
        public static string VideoBaseURLImg { get; set; } = "https://img.youtube.com/vi/";
        public static string VideoBaseURL { get; set; } = "https://www.youtube.com/watch?v=";
        public static string VideoURLEmbed { get; set; } = "https://www.youtube.com/embed/";
        public static string WebSiteUrl { get; set; } = "https://admin.sauditheaters.com/";
        public static string ImageResizerAdmin { get; set; } = "?w=100&h=100&scale=both&mode=pad";
        public static string ImageResizerBox { get; set; } = "?w=600&h=600&scale=both&mode=pad";
        public static string ImageResizerBox_325 { get; set; } = "?w=325&h=325&scale=both&mode=pad&scale=both&mode=crop&bgcolor=white";
        public static string ImageResizerBanner { get; set; } = "?w=1920&scale=both&mode=pad";
        public static string PictureBaseURL { get; set; }
        public static string BaseURL { get; set; }
        public static string DeletionRequestURL { get; set; } = $"{BaseURL}/en/account/fb/deletion?code=";
        //paytabs
        public static string SuccessURL { get; set; } = "/Booking/PaymentDone/";
        public static string IPNDomain { get; set; }
        public static string PTPayUrl { get; set; }
        public static string PTCurrency { get; set; }

        public static string PayTabsProfileId { get; set; }
        public static string PayTabsServerKey { get; set; }
        public static string CallbackUrl { get; set; }

        public static string IpnKey { get; set; } = "a0faab2a-6feb-416e-88e8-48bb6855c100";
        public static string PTAddress { get; set; } = "address street";
        public static string PTState { get; set; } = "01";
        public static string PTZip { get; set; } = "12345";
        public static string PTCountry { get; set; } = "SA";
        public static string PTMobile { get; set; } = "0522222222";
        public static string PTIP { get; set; } = "1.1.1.1";

        public static string ReturnUrl { get; set; } = $"{SuccessURL}";
        public static string WebViewReturnUrl { get; set; } = $"/Booking/WebViewPaymentDone/";
        public static string WebViewReturnUrlParam { get; set; } = "&WebView=true";



        public static string AdminUserId { get; set; } = "d21e21ed-c2d7-4c1f-a903-9dcfe4f9b3db";
        public static string Domain { get; set; }
        public static string TicketMX_Domain { get; set; } = "ticketmx.com";
        public static string TicketMX_API { get; set; }
        public static string API { get; set; }
        public static string Website { get; set; }
        public static string Schema { get; set; } = "https://";
        public static string JWTWebIssuer { get; set; }
        public static string JWTWebAudience { get; set; }
        public static string JWTWebKey { get; set; }

        public static string AppleSandboxSubscriptionURL { get; set; } = "api.storekit-sandbox.itunes.apple.com";
        public static string AppleSandboxVerifyReceiptURL { get; set; } = "sandbox.itunes.apple.com";

        public static string AppleSubscriptionURL { get; set; } = "api.storekit.itunes.apple.com";
        public static string AppleVerifyReceiptURL { get; set; } = "buy.itunes.apple.com";

        public static string Write_DefaultConnection { get; set; }
        public static string Read_DefaultConnection { get; set; }

        public static int AbsoluteExpiration { get; set; }
        public static int SlidingExpiration { get; set; }
        public static int YearMin { get; set; }
        public static int MonthMin { get; set; }

        public static string AccreditmxBaseUrl { get; set; }
        public static string AccreditmxClientId { get; set; }
        public static string AccreditmxClientToken { get; set; }
        public static string AccreditmxX_Key { get; set; }


        // firebase notification
        public static string FcmSenderId { get; set; } = "63212711751";
        public static string FcmServerKey { get; set; } = "AAAADrfFb0c:APA91bFx9FWNQXqbDDihL02X9GePMNQU3Vzu79118p0Dldj5A398dUKZ4SXD2729FxqgBzarxW0m-qIgVK7DhvzFLPBXGkysBBdiV-kjqBhkMYz2siqUaNhhOZQk3zuAWIjvhoStS4GV";

        public static List<string> AllowedEndpoints { get; set; } = new List<string>
        {
            "/ar/booking/paymentdone",
            "/en/booking/paymentdone",
            "/ar/booking/paytabscallback",
            "/en/booking/paytabscallback",
            "/en/account/login",
            "/ar/account/login",
            //"/ar/booking/confirmation",
            //"/en/booking/confirmation",
            "/ar/booking/success",
            "/en/booking/success",
            "/ar/booking/failed",
            "/en/booking/failed",
            "/ar/home/updatebranch",
            "/en/home/updatebranch",
            "/ar/branches/index",
            "/en/branches/index",
            "/ar/branches",
            "/en/branches",
        };

        public static List<string> AllowedLambdaFunctions { get; set; } = new List<string>
        {
            "/api/v1/clearcart",
            "/api/v1/clearorders",
        };

        #region Google settings
        public static string Google_RevokeURL { get; set; } = "https://oauth2.googleapis.com/revoke";
        public static string Google_ClientId { get; set; } = "35517194287-6t51npjtqmnhkql34dq0jp628nrvlub8.apps.googleusercontent.com";
        public static string Google_ClientSecret { get; set; } = "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";
        public static string Google_Map_API_KEY { get; set; } = "AIzaSyBi3wkpn58eD7WGMb_24psMehqejdg6wu0";

        #endregion

        #region Apple settings
        public static string Apple_RevokeURL { get; set; } = "https://appleid.apple.com/auth/revoke";
        public static string Apple_AUD { get; set; } = "https://appleid.apple.com";
        public static string Apple_ISS { get; set; } = "S7R5BE6E92";
        public static string Apple_KID { get; set; } = "9BZN3PB95K";
        public static string Apple_ALG { get; set; } = "ES256";
        public static string Apple_TokenKey { get; set; } = "MIGTAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBHkwdwIBAQQg4NNXWYWxK3mpvZT0EmyTznKtpXOahOAmayWUQA7oXZ+gCgYIKoZIzj0DAQehRANCAASb5IvaOMTElovlKnFGfrons+fsQIHHyWjv7iI0Ks2mLiOLDWGDWPsYwS9EBdQpf9NJQKOc0Vq0iTRQVy26yVnq";
        public static string Apple_TokenURL { get; set; } = "https://appleid.apple.com/auth/token";
        public static string Apple_Authorization_Endpoint { get; set; } = "https://appleid.apple.com/auth/authorize";
        public static string Apple_JWKS_URI { get; set; } = "https://appleid.apple.com/auth/keys";
        public static string Apple_RedirectURL { get; set; } = "intent://callback?{0}#Intent;package={1};scheme=signinwithapple;end";
        public static string PackageName_IOS { get; set; } = "sa.sela.saudievents";
        public static string PackageName_Android { get; set; } = "sa.sela.saudieventsweb";

        #endregion

        #region Facebook settings

        public static string Facebook_AppId { get; set; } = "945373523038943";
        public static string Facebook_AppSecret { get; set; } = "AppSecret";
        public static string Facebook_RevokeURL { get; set; } = "https://graph.facebook.com/{0}/permissions?access_token={1}";

        #endregion

       

        public static string? GetPictureBaseUrl(bool? apiAssets = false)
        {
            return apiAssets != null && apiAssets.Value ? PictureBaseURL : ImageflowS3Key;
        }

        public static string? GetWebViewRoute(bool? webView = false)
        {
            return webView != null && webView.Value ? WebViewReturnUrlParam : "";
        }

        public static int GetBranchId(int? branchId, HttpContext httpContext)
        {
            if (branchId == null || branchId == 0)
            {
                branchId = httpContext.Session.GetInt32("branchId");
                return branchId ?? 0;
            }
            return branchId ?? 0;
        }
        public static string? GetWebViewReturn(string language, bool? webView = false)
        {
            return webView != null && webView.Value ? $"https://{Website}/{language}{WebViewReturnUrl}" : $"https://{Website}/{language}{ReturnUrl}";
        }

        public static string? GetAppleUrls(bool sandbox, string urlFor)
        {

            return urlFor switch
            {
                "Subscription" => sandbox ? AppleSandboxSubscriptionURL : AppleSubscriptionURL,
                "VerifyReceipt" => sandbox ? AppleSandboxVerifyReceiptURL : AppleVerifyReceiptURL,
                _ => null
            };
        }

        public static List<string> HiddenUsers()
        {
            return new List<string>() { "494b2239-5bd6-45e0-a2e4-b1b36ffb18ef", "74e5ca29-0135-4d98-8160-2a51394c8cec", "1b2d7150-805c-4863-885d-eb19093979e4", "fa4973c0-54a9-4322-9f01-b19ea83cd530" };
        }

        public static void AddModelStateError(ModelStateDictionary ModelState, IEnumerable<ErrorRequestViewModel> Errors)
        {
            foreach (var error in Errors)
                ModelState.AddModelError(error.Code, error.Description);
        }
        


       

       
       

       


        

        public static string? GetIpAddress(IHttpContextAccessor _httpContextAccessor)
        {
            var ipAddress = string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"]) ? "0:0:0:1" :
                _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            return ipAddress;
        }

        public static string? GetUserId(IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager)
        {
            var updatedByUserId = _httpContextAccessor?.HttpContext?.User != null ? _userManager.GetUserId(_httpContextAccessor?.HttpContext?.User) ??
                                                                                    AdminUserId : AdminUserId;
            return updatedByUserId;
        }



        
        public static MemoryCacheEntryOptions GetCacheEntryOptionMinute()
        {
            return new MemoryCacheEntryOptions
            {
                // AbsoluteExpiration = DateTime.Now.AddMinutes(AbsoluteExpirationMinute),
                // SlidingExpiration = TimeSpan.FromMinutes(SlidingExpirationMinute),
                //
                AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                SlidingExpiration = TimeSpan.FromMinutes(1),
                Size = 1024,
            };
        }
        public static MemoryCacheEntryOptions GetCacheEntryOption(int second = 30)
        {
            return new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(second),
                SlidingExpiration = TimeSpan.FromSeconds(second),
                Size = 1024,
            };
        }


    }
    public class ImageResponse
    {
        public bool IsSuccess { get; set; }
        public string? Link { get; set; }
        public string? FileName { get; set; }
        public List<string>? Message { get; set; }
    }
}
