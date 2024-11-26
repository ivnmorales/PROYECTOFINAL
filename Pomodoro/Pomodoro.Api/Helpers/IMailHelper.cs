using Azure;
using Pomodoro.Shared.Responses;

namespace Pomodoro.API.Helpers
{
    public interface IMailHelper

    {
        Response<string> SendMail(string toName, string toEmail, string subject, string body);

    }
}
