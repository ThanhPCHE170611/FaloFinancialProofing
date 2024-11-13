using FALOFinancialProofing.DTOs.CreateQrCodeDTO;
using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using System.Text;

namespace FALOFinancialProofing.Services.CreateQrCodeServices
{
    // get createQrCode by id, get by userId, create createQrCode, update createQrCode
    public class CreateQrCodeService : ICreateQrCodeService
    {
        private readonly IRepository<CreateQrCode, int> _createQrCodeRepository;
        private readonly AuthServices _authServices;
        public CreateQrCodeService(IRepository<CreateQrCode, int> createQrCodeRepository, AuthServices authServices)
        {
            _createQrCodeRepository = createQrCodeRepository;
            _authServices = authServices;
        }

        public async Task<bool> CreateQrCodeAsync(CreateQrCode createQrCode)
        {
            try
            {
                if (createQrCode == null)
                {
                    throw new Exception("createQrCode is null");
                }
                await _createQrCodeRepository.InsertAsync(createQrCode);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CreateQrCodeAsync: {ex.Message}!");
            }

            return false;
        }

        public Task<bool> DeleteCreateQrCodeAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CreateQrCode>> GetAllCreateQrCodesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<CreateQrCode> GetQrCodeByIdAsync(int id)
        {
            CreateQrCode createQrCode = null!;
            try
            {
                createQrCode = await _createQrCodeRepository.Get(id);
                if (createQrCode == null)
                {
                    throw new Exception("CreateQrCode not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetCreateQrCodeById: {ex.Message}");
            }

            return createQrCode;
        }

        public async Task<bool> UpdateCreateQrCodeAsync(CreateQrCode createQrCode)
        {
            CreateQrCode createQrCodeDb = null!;
            bool result = false;
            try
            {
                createQrCodeDb = await _createQrCodeRepository.Get(createQrCode.Id);
                if (createQrCodeDb == null)
                {
                    throw new Exception("CreateQrCode not found!");
                }

                result = await _createQrCodeRepository.UpdateAsync(createQrCode);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateCreateQrCode: {ex.Message}");
            }

            return result;
        }

        public async Task<bool> ValidateQrCodeCreate(CreateQrCodeRequest createQrCodeRequest, StringBuilder message)
        {

            bool IsValid = false;
            try
            {
                bool checkValidUser = await _authServices.CheckUserExist(createQrCodeRequest.UserId, message);
                if (!checkValidUser)
                {
                    return IsValid;
                }

                IsValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"ValidateQrCodeCreate: {ex.Message}");
            }

            return IsValid;
        }
    }
}
