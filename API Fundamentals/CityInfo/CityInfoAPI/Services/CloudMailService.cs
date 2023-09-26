﻿using CityInfoAPI.Interfaces;

namespace CityInfoAPI.Services
{
    public class CloudMailService : IMailService
    {
        //private variables must start with underscore
        private readonly string _to = string.Empty;
        private readonly string _from = string.Empty;

        public CloudMailService(IConfiguration configuration)
        {
            _to = configuration["mailSettings:toMail"];
            _from = configuration["mailSettings:fromMail"];
        }

        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail has been sended to {_to} from {_from}, with {nameof(CloudMailService)}" + "\n" + $"Subject:{subject}" + "\n" + $"Content: {message}");
        }
    }
}
