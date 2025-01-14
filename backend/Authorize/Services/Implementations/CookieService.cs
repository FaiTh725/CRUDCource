using Authorize.Services.Interfaces;

namespace Authorize.Services.Implementations
{
    public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public void SetCookie(string key, string value, int? expireTime = null)
        {
            var cookieOptions = new CookieOptions 
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            };

            if(expireTime is not null)
            {
                cookieOptions.Expires = DateTime.Now.AddSeconds(expireTime.Value);
            }

            var response = httpContextAccessor.HttpContext?.Response;
            if(response is not null)
            {
                response.Cookies.Append(key, value, cookieOptions);
            }
        }
    }
}
