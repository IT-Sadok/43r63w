using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Policy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_PolicyScheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PolicyHistory_Policies_PolicyId",
                schema: "policy",
                table: "PolicyHistory");
            
            migrationBuilder.AddForeignKey(
                name: "FK_PolicyHistory_Policies_PolicyId",
                schema: "policy",
                table: "PolicyHistory",
                column: "PolicyId",
                principalSchema: "policy",
                principalTable: "Policies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PolicyHistory_Policies_PolicyId",
                schema: "policy",
                table: "PolicyHistory");
            
            migrationBuilder.AddForeignKey(
                name: "FK_PolicyHistory_Policies_PolicyId",
                schema: "policy",
                table: "PolicyHistory",
                column: "PolicyId",
                principalSchema: "policy",
                principalTable: "Policies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
