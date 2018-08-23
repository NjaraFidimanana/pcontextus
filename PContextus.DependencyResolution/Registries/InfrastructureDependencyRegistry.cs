
using Contextus.Core.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PContextus.Core.Configuration;
using PContextus.Infrastructure.MongoDb;

namespace PContextus.DependencyResolution
{
    
    public sealed class InfrastructureDependencyRegistry : IDependency
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped(conf => conf.GetService<IOptionsSnapshot<DatabaseConfiguration>>().Value);
          
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IRepository, Repository>();
       
        }
    }
}
