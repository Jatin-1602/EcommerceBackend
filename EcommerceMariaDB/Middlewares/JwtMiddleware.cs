using Microsoft.IdentityModel.Tokens;


namespace EcommerceMariaDB.Middlewares
{
    public class JwtMiddleware : IMiddleware
    {
        private readonly JwtSecurityTokenHandlerWrapper _jwtSecurityTokenHandler;

        public JwtMiddleware(JwtSecurityTokenHandlerWrapper jwtSecurityTokenHandler)
        {
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Get the token from the Authorization header
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!token.IsNullOrEmpty())
            {

                try
                {
                    // Verify the token using the JwtSecurityTokenHandlerWrapper
                    var claimsPrincipal = _jwtSecurityTokenHandler.ValidateJwtToken(token);

                    foreach(var  claim in claimsPrincipal.Claims) {
                        Console.WriteLine($"{claim.Type} : {claim.Value}");
                    }
                    // Extract the user ID from the token
                    var userId = claimsPrincipal.FindFirst(c => c.Type == "id")?.Value;
                    var email = claimsPrincipal.FindFirst(c => c.Type == "email")?.Value;

                    Console.WriteLine($"In middleware : {userId}");
                    Console.WriteLine($"In middleware : {email}");

                    // Store the user ID in the HttpContext items for later use
                    context.Items["userId"] = userId;

                    // You can also do the for same other key which you have in JWT token.
                    context.Items["email"] = email;
                }
                catch (Exception)
                {
                    // If the token is invalid, throw an exception
                    //context
                }


            }
            // Continue processing the request
            await next(context);
        }
    }
}
