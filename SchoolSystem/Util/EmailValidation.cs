using System.Text.RegularExpressions;

namespace SchoolSystem.Util
{
    public class EmailValidation
    {
        public bool IsValid(string email)
        {
            Regex Email = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return Email.IsMatch(email);
        }
    }
}
