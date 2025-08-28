namespace App.Web.ViewModels.Account
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
    }

    public class UserListViewModel
    {
        public required List<UserViewModel> Users { get; set; }
    }
}
