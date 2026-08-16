namespace Movies.Api.Auth
{
    public static class AuthExtensions
    {
        public static Guid? GetUserId(this HttpContext context) {


            var userId = context.User?.Claims?.SingleOrDefault(x => x.Type == "userid");

            if(Guid.TryParse(userId?.Value,out Guid parsedId))
            {
                return parsedId; 
            }

            return null;
        
        }
    }
}
