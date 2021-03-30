using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents;

namespace ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var commandName = request.GetGenericTypeName();

            _logger.LogTrace("----- Handling command {CommandName} ({@Command})", commandName, Newtonsoft.Json.JsonConvert.SerializeObject(request));
            var response = await next();
            _logger.LogTrace("----- Handled command {CommandName} - response: {@Response}", commandName, response);

            return response;
        }
    }
}