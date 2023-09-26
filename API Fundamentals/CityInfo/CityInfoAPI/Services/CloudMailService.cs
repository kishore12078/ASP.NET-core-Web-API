using CityInfoAPI.Interfaces;

namespace CityInfoAPI.Services
{
    public class CloudMailService : IMailService
    {
        //private variables must start with underscore
        private string _to { get; set; } = "kishore1207india@gmail.com";
        private string _from { get; set; } = "kishore1207india@gmail.com";

        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail has been sended to {_to} from {_from}, with {nameof(CloudMailService)}" + "\n" + $"Subject:{subject}" + "\n" + $"Content: {message}");
        }
    }
}
