namespace LibraryApi.Validation
{
    public class PasswordValidation
    {
        public static bool ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;
   
            bool lower = password.Any(char.IsLower);
            bool upper = password.Any(char.IsUpper);
            bool number = password.Any(char.IsDigit);
            bool specialcharacters = password.Any(ch => !char.IsLetterOrDigit(ch));

            return lower && upper && number && specialcharacters; 
        }
    }
}
