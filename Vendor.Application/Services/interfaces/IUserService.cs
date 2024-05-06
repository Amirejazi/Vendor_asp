using Microsoft.AspNetCore.Http;
using Vendor.DataLayer.DTOs.Account;
using Vendor.DataLayer.Entities.Account;

namespace Vendor.Application.Services.interfaces;

public interface IUserService: IAsyncDisposable
{
    #region account

    Task<RegisterUserResult> RegisterUser(RegisterUserDTO registerUser);
    Task<bool> IsUserExistsByMobileNumber(string mobileNumber);
    Task<LoginUserResult> GetUserForLogin(LoginUserDTO  loginUser);
    Task<User> GetUserByMobile(string mobileNumber);
    Task<ForgotPasswordResult> RecoveryUserPassword(ForgotPasswordDTO forgotPassword);
    Task<ActivateMobileResult> ActivateMobile(ActivateMobileDTO activateMobile);
    Task<bool> ChangeUserPassword(ChangePasswordDTO  changePass, long currentUserId);
    Task<EditUserProfileDTO> GetProfileForEdit(long userId);
    Task<EditUserProfileResult> EditProfileUser(EditUserProfileDTO editProfile, long userId, IFormFile avatarImage);

    #endregion
}