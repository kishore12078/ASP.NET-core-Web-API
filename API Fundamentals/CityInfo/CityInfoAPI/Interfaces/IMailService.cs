namespace CityInfoAPI.Interfaces
{
    public interface IMailService
    {
        public void Send(string subject,string message);
    }
}
