using Microsoft.EntityFrameworkCore.Migrations;

namespace Scheduler.Migrations
{
    public class CreateSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder){
            migrationBuilder.EnsureSchema("public");
        }

        protected override void Down(MigrationBuilder migrationBuilder){
            migrationBuilder.DropSchema("public");
        }
    }
}
