using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApi.Migrations
{
    /// <inheritdoc />
    public partial class PopulatingGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Genres (Name) VALUES ('Science fiction')");
            migrationBuilder.Sql("INSERT INTO Genres (Name) VALUES ('Fantasy')");
            migrationBuilder.Sql("INSERT INTO Genres (Name) VALUES ('Mystery')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE From Genres");
        }
    }
}
