using Microsoft.AspNetCore.Identity;

namespace TweetBook.Services
{
    public interface ITokenGenerator
    {
        string Generate(IdentityUser user);
    }
}
