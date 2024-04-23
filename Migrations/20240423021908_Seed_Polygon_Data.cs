using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASAP_Clients.Migrations
{
    /// <inheritdoc />
    public partial class Seed_Polygon_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                "PolygonRequestTypes",
                columns: ["ID", "Name"],
                values: [1, "Previous Close"]);

            migrationBuilder.InsertData(
                "PolygonTickers",
                columns: ["ID", "Name", "CompanyName"],
                values: new object[,] {
                    {1, "MSFT", "Microsoft Corp."},
                    {2, "AMZN", "Amazon.Com Inc."},
                    {3, "META", "Meta Platforms, Inc. Class A Common Stock"},
                    {4, "AAPL", "Apple Inc."},
                    {5, "NFLX", "NetFlix Inc."}
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PolygonTickers",
                keyColumn: "ID",
                keyValues: [1, 2, 3, 4, 5]);

            migrationBuilder.DeleteData(
                table: "PolygonRequestTypes",
                keyColumn: "ID",
                keyValue: 1);
        }
    }
}
