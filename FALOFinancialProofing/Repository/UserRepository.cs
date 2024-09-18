//using FALOFinancialProofing.DTOs;
//using FALOFinancialProofing.Models;
//using Microsoft.EntityFrameworkCore;

//namespace FALOFinancialProofing.Repository
//{
//    public class UserRepository : IUserRepository
//    {
//        private readonly Repository<User> userRepository;
//        public UserRepository(Repository<User> _userRepository)
//        {
//            this.userRepository = _userRepository;
//        }

//        public async Task<UserDto> GetByEmail(string email)
//        {
//            Get()
//        }

//        public async Task<UserDto> GetUserByEmailAndPassword(string email, string password)
//        {
//            return await _dbContext.Users
//                .Where(u => u.Email == email && u.Password == password)
//                .Select(u => new UserDto
//                {
//                    Id = u.Id,
//                    Email = u.Email,
//                    Password = u.Password,
//                    FullName = u.FullName,
//                    RoleName = u.UserRoles.Select(ur => ur.Role.RoleName).FirstOrDefault()
//                }).SingleOrDefaultAsync();
//        }

//    }
//}
