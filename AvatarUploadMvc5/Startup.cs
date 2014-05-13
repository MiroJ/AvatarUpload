using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(test_AvatarUpload.Startup))]
namespace test_AvatarUpload
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
