using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GeorgiaTech.Application.Contracts;
using GeorgiaTech.Product.Application.Contracts.Persistence;


namespace GeorgiaTech.Product.Persistence
{
    public static class ProductInfrastructureRegistration
    {
        public static IServiceCollection AddProductInfrastructureServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
