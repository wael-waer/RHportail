
namespace BuildingBlocks.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>
       (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
       : IPipelineBehavior<TRequest, TResponse>
       where TRequest : notnull, IRequest<TResponse>
       where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
                typeof(TRequest).Name, typeof(TResponse).Name, request);
            var timer = new Stopwatch();
            timer.Start();
            var response = await next();
            timer.Stop();
            var timeTaken = timer.Elapsed;
            if (timeTaken.Seconds > 3)//if the request takes more than 3 seconds, then log it as a warning
                logger.LogWarning("[PERFOMANCE] The request {Request} took {TimeTaken} seconds ",
                    typeof(TRequest).Name, timeTaken);

            logger.LogInformation("[END] Handled {Request} with {Response}  took {TimeTaken} seconds",
                   typeof(TRequest).Name, typeof(TResponse).Name, timeTaken);

            return response;
        }
    }
}
