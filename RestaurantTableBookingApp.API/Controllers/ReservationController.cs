using LSC.RestaurantTableBookingApp.API.PermissionValidation;
using LSC.RestaurantTableBookingApp.Core;
using LSC.RestaurantTableBookingApp.Core.ViewModels;
using LSC.RestaurantTableBookingApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using System.Security.Claims;

namespace LSC.RestaurantTableBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService reservationService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPermissionValidation permissionValidation;
        private readonly IEmailNotification emailNotification;
        private ClaimsPrincipal _currentPrincipal;
        /// <summary>
        /// We store the object id of the user/app derived from the presented Access token
        /// </summary>
        private string _currentPrincipalId = string.Empty;

        public ReservationController(IReservationService reservationService, IPermissionValidation permissionValidation,
            IEmailNotification emailNotification,
            IHttpContextAccessor contextAccessor )
        {
            this.reservationService = reservationService;
            _contextAccessor = contextAccessor;
            this.permissionValidation = permissionValidation;
            this.emailNotification = emailNotification;

            // We seek the details of the user/app represented by the access token presented to this API, This can be empty unless authN succeeded
            // If a user signed-in, the value will be the unique identifier of the user.
            _currentPrincipal = GetCurrentClaimsPrincipal();

            if (!IsAppOnlyToken() && _currentPrincipal != null)
            {
                // The default behavior of the JwtSecurityTokenHandler is to map inbound claim names to new values in the generated ClaimsPrincipal. 
                // The result is that "sub" claim that identifies the subject of the incoming JWT token is mapped to a claim
                // named "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier". An alternative approach is to 
                // disable this mapping by setting JwtSecurityTokenHandler.DefaultMapInboundClaims to false in Startup.cs and
                // then calling _currentPrincipal.FindFirstValue(ClaimConstants.Sub) to obtain the value of the unmapped "sub" claim.
                _currentPrincipalId = _currentPrincipal.GetNameIdentifierId(); // use "sub" claim as a unique identifier in B2C

            }
        }


        [HttpGet("{id}", Name = "GetReservation")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            // Your logic to retrieve and return a reservation
            return Ok();
        }


        [HttpPost("CheckIn")]
        [ProducesResponseType(200, Type = typeof(ReservationModel))]
        [ProducesResponseType(400)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<ActionResult<ReservationModel>> CheckInReservationAsync(DiningTableWithTimeSlotsModel reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the selected time slot exists
            var timeSlot = await reservationService.TimeSlotIdExistAsync(reservation.TimeSlotId);
            if (!timeSlot)
            {
                return NotFound("Selected time slot not found.");
            }
            var response = await reservationService.CheckInReservationAsync(reservation);
            await emailNotification.SendCheckInEmailAsync(reservation);
            return Ok(response);
        }






        /// <summary>
        /// returns the current claimsPrincipal (user/Client app) dehydrated from the Access token
        /// </summary>
        /// <returns></returns>
        private ClaimsPrincipal GetCurrentClaimsPrincipal()
        {
            // Irrespective of whether a user signs in or not, the AspNet security middleware dehydrates 
            // the claims in the HttpContext.User.Claims collection
            if (_contextAccessor.HttpContext != null && _contextAccessor.HttpContext.User != null)
            {
                return _contextAccessor.HttpContext.User;
            }

            return null;
        }

        /// <summary>
        /// Indicates of the AT presented was for an app-only token or not.
        /// </summary>
        /// <returns></returns>
        private bool IsAppOnlyToken()
        {
            // Add in the optional 'idtyp' claim to check if the access token is coming from an application or user.
            //
            // See: https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-optional-claims

            if (GetCurrentClaimsPrincipal() != null)
            {
                return GetCurrentClaimsPrincipal().Claims.Any(c => c.Type == "idtyp" && c.Value == "app");
            }

            return false;
        }

    }
}
