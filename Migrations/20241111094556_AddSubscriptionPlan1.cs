using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAASLMS.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionPlan1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaxUserLimit",
                table: "Tenants",
                newName: "SubscriptionPlanId");

            migrationBuilder.AddColumn<string>(
                name: "SiteUrl",
                table: "Tenants",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MaximumUsersUpperLimit",
                table: "SubscriptionPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 1,
                column: "MaximumUsersUpperLimit",
                value: 100);

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 2,
                column: "MaximumUsersUpperLimit",
                value: 1000);

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 3,
                column: "MaximumUsersUpperLimit",
                value: 10000);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteUrl",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "MaximumUsersUpperLimit",
                table: "SubscriptionPlans");

            migrationBuilder.RenameColumn(
                name: "SubscriptionPlanId",
                table: "Tenants",
                newName: "MaxUserLimit");
        }
    }
}
