namespace PContextus.DependencyResolution
{
    using Microsoft.Extensions.DependencyInjection;

    public interface IDependency
    {
        void Register(IServiceCollection services);
    }
}
