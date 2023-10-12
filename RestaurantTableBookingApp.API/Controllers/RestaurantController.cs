using LSC.RestaurantTableBookingApp.Core.ViewModels;
using LSC.RestaurantTableBookingApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace LSC.RestaurantTableBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]//the end points from this controller is used by any user without login so it should be ananymous
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IReservationService reservationService;
        private readonly IEmailNotification emailNotification;

        public RestaurantController(IRestaurantService restaurantService, IReservationService reservationService,
            IEmailNotification emailNotification)
        {
            _restaurantService = restaurantService;
            this.reservationService = reservationService;
            this.emailNotification = emailNotification;
        }

        [HttpGet("restaurants")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RestaurantModel>))]

        public async Task<ActionResult<IEnumerable<RestaurantModel>>> GetAllRestaurantsAsync()
        {
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();
            return Ok(restaurants);
        }

        [HttpGet("branches/{restaurantId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RestaurantBranchModel>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<RestaurantBranchModel>>> GetRestaurantBranchsByRestaurantIdAsync(int restaurantId)
        {
            var branches = await _restaurantService.GetRestaurantBranchsByRestaurantIdAsync(restaurantId);
            if (branches == null)
            {
                return NotFound();
            }
            return Ok(branches);
        }

        [HttpGet("diningtables/{branchId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DiningTableWithTimeSlotsModel>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<DiningTableWithTimeSlotsModel>>> GetDiningTablesByBranchAsync(int branchId)
        {
            var diningTables = await _restaurantService.GetDiningTablesByBranchAsync(branchId);
            if (diningTables == null)
            {
                return NotFound();
            }
            return Ok(diningTables);
        }

        [HttpGet("diningtables/{branchId}/{date}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DiningTableWithTimeSlotsModel>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<DiningTableWithTimeSlotsModel>>> GetDiningTablesByBranchAndDateAsync(int branchId, DateTime date)
        {
            var diningTables = await _restaurantService.GetDiningTablesByBranchAsync(branchId, date);
            if (diningTables == null)
            {
                return NotFound();
            }
            return Ok(diningTables);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ReservationModel))]
        [ProducesResponseType(400)]
        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        [AllowAnonymous]
        public async Task<ActionResult<ReservationModel>> CreateReservationAsync(ReservationModel reservation)
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

            // Create a new reservation
            var newReservation = new ReservationModel
            {
                UserId = reservation.UserId,
                FirstName = reservation.FirstName,
                LastName = reservation.LastName,
                EmailId = reservation.EmailId,
                PhoneNumber = reservation.PhoneNumber,
                TimeSlotId = reservation.TimeSlotId,
                ReservationDate = reservation.ReservationDate,
                ReservationStatus = reservation.ReservationStatus
            };

            var createdReservation = await reservationService.CreateOrUpdateReservationAsync(newReservation);
            await emailNotification.SendBookingEmailAsync(reservation);            
            
            return new CreatedResult("GetReservation", new { id = createdReservation });
        }

        [HttpGet("getreservations")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReservationDetailsModel>))]
        [ProducesResponseType(404)]
        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<ActionResult<IEnumerable<ReservationDetailsModel>>> GetReservationDetails(int branchId, DateTime date)
        {
            var reservations = await reservationService.GetReservationDetails();
           
            return Ok(reservations);
        }

    }
}
