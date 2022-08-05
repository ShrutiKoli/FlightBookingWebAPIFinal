namespace UserManagementService.Model
{
    public class UserDto
    {

        public string UserName { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string Role { get; set; }
    }
}
