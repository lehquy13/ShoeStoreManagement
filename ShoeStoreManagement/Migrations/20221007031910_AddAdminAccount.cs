using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using ShoeStoreManagement.Areas.Identity.Data;
using System.Text;

#nullable disable

namespace ShoeStoreManagement.Migrations
{
	public partial class AddAdminAccount : Migration
	{

		const string ADMIN_USER_GUID = "0bf7bc9c-1804-4212-8220-5722f65994c3";
		const string ADMIN_ROLE_GUID = "9892149b-ebfd-420d-8864-c63b88121dec";
		const string PASSENGER_ROLE_GUID = "9892149b-ebfd-420d-8864-c63b88121dec1";

		protected override void Up(MigrationBuilder migrationBuilder)
		{
			var hasher = new PasswordHasher<ApplicationUser>();

			var passwordHash = hasher.HashPassword(null, "Password100!");

			StringBuilder sb = new StringBuilder();

			sb.AppendLine("INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName,Email,EmailConfirmed,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnabled,AccessFailedCount,NormalizedEmail,PasswordHash,SecurityStamp,CreatedDate,Birthday,PhoneNumber,FullName)");
			sb.AppendLine("VALUES(");
			sb.AppendLine($"'{ADMIN_USER_GUID}'");
			sb.AppendLine(",'admin@gmail.com'");
			sb.AppendLine(",'ADMIN@GMAIL.COM'");
			sb.AppendLine(",'admin@gmail.com'");
			sb.AppendLine(", 0");
			sb.AppendLine(", 0");
			sb.AppendLine(", 0");
			sb.AppendLine(", 0");
			sb.AppendLine(", 0");
			sb.AppendLine(",'ADMIN@GMAIL.COM'");
			sb.AppendLine($", '{passwordHash}'");
			sb.AppendLine(", ''");
			sb.AppendLine(", '01/01/2022'");
			sb.AppendLine(", '01/01/2002'");
			sb.AppendLine(", '19008060'");
			sb.AppendLine(", '19008060'");
			sb.AppendLine(")");

			migrationBuilder.Sql(sb.ToString());

			migrationBuilder.Sql($"INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES ('{ADMIN_ROLE_GUID}','Admin','ADMIN')");
			migrationBuilder.Sql($"INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES ('{ADMIN_USER_GUID}','Customer','CUSTOMER')");//cái này là dùng guid của 1 bạn admin để làm id, mb
			migrationBuilder.Sql($"INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES ('{PASSENGER_ROLE_GUID}','Passenger','PASSENGER')");

			migrationBuilder.Sql($"INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES ('{ADMIN_USER_GUID}','{ADMIN_ROLE_GUID}')");

			migrationBuilder.Sql($"INSERT INTO ProductCategories (ProductCategoryId, ProductCategoryName) VALUES ('1','Sneaker')");
			migrationBuilder.Sql($"INSERT INTO ProductCategories (ProductCategoryId, ProductCategoryName) VALUES ('2','Boot')");
			migrationBuilder.Sql($"INSERT INTO ProductCategories (ProductCategoryId, ProductCategoryName) VALUES ('3','Hiking Boot')");
			migrationBuilder.Sql($"INSERT INTO ProductCategories (ProductCategoryId, ProductCategoryName) VALUES ('4','Derby')");
			migrationBuilder.Sql($"INSERT INTO ProductCategories (ProductCategoryId, ProductCategoryName) VALUES ('5','Oxford')");
			migrationBuilder.Sql($"INSERT INTO ProductCategories (ProductCategoryId, ProductCategoryName) VALUES ('6','High heel')");
			migrationBuilder.Sql($"INSERT INTO ProductCategories (ProductCategoryId, ProductCategoryName) VALUES ('7','Loafer')");
			migrationBuilder.Sql($"INSERT INTO ProductCategories (ProductCategoryId, ProductCategoryName) VALUES ('8','Slip-on')");
			migrationBuilder.Sql($"INSERT INTO ProductCategories (ProductCategoryId, ProductCategoryName) VALUES ('9','Sandal')");
			migrationBuilder.Sql($"INSERT INTO ProductCategories (ProductCategoryId, ProductCategoryName) VALUES ('10','Hand Bag')");


		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql($"DELETE FROM AspNetUserRoles WHERE UserId = '{ADMIN_USER_GUID}' AND RoleId = '{ADMIN_ROLE_GUID}'");

			migrationBuilder.Sql($"DELETE FROM AspNetUsers WHERE Id = '{ADMIN_USER_GUID}'");

			migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id = '{ADMIN_ROLE_GUID}'");

		}
	}
}
