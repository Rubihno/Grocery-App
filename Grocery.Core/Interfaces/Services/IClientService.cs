using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Services
{
    public interface IClientService
    {
        public Client? Get(string email);

        public Client? Get(int id);

        public List<Client> GetAll();

        public void AddNieuwAccountToClientList(int id, string gebruikersnaam, string email, string wachtwoord);
    }
}
