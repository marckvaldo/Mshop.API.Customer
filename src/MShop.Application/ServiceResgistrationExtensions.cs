using Microsoft.Extensions.DependencyInjection;
using Mshop.Application.Event;
using Mshop.Core.Message;
using Mshop.Core.Message.DomainEvent;
using MShop.Domain.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Message = Mshop.Core.Message;

namespace MShop.Application
{
    public static class ServiceResgistrationExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
            services.AddScoped<IDomainEventHandler<CreatedCustomerEvent>, CreatedCustomerEventHandler>();
            services.AddScoped<Message.INotification, Notifications>();
            services.AddMediatR(x=>x.RegisterServicesFromAssemblies(typeof(ServiceResgistrationExtensions).Assembly));
            return services;
        }
    }
}
