using System;
using MusicFest.repository;

namespace MusicFest.business
{
    public class UserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool validateUser(String username, String password)
        {
            return _userRepository.FindOne(username, password);
        }
    }
}