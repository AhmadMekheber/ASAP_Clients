using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASAP_Clients.Migrations
{
    /// <inheritdoc />
    public partial class Added_Polygon_Models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PolygonRequestTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolygonRequestTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PolygonTickers",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolygonTickers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PolygonRequests",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestTypeID = table.Column<int>(type: "int", nullable: false),
                    TickerID = table.Column<long>(type: "bigint", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolygonRequests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PolygonRequests_PolygonRequestTypes_RequestTypeID",
                        column: x => x.RequestTypeID,
                        principalTable: "PolygonRequestTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PolygonRequests_PolygonTickers_TickerID",
                        column: x => x.TickerID,
                        principalTable: "PolygonTickers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreviousCloseResponses",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClosePrice = table.Column<decimal>(type: "decimal(16,4)", precision: 16, scale: 4, nullable: false),
                    HighestPrice = table.Column<decimal>(type: "decimal(16,4)", precision: 16, scale: 4, nullable: false),
                    LowestPrice = table.Column<decimal>(type: "decimal(16,4)", precision: 16, scale: 4, nullable: false),
                    OpenPrice = table.Column<decimal>(type: "decimal(16,4)", precision: 16, scale: 4, nullable: false),
                    VolumeWeightedAveragePrice = table.Column<decimal>(type: "decimal(16,4)", precision: 16, scale: 4, nullable: false),
                    TransactionsCount = table.Column<long>(type: "bigint", nullable: false),
                    TradingVolume = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AggregateWindowDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsClientsNotified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousCloseResponses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PreviousCloseResponses_PolygonRequests_RequestID",
                        column: x => x.RequestID,
                        principalTable: "PolygonRequests",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PolygonRequests_RequestTypeID",
                table: "PolygonRequests",
                column: "RequestTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_PolygonRequests_TickerID",
                table: "PolygonRequests",
                column: "TickerID");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousCloseResponses_RequestID",
                table: "PreviousCloseResponses",
                column: "RequestID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreviousCloseResponses");

            migrationBuilder.DropTable(
                name: "PolygonRequests");

            migrationBuilder.DropTable(
                name: "PolygonRequestTypes");

            migrationBuilder.DropTable(
                name: "PolygonTickers");
        }
    }
}
