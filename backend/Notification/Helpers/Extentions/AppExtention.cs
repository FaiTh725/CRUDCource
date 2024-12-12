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
        public static void AddMessageBroker(this WebApplicationBuilder builder)
        {
            string rabbitMqSection = "";
            if (builder.Environment.IsDevelopment())
            {
                rabbitMqSection = "RabbitMqSetting";
            }
            else
            {
                rabbitMqSection = "RabbitMqSettingRelease";
            }

            var messageBrokerSetting = builder.Configuration.GetSection(rabbitMqSection).Get<MessageBrokerSetting>();

            builder.Services.AddMassTransit(conf =>
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
