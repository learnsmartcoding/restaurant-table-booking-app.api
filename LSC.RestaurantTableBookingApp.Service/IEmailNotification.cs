using LSC.RestaurantTableBookingApp.Core.ViewModels;
using LSC.RestaurantTableBookingApp.Data;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LSC.RestaurantTableBookingApp.Service
{
    public interface IEmailNotification
    {
        Task<Response> SendBookingEmailAsync(ReservationModel model, bool isReminderEmail = false);
        Task<Response> SendCheckInEmailAsync(DiningTableWithTimeSlotsModel model);
    }

    public class EmailNotification : IEmailNotification
    {
        private readonly IConfiguration configuration;
        private readonly IRestaurantRepository restaurantRepository;

        public EmailNotification(IConfiguration configuration, IRestaurantRepository restaurantRepository)
        {
            this.configuration = configuration;
            this.restaurantRepository = restaurantRepository;
        }
        public async Task<Response> SendBookingEmailAsync(ReservationModel model, bool isReminderEmail = false)
        {
            var apiKey = configuration["SendGrid:SENDGRID_API_KEY"];
            var from = new EmailAddress(configuration["SendGrid:From"]);
            var to = new EmailAddress(model.EmailId, $"{model.LastName},{model.FirstName}");

            var sendGridMessage = new SendGridMessage()
            {
                From = from,
                Subject = "Restaurant Reservation Confirmation" + (isReminderEmail ? " - Reminder" : "")
            };

            var reservationDetails = await restaurantRepository.GetRestaurantReservationDetailsAsync(model.TimeSlotId);

            sendGridMessage.AddContent(MimeType.Html, GetBookingEmailBody(model, reservationDetails,isReminderEmail));
            sendGridMessage.AddTo(to);

            Console.WriteLine($"Sending email with payload: \n{sendGridMessage.Serialize()}");

            var response = await new SendGridClient(apiKey).SendEmailAsync(sendGridMessage).ConfigureAwait(false);
            Console.WriteLine($"Response: {response.StatusCode}");
            Console.WriteLine(response.Headers);

            return response;
        }

        public async Task<Response> SendCheckInEmailAsync(DiningTableWithTimeSlotsModel model)
        {
            var userInfo = await restaurantRepository.GetUserAsync(model.UserEmailId??"");
            var apiKey = configuration["SendGrid:SENDGRID_API_KEY"];
            var from = new EmailAddress(configuration["SendGrid:From"]);
            var to = new EmailAddress(model.UserEmailId, $"{userInfo.LastName},{userInfo.FirstName}");

            var sendGridMessage = new SendGridMessage()
            {
                From = from,
                Subject = "Restaurant Check-In Confirmation"
            };

            var reservationDetails = await restaurantRepository.GetRestaurantReservationDetailsAsync(model.TimeSlotId);

            sendGridMessage.AddContent(MimeType.Html, GetCheckInEmailBody(userInfo.FirstName, userInfo.LastName, reservationDetails));
            sendGridMessage.AddTo(to);

            Console.WriteLine($"Sending email with payload: \n{sendGridMessage.Serialize()}");

            var response = await new SendGridClient(apiKey).SendEmailAsync(sendGridMessage).ConfigureAwait(false);
            Console.WriteLine($"Response: {response.StatusCode}");
            Console.WriteLine(response.Headers);

            return response;

        }

        private string GetCheckInEmailBody(string firstName, string lastName, RestaurantReservationDetails reservationDetails)
        {
            return $$"""
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset="UTF-8">
                        <title>Restaurant Check-In Confirmation</title>
                    </head>
                    <body>
                        <p><strong>Thank You for Checking In at {{reservationDetails.RestaurantName}}</strong></p>
                        <p>Dear {{lastName}}, {{firstName}},</p>
                        <p>We're delighted that you've checked in at {{reservationDetails.RestaurantName}}. Your visit is important to us, and we hope you have a wonderful dining experience.</p>

                        <p><strong>Check-In Details:</strong></p>
                        <ul>                            
                           <li>Restaurant Name: {{reservationDetails.RestaurantName}}</li>
                            <li>Branch Name: {{reservationDetails.BranchName}}</li>
                            <li>Table Name: {{reservationDetails.TableName}}</li>
                            <li>Capacity: {{reservationDetails.Capacity}} guests</li>
                            <li>Meal Type: {{reservationDetails.MealType}}</li>
                            <li>Reservation Day: {{reservationDetails.ReservationDay}}</li>
                            <li>Check-In Date and Time: {{DateTime.Now}}</li>
                            <li>Address: {{reservationDetails.Address}}</li>
                        </ul>

                        <p>Your safety and comfort are our top priorities. Our team is dedicated to providing you with a memorable meal. If you have any specific preferences or requirements, please don't hesitate to inform our staff, and we'll do our utmost to accommodate them.

                        <p>Once again, thank you for choosing {{reservationDetails.RestaurantName}}. Enjoy your meal, and we look forward to serving you.

                        <p>If you have any questions or need assistance during your visit, please feel free to reach out to our staff, who will be more than happy to help.

                        <p>Warm regards,</p>
                        <p>[Learn Smart Coding]<br>{{reservationDetails.RestaurantName}}<br>[Contact Information]</p>
                    </body>
                    </html>
                    

                    """;
        }
        private string GetBookingEmailBody(ReservationModel model, RestaurantReservationDetails reservationDetails, bool isReminderEmail)
        {
            var reminder = (isReminderEmail ? " - Reminder" : "");
            return $$"""
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset=""UTF-8"">
                        <title>Restaurant Reservation Confirmation {{reminder}}</title>
                    </head>
                    <body>
                        <p><strong>Confirmation of Your Restaurant Table Reservation</strong></p>
                        <p>Dear {{model.LastName}}, {{model.FirstName}},</p>
                        <p>We are excited to confirm your reservation at {{reservationDetails.RestaurantName}} for {{model.ReservationDate}}. Your reservation status is {{model.ReservationStatus}}. We can't wait to welcome you and provide you with an exceptional dining experience.</p>
                    
                        <p><strong>Reservation Details:</strong></p>
                        <ul>
                            <li>Restaurant Name: {{reservationDetails.RestaurantName}}</li>
                            <li>Branch Name: {{reservationDetails.BranchName}}</li>
                            <li>Table Name: {{reservationDetails.TableName}}</li>
                            <li>Capacity: {{reservationDetails.Capacity}} guests</li>
                            <li>Meal Type: {{reservationDetails.MealType}}</li>
                            <li>Reservation Day: {{reservationDetails.ReservationDay}}</li>
                            <li>Address: {{reservationDetails.Address}}</li>
                        </ul>
                    
                        <p>We want to assure you that your table is all set and ready for your arrival. Our team is dedicated to making your dining experience memorable, and we've taken every precaution to ensure your safety and comfort.</p>
                    
                        <p>If you have any specific requests or dietary preferences, please feel free to let us know in advance, and we will do our best to accommodate them.</p>
                    
                        <p>If you have any questions or need to make any changes to your reservation, please do not hesitate to reach out to our reservation team at [Reservation Phone/Email]. We're here to assist you and make your visit as enjoyable as possible.</p>
                    
                        <p>Thank you for choosing {{reservationDetails.RestaurantName}}. We value your patronage and can't wait to provide you with a delightful dining experience.</p>
                    
                        <p>See you on {{model.ReservationDate}}!</p>
                    
                        <p><strong>Warm regards,</strong></p>
                        <p>[LearnSmartCoding]<br>{{reservationDetails.RestaurantName}}<br>[Contact Information]</p>
                    </body>
                    </html>                    
                    """;
        }

    
    }
}
