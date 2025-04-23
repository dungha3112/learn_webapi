

using System.Security.Claims;
using Newtonsoft.Json;

namespace api.Extensions
{
    public static class ClaimExtensions
    {
        public static string? GetUsername(this ClaimsPrincipal user)
        {


            // var result = user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")).Value;
            // Console.WriteLine("----------------- ClaimExtensions ---------------");
            // Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            // return result;

            var possibleClaims = new[] {
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname",
                ClaimTypes.Name,
                ClaimTypes.Email,
                "username"
            };

            foreach (var type in possibleClaims)
            {
                var claim = user.Claims.FirstOrDefault(c => c.Type == type);
                Console.WriteLine("claim.Value :" + claim!.Value);
                if (claim != null) return claim.Value;
            }

            Console.WriteLine("⚠️ Không tìm thấy username trong claims.");
            return null;
        }
    }
}
