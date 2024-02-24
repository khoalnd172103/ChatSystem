using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repository;

namespace ChatSystem.Pages.Users
{
    public class FriendListModel : PageModel
    {
        private readonly IFriendRepository _friendRepository;

        public FriendListModel(IFriendRepository friendRepository)
        {
            _friendRepository = friendRepository;
        }

        public List<Friend> Friends { get; set; }

        public async Task<IActionResult> OnGetAsync(string searchTerm, string sortDirection)
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId");
            if (idClaim == null)
            {
                return RedirectToPage("/Account/Login");
            }

            int userId = int.Parse(idClaim.Value);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                Friends = await _friendRepository.SearchFriendsForUserAsync(userId, searchTerm);
            }
            else if (!string.IsNullOrEmpty(sortDirection))
            {   
                bool isAscending = sortDirection != "desc";
                Friends = await _friendRepository.SortByDateAsync(userId, isAscending);
            }
            else
            {
                Friends = await _friendRepository.GetFriendsForUserAsync(userId);
            }

            return Page();
        }
    }
}
