using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;

namespace PRN221ProjectGroup.Views.Users
{
    public class UsersModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        public IEnumerable<User> Users { get; set; }

        public UsersModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            Users = new List<User>();
        }

        public void OnGet()
        {
            Users = _userRepository.GetUsers();
        }
    }
}
