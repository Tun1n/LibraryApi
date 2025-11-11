using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApi.Migrations
{
    /// <inheritdoc />
    public partial class PopulatingBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Books (Title,Description,Author,Price,YearLaunch,Stock,GenreId) " +
           "VALUES ('Dune','Dune, written by Frank Herbert, is a science fiction novel set in the distant future, where interplanetary empires vie for control of the desert planet Arrakis','Frank Herbert',29.99,1965,100,1)");

            migrationBuilder.Sql("INSERT INTO Books (Title,Description,Author,Price,YearLaunch,Stock,GenreId) " +
                "VALUES ('The Hobbit','The Hobbit, written by J.R.R. Tolkien, is a fantasy novel that follows the journey of Bilbo Baggins, a hobbit who embarks on an adventurous quest to help a group of dwarves reclaim their homeland from the dragon Smaug','J.R.R. Tolkien',19.99,1937,150,2)");

            migrationBuilder.Sql("INSERT INTO Books (Title,Description,Author,Price,YearLaunch,Stock,GenreId) " +
                "VALUES ('The Hound of the Baskervilles','The Hound of the Baskervilles, written by Sir Arthur Conan Doyle, is a mystery novel featuring the famous detective Sherlock Holmes as he investigates the legend of a supernatural hound that haunts the Baskerville family on the moors of Devonshire','Arthur Conan Doyle',14.99,1902,200,3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE From Books");
        }
    }
}



