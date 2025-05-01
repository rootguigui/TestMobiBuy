using MassTransit;
using TestMobiBuy.Consumer;
using TestMobiBuy.Consumer.Consumers;
using TestMobiBuy.Domain.Settings;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{
    services.AddHostedService<Worker>();

    var rabbitMqConfig = hostContext.Configuration.GetSection("RabbitMq").Get<RabbitMqSettings>();
    services.Configure<SmtpSettings>(hostContext.Configuration.GetSection(SmtpSettings.SectionName));

    services.AddMassTransit(x =>
    {
        x.AddConsumer<EmailSendConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(rabbitMqConfig.Host, rabbitMqConfig.VirtualHost, h =>
            {
                h.Username(rabbitMqConfig.Username);
                h.Password(rabbitMqConfig.Password);
            });

            cfg.ReceiveEndpoint("email-send-queue", e =>
            {
                e.ConfigureConsumer<EmailSendConsumer>(context);
            });
        });
    });
});

var host = builder.Build();
host.Run();
