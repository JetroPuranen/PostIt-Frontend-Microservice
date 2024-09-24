

namespace PostIt.Application.Services
{
    public interface ITokenCacheService
    {
        string GetToken();
        void SetToken(string token);
    }

    public class TokenCacheService : ITokenCacheService
    {
        private string _cachedToken;

        public string GetToken()
        {
            return _cachedToken;
        }

        public void SetToken(string token)
        {
            _cachedToken = token;
        }
    }

}
