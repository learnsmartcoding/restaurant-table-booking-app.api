namespace LSC.RestaurantTableBookingApp.API.Middleware
{
    public class DelayMiddleware
    {
        private readonly RequestDelegate _next;

        public DelayMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Introduce a delay of 1 second
            await Task.Delay(TimeSpan.FromSeconds(1));

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}
