namespace TwilioAIIntegration
{
    public static class PhoneNumberHelper
    {
        public static string NormalizePhoneNumber(string phoneNumber)
        {
            var trimmedPhoneNumber = phoneNumber.Trim();
            if (!trimmedPhoneNumber.StartsWith("+"))
            {
                trimmedPhoneNumber = $"+{trimmedPhoneNumber.TrimStart()}";
            }
            return trimmedPhoneNumber;
        }
    }
}
