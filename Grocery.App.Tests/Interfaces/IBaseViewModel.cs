using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.App.Tests.Interfaces
{
    public interface IBaseViewModelMock
    {
        public bool EmailValidatie(string email);
        public bool GebruikersnaamValidatie(string gebruikersnaam, List<MockClient> clientList);
        public bool WachtwoordValidatie(string wachtwoord, string wachtwoordBevestiging);
        public void AddNieuwAccountToClientList(List<MockClient> clientList, string email, string gebruikersnaam, string wachtwoord);
    }
}
