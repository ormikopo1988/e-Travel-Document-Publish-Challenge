using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;
using ETravel.AuthServer.Authorization.Web.Api.Models;

namespace ETravel.AuthServer.Authorization.Web.Api
{
    public class AuthRepository :IDisposable
    {

        protected AuthContext _context;
        protected UserManager<IdentityUser> _userManager;


        public AuthRepository()
        {
            try
            {
                _context = new AuthContext();
                _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_context));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IdentityUser> FindUser(string username , string password)
        {
            try
            {
                return await _userManager.FindAsync(username, password);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            try
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = userModel.UserName
                };
            
                var result = await _userManager.CreateAsync(user, userModel.Password);

                if (result.Succeeded) { }

                return result;
            }
            catch(Exception)
            {
                throw;
            }
            
        }

        public async Task<IdentityUser> FindUserById(int id)
        {
            try
            {
                return await _userManager.FindByIdAsync(id.ToString());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Client FindClient(string clientId)
        {
            try
            {
                var client = _context.Clients.Find(clientId);

                return client;
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        public void Dispose()
        {
            _context.Dispose();
            _userManager.Dispose();
        }
    }
}