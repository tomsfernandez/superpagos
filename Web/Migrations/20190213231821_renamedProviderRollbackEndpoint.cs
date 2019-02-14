using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class renamedProviderRollbackEndpoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RollbackEndPoint",
                table: "Providers",
                newName: "RollbackEndpoint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RollbackEndpoint",
                table: "Providers",
                newName: "RollbackEndPoint");
        }
    }
}
