using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PContextus.Core.Domain;
using PContextus.Core.Interfaces;
using PContextus.Core.Services;

namespace PContextus.DependencyResolution
{
  

    public sealed class CoreDependencyRegistry : IDependency
    {
        public void Register(IServiceCollection services)
        {

            services.AddScoped<IRecommendationContentService, RecommendationContentService>();

            services.AddScoped<IContentAgentService, ContentAgentService > ();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IBusinessRule, BusinessRulesService>();

        }
    }
}
