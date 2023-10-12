using LSC.RestaurantTableBookingApp.Core.ViewModels;
using LSC.RestaurantTableBookingApp.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSC.RestaurantTableBookingApp.Data.BackgroundService
{
    public class ReminderService : BackgroundService
    {
        private readonly ILogger<ReminderService> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public ReminderService(
            ILogger<ReminderService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"ReminderService is starting.");
            stoppingToken.Register(() =>
                logger.LogInformation($" ReminderService background task is stopping."));


            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = serviceScopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<RestaurantTableBookingDbContext>();
                        var emailNotificationService = scope.ServiceProvider.GetRequiredService<IEmailNotification>();

                        var remindersToSend = await dbContext.Reservations.AsNoTracking()
                              .Where(r => r.ReservationStatus == "Booked" &&
                                          DateTime.Compare(DateTime.Now, r.ReservationDate.AddHours(24)) < 0 &&
                                          //&& DateTime.Compare(DateTime.Now, r.ReservationDate.AddHours(24)) >1
                                          r.ReminderSent == false)
                              .ToListAsync();

                        remindersToSend.ForEach(b =>
                        {
                            var userInfo = dbContext.Users.FirstOrDefault(f => f.Id == b.UserId);
                            var model = new ReservationModel()
                            {
                                EmailId = userInfo.Email,
                                FirstName = userInfo.FirstName,
                                LastName = userInfo.LastName,
                                TimeSlotId = b.TimeSlotId,
                                ReservationDate = b.ReservationDate
                            };

                            var response = emailNotificationService.SendBookingEmailAsync(model, isReminderEmail: true).Result;
                            logger.LogInformation($"Reminder status:{response.StatusCode}");
                            if (response.IsSuccessStatusCode)
                            {
                                var reservationToUpdate = dbContext.Reservations.FirstOrDefault(f => f.UserId == b.UserId && f.TimeSlotId == b.TimeSlotId);
                                reservationToUpdate.ReminderSent = true;
                                dbContext.Reservations.Update(reservationToUpdate);
                                dbContext.SaveChanges();
                            }
                        });

                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"ReminderService task failed with exception", ex);
                }


                logger.LogInformation($"ReminderService task doing background work.");

                await Task.Delay(2000, stoppingToken);
            }

            logger.LogInformation($"ReminderService background task is stopping.");
        }
    }
}
