using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class seedUserandUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Address", "Bio", "BirthDate", "ConcurrencyStamp", "Education", "Email", "EmailConfirmed", "FirstName", "Gender", "Hobby", "Image", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Skill", "Strength", "TwoFactorEnabled", "UserName", "VolunteerExperience", "VolunteerGoal", "WorkPlace" },
                values: new object[,]
                {
                    { "01234567-89ab-cdef-1234-567890123", 0, "777 Đường Điện Biên Phủ, Quận 3, TP.HCM", "Luật sư, bảo vệ công lý và lẽ phải.", new DateOnly(1993, 2, 14), "c6ef6602-b083-4d3c-a55b-fe80cedde872", "Đại học Luật TP.HCM, Luật", "user9@falofinancial.com", true, "Trương", false, "Du lịch, Âm nhạc", null, "Thị Thu", false, null, "USER9@FALOFINANCIAL.COM", "USER9@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAEAv5Ft/tuDcLhj/RXPpqHRsFhy0VLijVazfvQQVxP0kAk0apAye3AowK++jZi7YWYA==", null, false, "fc039a92-e9c2-4306-92a7-2b7bb12a2b3a", "Tư vấn pháp lý, Soạn thảo văn bản pháp luật", "Công bằng, Chính trực", false, "user9@falofinancial.com", "Tư vấn pháp lý miễn phí cho người nghèo", "Đóng góp cho sự phát triển của pháp luật.", "Công ty Luật" },
                    { "78901234-5678-90ab-cdef-123456789", 0, "444 Đường Cách Mạng Tháng 8, Quận 10, TP.HCM", "Giáo viên yêu nghề, mong muốn truyền đạt kiến thức cho học sinh.", new DateOnly(1987, 4, 8), "fc9b59a3-9d1f-4882-8af8-5621abc7e4da", "Đại học Sư phạm TP.HCM, Ngữ văn", "user6@falofinancial.com", true, "Võ", true, "Đọc sách, Du lịch", null, "Văn Nam", false, null, "USER6@FALOFINANCIAL.COM", "USER6@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAEAv5Ft/tuDcLhj/RXPpqHRsFhy0VLijVazfvQQVxP0kAk0apAye3AowK++jZi7YWYA==", null, false, "aadba03d-b144-4cda-bda0-ca7361089881", "Giảng dạy, Truyền đạt kiến thức", "Nhiệt tình, Trách nhiệm", false, "user6@falofinancial.com", "Tham gia dạy học tình thương", "Cống hiến cho sự nghiệp giáo dục.", "Trường THPT Lê Hồng Phong" },
                    { "89012345-6789-abcd-ef12-345678901", 0, "555 Đường Nguyễn Thị Minh Khai, Quận 3, TP.HCM", "Nhà thiết kế thời trang, yêu cái đẹp và sự sáng tạo.", new DateOnly(1994, 11, 12), "a02e5b93-183f-4753-998f-1df9c4f50acb", "Đại học Mỹ thuật TP.HCM, Thiết kế Thời trang", "user7@falofinancial.com", true, "Đỗ", false, "Xem phim, Nghe nhạc", null, "Thị Ngọc", false, null, "USER7@FALOFINANCIAL.COM", "USER7@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAEAv5Ft/tuDcLhj/RXPpqHRsFhy0VLijVazfvQQVxP0kAk0apAye3AowK++jZi7YWYA==", null, false, "d525259b-01c3-4355-a556-68c9198360e8", "Thiết kế, May vá", "Sáng tạo, Thẩm mỹ", false, "user7@falofinancial.com", "Tham gia thiết kế trang phục cho chương trình từ thiện", "Góp phần làm đẹp cho đời.", "Công ty Thiết kế Thời trang" },
                    { "90123456-789a-bcde-f123-456789012", 0, "666 Đường Hai Bà Trưng, Quận 1, TP.HCM", "Chuyên viên phân tích chứng khoán, đam mê thị trường tài chính.", new DateOnly(1991, 8, 22), "e4c13025-1809-4bbc-8cc8-0fbeed1c5496", "Đại học Ngân hàng TP.HCM, Chứng khoán", "user8@falofinancial.com", true, "Lý", true, "Chơi thể thao, Đọc sách", null, "Văn Phong", false, null, "USER8@FALOFINANCIAL.COM", "USER8@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAEAv5Ft/tuDcLhj/RXPpqHRsFhy0VLijVazfvQQVxP0kAk0apAye3AowK++jZi7YWYA==", null, false, "43380827-e1a6-4710-a229-c34f5f3ed721", "Phân tích chứng khoán, Đầu tư", "Tư duy logic, Phân tích", false, "user8@falofinancial.com", "Tham gia tư vấn tài chính cho người dân", "Giúp mọi người hiểu rõ hơn về tài chính.", "Công ty Chứng khoán" },
                    { "a1b2c3d4-e5f6-7890-1234-567890abcdef", 0, "123 Đường Chính, Thành phố A", "Lập trình viên phần mềm đam mê công nghệ web.", new DateOnly(1990, 5, 15), "dbe4e9d1-be15-42ea-914d-dc7ed1237e63", "Đại học Bách Khoa, Khoa Công nghệ Thông tin", "admin@falofinancial.com", true, "Nguyễn", true, "Đi bộ đường dài, Nhiếp ảnh", null, "Văn An", false, null, "ADMIN@FALOFINANCIAL.COM", "ADMIN@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAEAv5Ft/tuDcLhj/RXPpqHRsFhy0VLijVazfvQQVxP0kAk0apAye3AowK++jZi7YWYA==", null, false, "da2b19ae-f42f-491b-b370-e67f25d16f21", "C#, .NET, JavaScript", "Giải quyết vấn đề, Làm việc nhóm", false, "admin@falofinancial.com", "Tình nguyện viên tại Mái ấm Tình Thương", "Đóng góp cho sự phát triển cộng đồng địa phương.", "Công ty ABC" },
                    { "b2c3d4e5-f678-9012-3456-7890abcdef1", 0, "456 Đường Số 2, Thành phố B", "Nhân viên văn phòng năng động.", new DateOnly(1988, 7, 20), "5c3a7381-7b9c-41b5-ab6c-fbb67f2550ca", "Cao đẳng Kinh tế, Quản trị văn phòng", "user1@falofinancial.com", true, "Lê", false, "Xem phim, Nghe nhạc", null, "Thị Bình", false, null, "USER1@FALOFINANCIAL.COM", "USER1@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAEAv5Ft/tuDcLhj/RXPpqHRsFhy0VLijVazfvQQVxP0kAk0apAye3AowK++jZi7YWYA==", null, false, "8afa6bed-641d-41ef-8f52-5ad51b2df57e", "Soạn thảo văn bản, Quản lý hồ sơ, Giao tiếp tốt", "Cẩn thận, Chu đáo", false, "user1@falofinancial.com", null, "Tham gia các hoạt động thiện nguyện giúp đỡ cộng đồng.", "Công ty XYZ" },
                    { "c3d4e5f6-7890-1234-5678-90abcdef12", 0, "789 Đường 30/4, Thành phố CT", "Lập trình viên tự do, thích khám phá công nghệ mới.", new DateOnly(1995, 3, 10), "d1764107-dda4-4540-912e-aac030b105ae", "Đại học Cần Thơ, Công nghệ Phần mềm", "user2@falofinancial.com", true, "Cao", true, "Đọc sách, Chơi thể thao", null, "Văn Tuấn", false, null, "USER2@FALOFINANCIAL.COM", "USER2@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAEAv5Ft/tuDcLhj/RXPpqHRsFhy0VLijVazfvQQVxP0kAk0apAye3AowK++jZi7YWYA==", null, false, "0dab950b-c548-42af-9913-10edc18ca4ab", "PHP, MySQL, Laravel", "Tự học, Sáng tạo", false, "user2@falofinancial.com", "Tham gia dự án mã nguồn mở", "Đóng góp cho cộng đồng lập trình viên.", "Freelancer" },
                    { "d4e5f678-9012-3456-7890-abcdef123", 0, "1011 Đường Lê Lợi, Quận 1, TP.HCM", "Thích tham gia các hoạt động xã hội.", new DateOnly(1992, 9, 25), "1e853b2b-e5f5-4072-a7f9-4d86d42b1701", "Đại học Kinh tế TP.HCM, Tài chính Ngân hàng", "user3@falofinancial.com", true, "Trần", false, "Du lịch, Đọc sách", null, "Thị Diễm", false, null, "USER3@FALOFINANCIAL.COM", "USER3@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAEAv5Ft/tuDcLhj/RXPpqHRsFhy0VLijVazfvQQVxP0kAk0apAye3AowK++jZi7YWYA==", null, false, "b83d9399-5810-4201-9440-b8d86bf12a46", "Phân tích tài chính, Tư vấn đầu tư", "Giao tiếp, Thuyết trình", false, "user3@falofinancial.com", "Tình nguyện viên dạy học cho trẻ em nghèo", "Góp phần xây dựng một xã hội tốt đẹp hơn.", "Ngân hàng ACB" },
                    { "e5f67890-1234-5678-90ab-cdef12345", 0, "222 Đường Nguyễn Huệ, Quận 3, TP.HCM", "Kỹ sư cầu nối, yêu thích công việc và cuộc sống.", new DateOnly(1985, 12, 5), "3ef90099-510e-4556-affa-3b52baefe71d", "Đại học Giao thông Vận tải, Kỹ thuật Cầu đường", "user4@falofinancial.com", true, "Phạm", true, "Chơi game, Xem phim", null, "Văn Hoàng", false, null, "USER4@FALOFINANCIAL.COM", "USER4@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAEAv5Ft/tuDcLhj/RXPpqHRsFhy0VLijVazfvQQVxP0kAk0apAye3AowK++jZi7YWYA==", null, false, "84cd4b0b-eb54-43d4-89e0-e133d885530d", "Thiết kế cầu đường, Quản lý dự án", "Chịu khó, Ham học hỏi", false, "user4@falofinancial.com", "Tham gia xây dựng cầu dân sinh", "Mang lại niềm vui cho mọi người.", "Công ty FPT" },
                    { "f6789012-3456-7890-abcd-ef1234567", 0, "333 Đường Pasteur, Quận 1, TP.HCM", "Y tá tận tâm với nghề.", new DateOnly(1998, 6, 18), "6f951b75-368f-4d63-aa75-34c2a43e30c8", "Đại học Y Dược TP.HCM, Điều dưỡng", "user5@falofinancial.com", true, "Hồ", false, "Nấu ăn, Làm bánh", null, "Thị Mai", false, null, "USER5@FALOFINANCIAL.COM", "USER5@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAEAv5Ft/tuDcLhj/RXPpqHRsFhy0VLijVazfvQQVxP0kAk0apAye3AowK++jZi7YWYA==", null, false, "3bf3f2d0-4e6a-401d-b7d3-679d6c0b9e36", "Chăm sóc bệnh nhân, Sơ cứu", "Kiên nhẫn, Yêu thương con người", false, "user5@falofinancial.com", "Tình nguyện viên tại trạm y tế xã", "Giúp đỡ những người bệnh tật.", "Bệnh viện Chợ Rẫy" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "83292e2c-6c86-4153-bdc5-760d05ec2295", "01234567-89ab-cdef-1234-567890123" },
                    { "83292e2c-6c86-4153-bdc5-760d05ec2293", "78901234-5678-90ab-cdef-123456789" },
                    { "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472", "89012345-6789-abcd-ef12-345678901" },
                    { "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472", "90123456-789a-bcde-f123-456789012" },
                    { "15db7f37-5dbc-4035-9b00-a0af4c3fe8bb", "a1b2c3d4-e5f6-7890-1234-567890abcdef" },
                    { "205d4496-4ac8-40d9-84b9-e09e1ada7a49", "b2c3d4e5-f678-9012-3456-7890abcdef1" },
                    { "205d4496-4ac8-40d9-84b9-e09e1ada7a49", "c3d4e5f6-7890-1234-5678-90abcdef12" },
                    { "83292e2c-6c86-4153-bdc5-760d05ec2299", "d4e5f678-9012-3456-7890-abcdef123" },
                    { "83292e2c-6c86-4153-bdc5-760d05ec2299", "e5f67890-1234-5678-90ab-cdef12345" },
                    { "83292e2c-6c86-4153-bdc5-760d05ec2293", "f6789012-3456-7890-abcd-ef1234567" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "83292e2c-6c86-4153-bdc5-760d05ec2295", "01234567-89ab-cdef-1234-567890123" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "83292e2c-6c86-4153-bdc5-760d05ec2293", "78901234-5678-90ab-cdef-123456789" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472", "89012345-6789-abcd-ef12-345678901" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472", "90123456-789a-bcde-f123-456789012" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "15db7f37-5dbc-4035-9b00-a0af4c3fe8bb", "a1b2c3d4-e5f6-7890-1234-567890abcdef" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "205d4496-4ac8-40d9-84b9-e09e1ada7a49", "b2c3d4e5-f678-9012-3456-7890abcdef1" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "205d4496-4ac8-40d9-84b9-e09e1ada7a49", "c3d4e5f6-7890-1234-5678-90abcdef12" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "83292e2c-6c86-4153-bdc5-760d05ec2299", "d4e5f678-9012-3456-7890-abcdef123" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "83292e2c-6c86-4153-bdc5-760d05ec2299", "e5f67890-1234-5678-90ab-cdef12345" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "83292e2c-6c86-4153-bdc5-760d05ec2293", "f6789012-3456-7890-abcd-ef1234567" });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01234567-89ab-cdef-1234-567890123");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "78901234-5678-90ab-cdef-123456789");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "89012345-6789-abcd-ef12-345678901");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "90123456-789a-bcde-f123-456789012");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-7890-1234-567890abcdef");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "b2c3d4e5-f678-9012-3456-7890abcdef1");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "c3d4e5f6-7890-1234-5678-90abcdef12");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "d4e5f678-9012-3456-7890-abcdef123");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "e5f67890-1234-5678-90ab-cdef12345");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "f6789012-3456-7890-abcd-ef1234567");
        }
    }
}
