using Grocery.App.Tests.Interfaces;
using Grocery.App.Tests.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.App.Tests
{
    public class TestableRegistratieViewModel : IBaseViewModelMock
    {
        // Implementatie zonder MAUI dependencies voor tests
        private readonly IClientRepositoryMock _clientRepository;

        public TestableRegistratieViewModel(IClientRepositoryMock clientRepository)
        {
            _clientRepository = clientRepository;
        }

        // Kopieer alleen de business logic methods uit je originele ViewModel
        public bool EmailValidatie(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);
                EmailValidatieFailMessage = string.Empty;
                return true;
            }
            catch (Exception e)
            {
                EmailValidatieFailMessage = "Geen geldig e-mailadres ingevuld!";
                return false;
            }
        }
        public bool GebruikersnaamValidatie(string gebruikersnaam, List<MockClient> clientList)
        {
            if (gebruikersnaam.Length < 5)
            {
                GebruikersnaamValidatieFailMessage = "Gebruikersnaam bevat minder dan 5 karakters!";
                return false;
            }

            foreach (MockClient client in clientList)
            {
                if (client.Name == gebruikersnaam)
                {
                    GebruikersnaamValidatieFailMessage = "Gebruikersnaam bestaat al!";
                    return false;
                }
            }

            GebruikersnaamValidatieFailMessage = string.Empty;
            return true;
        }
        public bool WachtwoordValidatie(string wachtwoord, string wachtwoordBevestiging)
        {
            if (wachtwoord == wachtwoordBevestiging)
            {
                if (wachtwoord.Length < 8)
                {
                    WachtwoordValidatieFailMessage = "Wachtwoord bevat minder dan 8 karakters!";
                    return false;
                }
                WachtwoordValidatieFailMessage = string.Empty;
                return true;
            }
            WachtwoordValidatieFailMessage = "Wachtwoorden zijn niet hetzelfde!";
            return false;
        }

        public bool GebruikersnaamMaxLengte(string gebruikersnaam)
        {
            if (gebruikersnaam.Length > 20)
            {
                return false;
            }
            return true;
        }
        public void AddNieuwAccountToClientList(List<MockClient> clientList, string email, string gebruikersnaam, string wachtwoord)
        {
            int id = clientList.Count + 1;
            string hashWachtwoord = PasswordHelperMock.HashPassword(wachtwoord);

            MockClient client = new MockClient(id, gebruikersnaam, email, hashWachtwoord);

            _clientRepository.AddClient(client);
        }

        // Properties voor test berichten
        public string EmailValidatieFailMessage { get; set; }
        public string GebruikersnaamValidatieFailMessage { get; set; }
        public string WachtwoordValidatieFailMessage { get; set; }
        public string VeldenLeegMessage { get; set; }
    }
}
