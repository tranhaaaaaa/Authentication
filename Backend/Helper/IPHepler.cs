namespace AuthenticationModule.Helper
{
    public class IPHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IPHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetIpAddress()
        {
            return _httpContextAccessor.HttpContext?
                .Connection
                .RemoteIpAddress?
                .ToString();
        }

        public string? GetUserAgent()
        {
            return _httpContextAccessor.HttpContext?
                .Request
                .Headers["User-Agent"]
                .ToString();
        }
    }
}
