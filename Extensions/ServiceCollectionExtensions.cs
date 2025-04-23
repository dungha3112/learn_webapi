
using api.Interfaces;
using api.Repositories;
using api.Services;

namespace api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IAuthServices, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
