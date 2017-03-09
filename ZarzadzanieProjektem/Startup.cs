using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using ZarzadzanieProjektem.Models;

[assembly: OwinStartupAttribute(typeof(ZarzadzanieProjektem.Startup))]
namespace ZarzadzanieProjektem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRoles();
        }
        public void CreateRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (!roleManager.RoleExists("Administrator"))//Role not existing creating new
            {
                var role = new IdentityRole();
                role.Name = "Administrator";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Student"))//Role not existing creating new
            {
                var role = new IdentityRole();
                role.Name = "Student";
                roleManager.Create(role);
            }
        }
    }
}
