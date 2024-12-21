namespace TwilioAIIntegration;

public class TwilioMessage
{
    public string From { get; set; }
    public string To { get; set; }
    public string Body { get; set; }
    public string MessageSid { get; set; }
    public string AccountSid { get; set; }
}