using System.Security.Claims;

namespace LSC.RestaurantTableBookingApp.API.PermissionValidation
{
    public interface IPermissionValidation
    {
        string GetCurrentUserFirstName();
        string GetCurrentUserLastName();
        string GetCurrentUserEmail();
        string GetCurrentUserId();
    }
    public class PermissionValidation: IPermissionValidation
    {
        public PermissionValidation(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }

        public string GetCurrentContextUserId()
        {
            return GetCurrentUserId();
        }
        private string GetClaimInfo(string property)
        {
            var propertyData = "";
            var identity = HttpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                propertyData = identity.Claims.FirstOrDefault(d => d.Type.Contains(property)).Value;

            }
            return propertyData;
        }

        public string GetCurrentUserFirstName()
        {
            return GetClaimInfo("emails");
        }
        public string GetCurrentUserLastName()
        {
            return GetClaimInfo("emails");
        }
        public string GetCurrentUserEmail()
        {
            return GetClaimInfo("emails");
        }

        public string GetCurrentUserId()
        {
            return GetClaimInfo("objectidentifier");
        }
    }
}
