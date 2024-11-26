using Pomodoro.Shared.Responses;

namespace Pomodoro.API.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string toName, string toEmail, string subject, string body);

    }
}
