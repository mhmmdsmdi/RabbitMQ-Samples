using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DirectShared;

public class RabbitMqOptionSetup : IConfigureOptions<RabbitMqOptions>
{
    private readonly IConfiguration _configuration;
    private const string SectionName = "rabbitmq";

    public RabbitMqOptionSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(RabbitMqOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}