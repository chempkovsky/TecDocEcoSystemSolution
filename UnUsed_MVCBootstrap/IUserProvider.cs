// MVCBootstrap.IUserProvider
using MVCBootstrap;

namespace MVCBootstrap
{

    public interface IUserProvider
    {
        bool Authenticated
        {
            get;
        }

        User ActiveUser
        {
            get;
        }
    }

}
