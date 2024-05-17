using Microsoft.EntityFrameworkCore.Migrations;
using Shared.Models;

namespace Backend.DbContext;

public class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Tours",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(nullable: false),
                TourDescription = table.Column<string>(nullable: false),
                From = table.Column<string>(nullable: false),
                To = table.Column<string>(nullable: false),
                TransportationType = table.Column<TransportationType>(nullable: false),
                Distance = table.Column<double>(nullable: false),
                Duration = table.Column<TimeSpan>(nullable: false),
                Image = table.Column<string>(nullable: false),
                
            },
            constraints: table => { table.PrimaryKey("PK_Tours", x => x.Id); });
    }
}