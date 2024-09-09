using System.Text.RegularExpressions;

namespace SchoolSystem.Util
{
    public class PhoneValidation
    {
        public bool IsValid(string phone)
        {
            Regex EgyptPhone = new Regex(@"^01[0-2]|[5]{1}[0-9]{8}$");
            return EgyptPhone.IsMatch(phone);
        }
    }
}
