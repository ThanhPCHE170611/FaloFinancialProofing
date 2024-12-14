using FALOFinancialProofing.Models;
using Microsoft.AspNetCore.Identity;

namespace FALOFinancialProofing.DataSeedings
{
    public class DataSeeding
    {
        public static List<User> GetUsers()
        {
            var hasher = new PasswordHasher<User>();
            string commonPasswordHash = hasher.HashPassword(null, "Falo123!");

            return new List<User>
        {
            new User
            {
                Id = "a1b2c3d4-e5f6-7890-1234-567890abcdef", // Guid tĩnh
                UserName = "admin@falofinancial.com",
                NormalizedUserName = "ADMIN@FALOFINANCIAL.COM",
                Email = "admin@falofinancial.com",
                NormalizedEmail = "ADMIN@FALOFINANCIAL.COM",
                EmailConfirmed = true,
                PasswordHash = commonPasswordHash,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                FirstName = "Nguyễn",
                LastName = "Văn An",
                BirthDate = new DateOnly(1990, 5, 15),
                Gender = true,
                Address = "123 Đường Chính, Thành phố A",
                WorkPlace = "Công ty ABC",
                Bio = "Lập trình viên phần mềm đam mê công nghệ web.",
                Education = "Đại học Bách Khoa, Khoa Công nghệ Thông tin",
                Skill = "C#, .NET, JavaScript",
                Hobby = "Đi bộ đường dài, Nhiếp ảnh",
                Strength = "Giải quyết vấn đề, Làm việc nhóm",
                VolunteerExperience = "Tình nguyện viên tại Mái ấm Tình Thương",
                VolunteerGoal = "Đóng góp cho sự phát triển cộng đồng địa phương."
            },
            new User
            {
                Id = "b2c3d4e5-f678-9012-3456-7890abcdef1", // Guid tĩnh
                UserName = "user1@falofinancial.com",
                NormalizedUserName = "USER1@FALOFINANCIAL.COM",
                Email = "user1@falofinancial.com",
                NormalizedEmail = "USER1@FALOFINANCIAL.COM",
                EmailConfirmed = true,
                PasswordHash = commonPasswordHash,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                FirstName = "Lê",
                LastName = "Thị Bình",
                BirthDate = new DateOnly(1988, 7, 20),
                Gender = false,
                Address = "456 Đường Số 2, Thành phố B",
                WorkPlace = "Công ty XYZ",
                Bio = "Nhân viên văn phòng năng động.",
                Education = "Cao đẳng Kinh tế, Quản trị văn phòng",
                Skill = "Soạn thảo văn bản, Quản lý hồ sơ, Giao tiếp tốt",
                Hobby = "Xem phim, Nghe nhạc",
                Strength = "Cẩn thận, Chu đáo",
                VolunteerGoal = "Tham gia các hoạt động thiện nguyện giúp đỡ cộng đồng."
            },
            new User
            {
                Id = "c3d4e5f6-7890-1234-5678-90abcdef12", // Guid tĩnh
                UserName = "user2@falofinancial.com",
                NormalizedUserName = "USER2@FALOFINANCIAL.COM",
                Email = "user2@falofinancial.com",
                NormalizedEmail = "USER2@FALOFINANCIAL.COM",
                EmailConfirmed = true,
                PasswordHash = commonPasswordHash,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                FirstName = "Cao",
                LastName = "Văn Tuấn",
                BirthDate = new DateOnly(1995, 3, 10),
                Gender = true,
                Address = "789 Đường 30/4, Thành phố CT",
                WorkPlace = "Freelancer",
                Bio = "Lập trình viên tự do, thích khám phá công nghệ mới.",
                Education = "Đại học Cần Thơ, Công nghệ Phần mềm",
                Skill = "PHP, MySQL, Laravel",
                Hobby = "Đọc sách, Chơi thể thao",
                Strength = "Tự học, Sáng tạo",
                VolunteerExperience = "Tham gia dự án mã nguồn mở",
                VolunteerGoal = "Đóng góp cho cộng đồng lập trình viên."
            },
            new User
            {
                Id = "d4e5f678-9012-3456-7890-abcdef123", // Guid tĩnh
                UserName = "user3@falofinancial.com",
                NormalizedUserName = "USER3@FALOFINANCIAL.COM",
                Email = "user3@falofinancial.com",
                NormalizedEmail = "USER3@FALOFINANCIAL.COM",
                EmailConfirmed = true,
                PasswordHash = commonPasswordHash,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                FirstName = "Trần",
                LastName = "Thị Diễm",
                BirthDate = new DateOnly(1992, 9, 25),
                Gender = false,
                Address = "1011 Đường Lê Lợi, Quận 1, TP.HCM",
                WorkPlace = "Ngân hàng ACB",
                Bio = "Thích tham gia các hoạt động xã hội.",
                Education = "Đại học Kinh tế TP.HCM, Tài chính Ngân hàng",
                Skill = "Phân tích tài chính, Tư vấn đầu tư",
                Hobby = "Du lịch, Đọc sách",
                Strength = "Giao tiếp, Thuyết trình",
                VolunteerExperience = "Tình nguyện viên dạy học cho trẻ em nghèo",
                VolunteerGoal = "Góp phần xây dựng một xã hội tốt đẹp hơn."
            },
            new User
            {
                Id = "e5f67890-1234-5678-90ab-cdef12345", // Guid tĩnh
                UserName = "user4@falofinancial.com",
                NormalizedUserName = "USER4@FALOFINANCIAL.COM",
                Email = "user4@falofinancial.com",
                NormalizedEmail = "USER4@FALOFINANCIAL.COM",
                EmailConfirmed = true,
                PasswordHash = commonPasswordHash,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                FirstName = "Phạm",
                LastName = "Văn Hoàng",
                BirthDate = new DateOnly(1985, 12, 5),
                Gender = true,
                Address = "222 Đường Nguyễn Huệ, Quận 3, TP.HCM",
                WorkPlace = "Công ty FPT",
                Bio = "Kỹ sư cầu nối, yêu thích công việc và cuộc sống.",
                Education = "Đại học Giao thông Vận tải, Kỹ thuật Cầu đường",
                Skill = "Thiết kế cầu đường, Quản lý dự án",
                Hobby = "Chơi game, Xem phim",
                Strength = "Chịu khó, Ham học hỏi",
                VolunteerExperience = "Tham gia xây dựng cầu dân sinh",
                VolunteerGoal = "Mang lại niềm vui cho mọi người."
            },
            new User
            {
                Id = "f6789012-3456-7890-abcd-ef1234567", // Guid tĩnh
                UserName = "user5@falofinancial.com",
                NormalizedUserName = "USER5@FALOFINANCIAL.COM",
                Email = "user5@falofinancial.com",
                NormalizedEmail = "USER5@FALOFINANCIAL.COM",
                EmailConfirmed = true,
                PasswordHash = commonPasswordHash,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                FirstName = "Hồ",
                LastName = "Thị Mai",
                BirthDate = new DateOnly(1998, 6, 18),
                Gender = false,
                Address = "333 Đường Pasteur, Quận 1, TP.HCM",
                WorkPlace = "Bệnh viện Chợ Rẫy",
                Bio = "Y tá tận tâm với nghề.",
                Education = "Đại học Y Dược TP.HCM, Điều dưỡng",
                Skill = "Chăm sóc bệnh nhân, Sơ cứu",
                Hobby = "Nấu ăn, Làm bánh",
                Strength = "Kiên nhẫn, Yêu thương con người",
                VolunteerExperience = "Tình nguyện viên tại trạm y tế xã",
                VolunteerGoal = "Giúp đỡ những người bệnh tật."
            },
            new User
            {
                Id = "78901234-5678-90ab-cdef-123456789", // Guid tĩnh
                UserName = "user6@falofinancial.com",
                NormalizedUserName = "USER6@FALOFINANCIAL.COM",
                Email = "user6@falofinancial.com",
                NormalizedEmail = "USER6@FALOFINANCIAL.COM",
                EmailConfirmed = true,
                PasswordHash = commonPasswordHash,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                FirstName = "Võ",
                LastName = "Văn Nam",
                BirthDate = new DateOnly(1987, 4, 8),
                Gender = true,
                Address = "444 Đường Cách Mạng Tháng 8, Quận 10, TP.HCM",
                WorkPlace = "Trường THPT Lê Hồng Phong",
                Bio = "Giáo viên yêu nghề, mong muốn truyền đạt kiến thức cho học sinh.",
                Education = "Đại học Sư phạm TP.HCM, Ngữ văn",
                Skill = "Giảng dạy, Truyền đạt kiến thức",
                Hobby = "Đọc sách, Du lịch",
                Strength = "Nhiệt tình, Trách nhiệm",
                VolunteerExperience = "Tham gia dạy học tình thương",
                VolunteerGoal = "Cống hiến cho sự nghiệp giáo dục."
            },
            new User
            {
                Id = "89012345-6789-abcd-ef12-345678901", // Guid tĩnh
                UserName = "user7@falofinancial.com",
                NormalizedUserName = "USER7@FALOFINANCIAL.COM",
                Email = "user7@falofinancial.com",
                NormalizedEmail = "USER7@FALOFINANCIAL.COM",
                EmailConfirmed = true,
                PasswordHash = commonPasswordHash,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                FirstName = "Đỗ",
                LastName = "Thị Ngọc",
                BirthDate = new DateOnly(1994, 11, 12),
                Gender = false,
                Address = "555 Đường Nguyễn Thị Minh Khai, Quận 3, TP.HCM",
                WorkPlace = "Công ty Thiết kế Thời trang",
                Bio = "Nhà thiết kế thời trang, yêu cái đẹp và sự sáng tạo.",
                Education = "Đại học Mỹ thuật TP.HCM, Thiết kế Thời trang",
                Skill = "Thiết kế, May vá",
                Hobby = "Xem phim, Nghe nhạc",
                Strength = "Sáng tạo, Thẩm mỹ",
                VolunteerExperience = "Tham gia thiết kế trang phục cho chương trình từ thiện",
                VolunteerGoal = "Góp phần làm đẹp cho đời."
            },
            new User
            {
                Id = "90123456-789a-bcde-f123-456789012", // Guid tĩnh
                UserName = "user8@falofinancial.com",
                NormalizedUserName = "USER8@FALOFINANCIAL.COM",
                Email = "user8@falofinancial.com",
                NormalizedEmail = "USER8@FALOFINANCIAL.COM",
                EmailConfirmed = true,
                PasswordHash = commonPasswordHash,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                FirstName = "Lý",
                LastName = "Văn Phong",
                BirthDate = new DateOnly(1991, 8, 22),
                Gender = true,
                Address = "666 Đường Hai Bà Trưng, Quận 1, TP.HCM",
                WorkPlace = "Công ty Chứng khoán",
                Bio = "Chuyên viên phân tích chứng khoán, đam mê thị trường tài chính.",
                Education = "Đại học Ngân hàng TP.HCM, Chứng khoán",
                Skill = "Phân tích chứng khoán, Đầu tư",
                Hobby = "Chơi thể thao, Đọc sách",
                Strength = "Tư duy logic, Phân tích",
                VolunteerExperience = "Tham gia tư vấn tài chính cho người dân",
                VolunteerGoal = "Giúp mọi người hiểu rõ hơn về tài chính."
            },
            new User
            {
                Id = "01234567-89ab-cdef-1234-567890123", // Guid tĩnh
                UserName = "user9@falofinancial.com",
                NormalizedUserName = "USER9@FALOFINANCIAL.COM",
                Email = "user9@falofinancial.com",
                NormalizedEmail = "USER9@FALOFINANCIAL.COM",
                EmailConfirmed = true,
                PasswordHash = commonPasswordHash,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                FirstName = "Trương",
                LastName = "Thị Thu",
                BirthDate = new DateOnly(1993, 2, 14),
                Gender = false,
                Address = "777 Đường Điện Biên Phủ, Quận 3, TP.HCM",
                WorkPlace = "Công ty Luật",
                Bio = "Luật sư, bảo vệ công lý và lẽ phải.",
                Education = "Đại học Luật TP.HCM, Luật",
                Skill = "Tư vấn pháp lý, Soạn thảo văn bản pháp luật",
                Hobby = "Du lịch, Âm nhạc",
                Strength = "Công bằng, Chính trực",
                VolunteerExperience = "Tư vấn pháp lý miễn phí cho người nghèo",
                VolunteerGoal = "Đóng góp cho sự phát triển của pháp luật."
            }
        };
        }
        public static List<UserRole> GetUserRoles()
        {
            return new List<UserRole>
        {
            // admin@falofinancial.com là Admin
            new UserRole { UserId = "a1b2c3d4-e5f6-7890-1234-567890abcdef", RoleId = "15db7f37-5dbc-4035-9b00-a0af4c3fe8bb" },

            // user1@falofinancial.com là PM
            new UserRole { UserId = "b2c3d4e5-f678-9012-3456-7890abcdef1", RoleId = "205d4496-4ac8-40d9-84b9-e09e1ada7a49" },

            // user2@falofinancial.com là PM
            new UserRole { UserId = "c3d4e5f6-7890-1234-5678-90abcdef12", RoleId = "205d4496-4ac8-40d9-84b9-e09e1ada7a49" },

            // user3@falofinancial.com là PMB
            new UserRole { UserId = "d4e5f678-9012-3456-7890-abcdef123", RoleId = "83292e2c-6c86-4153-bdc5-760d05ec2299" },

            // user4@falofinancial.com là PMB
            new UserRole { UserId = "e5f67890-1234-5678-90ab-cdef12345", RoleId = "83292e2c-6c86-4153-bdc5-760d05ec2299" },

            // user5@falofinancial.com là Accounting
            new UserRole { UserId = "f6789012-3456-7890-abcd-ef1234567", RoleId = "83292e2c-6c86-4153-bdc5-760d05ec2293" },

            // user6@falofinancial.com là Accounting
            new UserRole { UserId = "78901234-5678-90ab-cdef-123456789", RoleId = "83292e2c-6c86-4153-bdc5-760d05ec2293" },

            // user7@falofinancial.com là VolunteerLeader
            new UserRole { UserId = "89012345-6789-abcd-ef12-345678901", RoleId = "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472" },

            // user8@falofinancial.com là VolunteerLeader
            new UserRole { UserId = "90123456-789a-bcde-f123-456789012", RoleId = "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472" },

            // user9@falofinancial.com là Volunteer
            new UserRole { UserId = "01234567-89ab-cdef-1234-567890123", RoleId = "83292e2c-6c86-4153-bdc5-760d05ec2295" }
        };
        }
    }
}
