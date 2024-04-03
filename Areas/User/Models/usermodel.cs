namespace Hospital_Management.Areas.User.Models
{
    public class usermodel
    {
        public int UserID { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public bool Status { get; set; }

        public int RoleId { get; set; }
    }
}
