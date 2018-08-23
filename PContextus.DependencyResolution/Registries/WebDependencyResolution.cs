namespace PG.Services.Console.DependencyResolution.Registries
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using PContextus.DependencyResolution;

    public sealed class WebDependencyResolution : IDependency
    {
        public void Register(IServiceCollection services)
        {
            //services.AddSingleton(conf => conf.GetService<IOptionsSnapshot<JwtConfiguration>>().Value);
        }
    }
}
