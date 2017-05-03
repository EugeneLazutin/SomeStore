using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Owin;
using SomeStore.Infrustructure.Services;
using Unity.Mvc5;

[assembly: OwinStartup(typeof(SomeStore.Startup))]
namespace SomeStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            RegisterComponents();
        }

        private void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<ShoppingCartService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
