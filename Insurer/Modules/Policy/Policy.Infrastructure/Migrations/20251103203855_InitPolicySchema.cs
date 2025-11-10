using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Policy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitPolicySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "policy");

            migrationBuilder.CreateSequence<int>(
                name: "PolicyNumbers",
                schema: "policy",
                startValue: 1000L);

            migrationBuilder.CreateTable(
                name: "Policies",
                schema: "policy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolicyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    PolicyType = table.Column<int>(type: "int", nullable: false),
                    CoverageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PolicyClaims",
                schema: "policy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolicyId = table.Column<int>(type: "int", nullable: false),
                    ClaimNumber = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ClaimAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DecisionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PolicyClaims_Policies_PolicyId",
                        column: x => x.PolicyId,
                        principalSchema: "policy",
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PolicyHistory",
                schema: "policy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolicyId = table.Column<int>(type: "int", nullable: false),
                    ChangeType = table.Column<int>(type: "int", nullable: false),
                    ChangedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PolicyId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PolicyHistory_Policies_PolicyId",
                        column: x => x.PolicyId,
                        principalSchema: "policy",
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PolicyHistory_Policies_PolicyId1",
                        column: x => x.PolicyId1,
                        principalSchema: "policy",
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPayments",
                schema: "policy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolicyId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPayments_Policies_PolicyId",
                        column: x => x.PolicyId,
                        principalSchema: "policy",
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyPayments",
                schema: "policy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyPayments_PolicyClaims_ClaimId",
                        column: x => x.ClaimId,
                        principalSchema: "policy",
                        principalTable: "PolicyClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                schema: "policy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolicyId = table.Column<int>(type: "int", nullable: true),
                    PolicyClaimId = table.Column<int>(type: "int", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileType = table.Column<int>(type: "int", maxLength: 64, nullable: false),
                    UploadedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Policies_PolicyId",
                        column: x => x.PolicyId,
                        principalSchema: "policy",
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_PolicyClaims_PolicyClaimId",
                        column: x => x.PolicyClaimId,
                        principalSchema: "policy",
                        principalTable: "PolicyClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPayments_ClaimId",
                schema: "policy",
                table: "CompanyPayments",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PolicyClaimId",
                schema: "policy",
                table: "Documents",
                column: "PolicyClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PolicyId",
                schema: "policy",
                table: "Documents",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Policies_PolicyNumber",
                schema: "policy",
                table: "Policies",
                column: "PolicyNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PolicyClaims_PolicyId",
                schema: "policy",
                table: "PolicyClaims",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyHistory_PolicyId",
                schema: "policy",
                table: "PolicyHistory",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyHistory_PolicyId1",
                schema: "policy",
                table: "PolicyHistory",
                column: "PolicyId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserPayments_PolicyId",
                schema: "policy",
                table: "UserPayments",
                column: "PolicyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyPayments",
                schema: "policy");

            migrationBuilder.DropTable(
                name: "Documents",
                schema: "policy");

            migrationBuilder.DropTable(
                name: "PolicyHistory",
                schema: "policy");

            migrationBuilder.DropTable(
                name: "UserPayments",
                schema: "policy");

            migrationBuilder.DropTable(
                name: "PolicyClaims",
                schema: "policy");

            migrationBuilder.DropTable(
                name: "Policies",
                schema: "policy");

            migrationBuilder.DropSequence(
                name: "PolicyNumbers",
                schema: "policy");
        }
    }
}
