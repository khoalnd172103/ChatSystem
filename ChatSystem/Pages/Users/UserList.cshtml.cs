using BusinessObject;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;

namespace ChatSystem.Pages.Users
{
    public class UserListModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        public IEnumerable<User> Users { get; set; }

        public UserListModel(IUserRepository userRepository)
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
