using MarketPlace.Application.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Vendor.Application.Services.interfaces;
using Vendor.Application.Utils;
using Vendor.DataLayer.DTOs.Account;
using Vendor.DataLayer.Entities.Account;
using Vendor.DataLayer.Repository;

namespace Vendor.Application.Services.implementions;

public class UserService : IUserService
{
    #region Ctor

    private readonly IGenericRepository<User> _userRepository;
    private readonly IPasswordHelper _passwordHelper;
    private readonly ISmsService _smsService;

    public UserService(IGenericRepository<User> userRepository, IPasswordHelper passwordHelper, ISmsService smsService)
    {
        _userRepository = userRepository;
        _passwordHelper = passwordHelper;
        _smsService = smsService;
    }

    #endregion

    #region dispose

    public async ValueTask DisposeAsync()
    {
        await _userRepository.DisposeAsync();
    }

    #endregion

    #region account

    public async Task<RegisterUserResult> RegisterUser(RegisterUserDTO registerUser)
    {
        if (!await IsUserExistsByMobileNumber(registerUser.Mobile))
        {
            var user = new User()
            {
                Mobile = registerUser.Mobile,
                FisrtName = registerUser.FisrtName,
                LastName = registerUser.LastName,
                Password = _passwordHelper.EncodePasswordMd5(registerUser.Password),
                MobileActiveCode = new Random().Next(10000, 99999).ToString(),
                EmailActiveCode = Guid.NewGuid().ToString("N"),
            };
            await _userRepository.AddEntity(user);
            await _userRepository.SaveChanges();
            await _smsService.SendVerificationSms(user.Mobile, user.MobileActiveCode);
            return RegisterUserResult.Success;
        }

        return RegisterUserResult.MobileExists;
    }

    public async Task<bool> IsUserExistsByMobileNumber(string mobileNumber)
    {
        return await _userRepository.GetQuery().AnyAsync(u => u.Mobile == mobileNumber);
    }

    public async Task<LoginUserResult> GetUserForLogin(LoginUserDTO loginUser)
    {
        var user = await _userRepository.GetQuery().SingleOrDefaultAsync(u => u.Mobile == loginUser.Mobile);
        if (user == null) return LoginUserResult.NotFound;
        if (!user.IsMobileActive) return LoginUserResult.NotActivated;
        if (user.Password != _passwordHelper.EncodePasswordMd5(loginUser.Password)) return LoginUserResult.NotFound;
        return LoginUserResult.Success;
    }

    public async Task<User> GetUserByMobile(string mobileNumber)
    {
        return await _userRepository.GetQuery().SingleOrDefaultAsync(u => u.Mobile == mobileNumber);
    }

    public async Task<ForgotPasswordResult> RecoveryUserPassword(ForgotPasswordDTO forgotPassword)
    {
        var user = await _userRepository.GetQuery().SingleOrDefaultAsync(u => u.Mobile == forgotPassword.Mobile);
        if (user == null) return ForgotPasswordResult.NotFound;
        var newPassword = new Random().Next(1000000, 9999999).ToString();
        user.Password = _passwordHelper.EncodePasswordMd5(newPassword);
        _userRepository.EditEntity(user);
        await _userRepository.SaveChanges();
        await _smsService.SendVerificationSms(user.Mobile, user.Password);// تمپلیت پترن باید متفاوت باشد چون این برای ارسال پسورد است
        return ForgotPasswordResult.Success;
    }

    public async Task<ActivateMobileResult> ActivateMobile(ActivateMobileDTO activateMobile)
    {
        var user = await _userRepository.GetQuery().SingleOrDefaultAsync(u => u.Mobile == activateMobile.Mobile);
        if (user == null) return ActivateMobileResult.NotFound;
        if (user.MobileActiveCode == activateMobile.MobileActiveCode)
        {
            user.IsMobileActive = true;
            user.MobileActiveCode = new Random().Next(10000, 99999).ToString();
            _userRepository.EditEntity(user);
            await _userRepository.SaveChanges();
            return ActivateMobileResult.Success;
        }

        return ActivateMobileResult.NotMatch;
    }

    public async Task<bool> ChangeUserPassword(ChangePasswordDTO changePass, long currentUserId)
    {
        var user = await _userRepository.GetEntityById(currentUserId);
        if (user != null)
        {
            var newPassword = _passwordHelper.EncodePasswordMd5(changePass.NewPassword);
            if (user.Password != newPassword)
            {
                user.Password = newPassword;
                _userRepository.EditEntity(user);
                await _userRepository.SaveChanges();
                return true;
            }
        }
        return false;
    }

    public async Task<EditUserProfileDTO> GetProfileForEdit(long userId)
    {
        var user = await _userRepository.GetEntityById(userId);
        if (user == null) return null;
        return new EditUserProfileDTO()
        {
            FisrtName = user.FisrtName,
            LastName = user.LastName,
            Avatar = user.Avatar
        };
    }

    public async Task<EditUserProfileResult> EditProfileUser(EditUserProfileDTO editProfile, long userId, IFormFile avatarImage)
    {
        var user = await _userRepository.GetEntityById(userId);
        if (user == null) return EditUserProfileResult.NotFound;
        if (user.IsBlocked) return EditUserProfileResult.IsBlocked;
        if (!user.IsMobileActive) return EditUserProfileResult.IsNotActive;

        user.FisrtName = editProfile.FisrtName;
        user.LastName = editProfile.LastName;

        if (avatarImage != null && avatarImage.IsImage())
        {
            string imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(avatarImage.FileName);

            avatarImage.AddImageToServer(fileName: imageName,
                orginalPath: PathExtentions.UserAvatarOriginServer,
                width: 100,
                height: 100,
                thumbPath: PathExtentions.UserAvatarThumbServer,
                deletefileName: user.Avatar);
            user.Avatar = imageName;
        }

        _userRepository.EditEntity(user);
        await _userRepository.SaveChanges();

        return EditUserProfileResult.Success;
    }

    #endregion

}