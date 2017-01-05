using ETravel.BAL.Models;
using ETravel.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ETravel.BAL.Services
{
    public class UserService : IDisposable
	{
		protected IUnitOfWork uow;

        private readonly Func<User, UserModel> _userModelSelector;

		public UserService(IUnitOfWork _uow)
		{
			if (_uow == null)
				uow = new UnitOfWork();
			else
				uow = _uow;

            _userModelSelector = e => new UserModel()
            {
                Id = e.Id,
                Username = e.Username,
                CreatedDateTime = e.CreatedDateTime,
                UpdatedDateTime = e.UpdatedDateTime,
                Name = e.Name,
                AttachmentSetId = e.AttachmentSetId,
                ShortBio = e.ShortBio,
                AvatarImage = e.AvatarImage
            };
        }

		// OK
		public IList<UserModel> GetAllUsers()
		{
			return uow
                    .UserRepository
                    .All()?
					.Select(_userModelSelector)
                    .OrderByDescending(e => e.UpdatedDateTime)
                    .ToList();
		}

		// OK
		public IList<UserModel> GetLastTenRegisteredUsers()
		{
			
			return uow
                    .UserRepository
                    .All()?
					.Select(_userModelSelector)
                    .OrderByDescending(e => e.CreatedDateTime)
                    .Take(10)
                    .ToList();
		}

		// OK
		public UserModel GetUser(int userId)
		{
            try
			{
				return uow
                        .UserRepository
						.SearchFor(e => e.Id == userId)
						.Select(_userModelSelector)
                        .SingleOrDefault();
			}
			catch (InvalidOperationException ex)
			{
				throw new InvalidOperationException("User lookup failed", ex);
			}
		}

		// OK
		public UserModel GetUserByUsername(string userName)
		{
			try
			{
				return uow
                        .UserRepository
						.SearchFor(e => e.Username == userName)
						.Select(_userModelSelector)
                        .SingleOrDefault();
			}
			catch (InvalidOperationException ex)
			{
				throw new InvalidOperationException("User lookup failed", ex);
			}
		}

		// OK
		public void CreateUser(UserModel source)
		{
			try
			{
                //1st Step - Create New AttachmentSet to be given to the new user
                using (var us = new AttachmentSetService(uow))
                {
                    source.AttachmentSetId = us.CreateAttachmentSet();
                }

                //2nd Step - Create the user
				var _user = new User()
				{
					Username = source.Username,
					CreatedDateTime = DateTime.Now,
					UpdatedDateTime = DateTime.Now,
					Name = source.Name,
                    AttachmentSetId = source.AttachmentSetId,
                    ShortBio = "",
					AvatarImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSODALYDYo2dqN0DG_kPNi2X7EAy1K8SpRRZQWkNv9alC62IHggOw"
				};

				uow.UserRepository.Insert(_user, true);
			}
			catch (Exception)
			{
				throw;
			}
		}

		// OK
		public bool UpdateUser(UserModel source, ClaimsIdentity identity)
		{
			try
			{
				User _user;
					
				try
				{
					_user = uow.UserRepository
							   .SearchFor(e => e.Username == identity.Name)
							   .SingleOrDefault();
				}
				catch (InvalidOperationException ex)
				{
					throw new InvalidOperationException("User lookup for requestor Id failed", ex);
				}

				if (_user == null) return false; 
				
				//update user from UserModel
				_user.UpdatedDateTime = DateTime.Now;
				_user.Name = source.Name;
				_user.ShortBio = source.ShortBio;

                if (source.AvatarImage == "")
                    _user.AvatarImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSODALYDYo2dqN0DG_kPNi2X7EAy1K8SpRRZQWkNv9alC62IHggOw";
                else
                    _user.AvatarImage = source.AvatarImage;

				uow.UserRepository.Update(_user, true);
				
				return true;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public IList<UserModel> GetByName(string searchTerm)
		{
			return uow.UserRepository
					  .SearchFor(e => e.Name.Contains(searchTerm))
					  .Select(_userModelSelector)
                      .OrderByDescending(e => e.CreatedDateTime)
                      .ToList();

		}
        
		public void Dispose() {}
	}
}
