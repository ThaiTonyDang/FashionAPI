namespace Domain.Dtos.Users
{
    public class UserPasswordChangeDto
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }

        public bool IsPasswordValidated()
        {
            if(string.IsNullOrEmpty(CurrentPassword) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmPassword))
            {
                return false;
            }

            return NewPassword == ConfirmPassword;
        }
    }
}
