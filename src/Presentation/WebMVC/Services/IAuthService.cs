 using System.Security.Principal;
namespace WebMVC.Services
{
    public interface IAuthService<T>
    {
         T Get(IPrincipal principal);
    }
}