using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AvatarUploadMvc5.Startup))]
namespace AvatarUploadMvc5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
