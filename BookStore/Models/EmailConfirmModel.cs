namespace BookStore.Models
{
    public class EmailConfirmModel
    {
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool EmailSent { get; set; }
        public bool EmailVerify { get; set; }
    }
}
