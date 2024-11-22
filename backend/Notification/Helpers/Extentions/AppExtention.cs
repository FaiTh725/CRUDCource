using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Notification.Domain.Contacrs.Email;
using Notification.Features.Consumers;
using Notification.Helpers.Setting;
using Notification.Validators;

namespace Notification.Helpers.Extentions
{
    public static class AppExtention
    {
        public static void AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            var messageBrokerSetting = configuration.GetSection("RabbitMqSetting").Get<MessageBrokerSetting>();

            services.AddMassTransit(conf =>
            {
                conf.AddConsumer<SentEmailConsumer>();

                conf.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri($"amqp://{messageBrokerSetting!.Host}"), x =>
                    {
                        x.Username(messageBrokerSetting.Login);
                        x.Password(messageBrokerSetting.Password);

                    });

                    configurator.ConfigureEndpoints(context);
                });
            });
        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();

            services.AddScoped<IValidator<SendEmail>, EmailValidator>();
        }
    }
}
