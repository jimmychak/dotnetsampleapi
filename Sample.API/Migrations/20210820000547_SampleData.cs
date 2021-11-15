using Microsoft.EntityFrameworkCore.Migrations;

namespace Sample.API.Migrations
{
    public partial class SampleData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 1, "A city in the northern English county of Yorkshire", "Leeds" });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 2, "A city in the English county of South Yorkshire", "Sheffield" });

            migrationBuilder.InsertData(
                table: "PointsOfInterest",
                columns: new[] { "Id", "CityId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 1, "A ruined Cistercian monastery", "Kirkstall Abbey" },
                    { 2, 1, "One of the largest indoor markets in Europe", "Kirkgate Market" },
                    { 3, 2, "Expansive 19th-century gardens", "Botanical Gardens" },
                    { 4, 2, "Museum of Sheffield's industrial history", "Kelham Island Museum" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
