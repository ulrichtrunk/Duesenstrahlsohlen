using Business.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;

namespace Website.Code
{
    public class AppUserStore : IUserStore<AppUser, int>, IUserPasswordStore<AppUser, int>, IUserLockoutStore<AppUser, int>, IUserTwoFactorStore<AppUser, int>
    {
        UserService userService = new UserService();

        /// <summary>
        /// Insert a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task CreateAsync(AppUser user)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task DeleteAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        { }


        /// <summary>
        /// Finds a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<AppUser> FindByIdAsync(int userId)
        {
            var user = userService.GetById(userId);

            return Task.FromResult(new AppUser(user));
        }


        /// <summary>
        /// Find a user by name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Task<AppUser> FindByNameAsync(string userName)
        {
            var user = userService.GetByUserName(userName);

            return Task.FromResult(new AppUser(user));
        }


        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task UpdateAsync(AppUser user)
        {
            throw new NotImplementedException();
        }


        #region PasswordStore

        /// <summary>
        /// Get the user password hash
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(AppUser user)
        {
            string passwordHash = userService.GetPasswordHashById(user.Id);

            return Task.FromResult(passwordHash);
        }


        /// <summary>
        /// Returns true if a user has a password set
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<bool> HasPasswordAsync(AppUser user)
        { 
             throw new NotImplementedException(); 
        }


        /// <summary>
        /// Set the user password hash
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task SetPasswordHashAsync(AppUser user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region UserLockoutStore

        /// <summary>
        /// Returns the current number of failed access attempts.  This number usually will be reset whenever the password is
        /// verified or the account is locked out.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> GetAccessFailedCountAsync(AppUser user)
        {
            return Task.FromResult(0); 
        }


        /// <summary>
        /// Returns whether the user can be locked out.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetLockoutEnabledAsync(AppUser user)
        {
            return Task.FromResult(false);
        }


        /// <summary>
        /// Returns the DateTimeOffset that represents the end of a user's lockout, any time in the past should be considered
        /// not locked out.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(AppUser user)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Used to record when an attempt to access the user has failed
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<int> IncrementAccessFailedCountAsync(AppUser user)
        { 
            throw new NotImplementedException(); 
        }


        /// <summary>
        /// Used to reset the access failed count, typically after the account is successfully accessed
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task ResetAccessFailedCountAsync(AppUser user)
        { 
            throw new NotImplementedException(); 
        }


        /// <summary>
        /// Sets whether the user can be locked out.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task SetLockoutEnabledAsync(AppUser user, bool enabled)
        { 
            throw new NotImplementedException(); 
        }


        /// <summary>
        /// Locks a user out until the specified end date (set to a past date, to unlock a user)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="lockoutEnd"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task SetLockoutEndDateAsync(AppUser user, DateTimeOffset lockoutEnd)
        { 
            throw new NotImplementedException(); 
        }

        #endregion

        /// <summary>
        /// Sets whether two factor authentication is enabled for the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task SetTwoFactorEnabledAsync(AppUser user, bool enabled)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns whether two factor authentication is enabled for the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetTwoFactorEnabledAsync(AppUser user)
        {
            return Task.FromResult(false);
        }
    }
}