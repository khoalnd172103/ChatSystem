using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Repository.DTOs;

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
        public PaginatedList<FriendListDto> FriendLists { get; set; }

        public async Task<IActionResult> OnGetAsync(string searchTerm, string sortDirection, int? pageIndex)
        {
            const int pageSize = 2;

            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId");
            if (idClaim == null)
            {
                return RedirectToPage("/Account/Login");
            }

            int userId = int.Parse(idClaim.Value);
            HttpContext.Session.SetInt32("UserId", userId);

            if(searchTerm != null)
            {
                pageIndex = 1; 
            }

            //if (!string.IsNullOrEmpty(sortDirection))
            //{
            //    bool isAscending = sortDirection != "desc";
            //    Friends = await _friendRepository.SortByDateAsync(userId, isAscending);
            //}

            FriendLists = _friendRepository.GetFriendListForUser(searchTerm, pageIndex ?? 1, pageSize, userId);
            

            return Page();
        }
    }
}
