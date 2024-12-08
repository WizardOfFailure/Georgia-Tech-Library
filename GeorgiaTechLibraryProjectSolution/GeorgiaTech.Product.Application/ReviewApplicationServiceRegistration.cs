using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GeorgiaTech.Application;
using GeorgiaTech.Application.Contracts;

namespace GeorgiaTech.Product.Application
{
    public static class ReviewApplicationServiceRegistration
    {
        public static IServiceCollection AddReviewApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IDispatcher>(sp => new Dispatcher(sp.GetService<IMediator>()));

            return services;
        }
    }
}
