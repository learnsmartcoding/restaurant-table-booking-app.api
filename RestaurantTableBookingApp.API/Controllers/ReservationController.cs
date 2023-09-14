using LSC.RestaurantTableBookingApp.Core;
using LSC.RestaurantTableBookingApp.Core.ViewModels;
using LSC.RestaurantTableBookingApp.Service;
using Microsoft.AspNetCore.Mvc;

namespace LSC.RestaurantTableBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService reservationService;

        public ReservationController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
        }


        [HttpGet("{id}", Name = "GetReservation")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            // Your logic to retrieve and return a reservation
            return Ok();
        }


        [HttpPost("CheckIn")]
        [ProducesResponseType(200, Type = typeof(ReservationModel))]
        [ProducesResponseType(400)]
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
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ReservationModel))]
        [ProducesResponseType(400)]
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
            return CreatedAtAction("GetReservation", new { id = createdReservation }, createdReservation);
        }

    }
}
