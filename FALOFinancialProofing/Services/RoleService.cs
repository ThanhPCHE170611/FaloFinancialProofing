using FALOFinancialProofing.DTOs.RoleDTOs;
using FALOFinancialProofing.Models;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace FALOFinancialProofing.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        public readonly UserManager<User> userManager;
        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task<IdentityRole> GetRoleByNameAsync(string roleName)
        {
            var role = roleManager.Roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
            return role;
        }

        public async Task<List<RoleInformation>> GetRoleInformationsByUserId(string userId)
        {
            List<RoleInformation> data = new List<RoleInformation>();
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var roles = await userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    IdentityRole identityRole = await roleManager.FindByNameAsync(role);
                    data.Add(new RoleInformation
                    {
                        RoleId = identityRole.Id,
                        RoleName = identityRole.Name
                    });
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetRoleInformationByUserId: {ex.Message}");
            }
            return data;
        }
    }
}
