using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product.Migrations
{
    /// <inheritdoc />
    public partial class AddcolumnIsReviewetocheckstatusstatusofrequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsComplete",
                table: "ChangeRoleRequests",
                newName: "IsReviewed");

            migrationBuilder.AddColumn<bool>(
                name: "IsCommite",
                table: "ChangeRoleRequests",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCommite",
                table: "ChangeRoleRequests");

            migrationBuilder.RenameColumn(
                name: "IsReviewed",
                table: "ChangeRoleRequests",
                newName: "IsComplete");
        }
    }
}
