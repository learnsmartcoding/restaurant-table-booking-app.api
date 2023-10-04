using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LSC.RestaurantTableBookingApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class reservation_reminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReminderSent",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReminderSent",
                table: "Reservations");
        }
    }
}
