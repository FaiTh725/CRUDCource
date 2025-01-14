namespace Authorize.Services.Interfaces
{
    public interface ICookieService
    {
        void SetCookie(string key, string value, int? expireTime = null);
    }
}
