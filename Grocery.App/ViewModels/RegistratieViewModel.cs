using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Data.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Grocery.App.ViewModels
{
    public partial class RegistratieViewModel : BaseViewModel
    {
        readonly ClientRepository clientRepository = new ClientRepository();
        
        public string EmailAdres { get; set; }
        public string Gebruikersnaam { get; set; }
        public string Wachtwoord { get; set; }
        public string WachtwoordBevestiging { get; set; }

        [ObservableProperty]
        private string emailValidatieFailMessage;

        [ObservableProperty]
        private string gebruikersnaamValidatieFailMessage;

        [ObservableProperty]
        private string wachtwoordValidatieFailMessage;

        [ObservableProperty]
        private string veldenLeegMessage;

        [RelayCommand]
        private void Annuleren()
        {
            Application.Current.MainPage?.Navigation.PopModalAsync();
        }

        private bool EmailValidatie(string email)
        {
            if (email.Contains('@'))
            {
                // Failmessage blijft nu niet staan wanneer er een 
                EmailValidatieFailMessage = string.Empty;
                return true;
            }
            EmailValidatieFailMessage = "Geen geldig e-mailadres ingevuld!";
            return false;
        }
        private bool GebruikersnaamValidatie(string gebruikersnaam, List<Client> clientList)
        {
            foreach (Client client in clientList)
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
        private bool WachtwoordValidatie(string wachtwoord, string wachtwoordBevestiging)
        {
            if (wachtwoord == wachtwoordBevestiging)
            {
                WachtwoordValidatieFailMessage = string.Empty;
                return true;
            }
            WachtwoordValidatieFailMessage = "Wachtwoorden zijn niet hetzelfde!";
            return false;
        }

        private void AddNieuwAccountToClientList(List<Client> clientList)
        {
            int id = clientList.Count() + 1;
            Client client = new Client(id, Gebruikersnaam, EmailAdres, Wachtwoord);
            clientRepository.AddClient(client);
        }

        [RelayCommand]
        private async Task Registratie()
        {
            List<Client> clientList = clientRepository.GetAll();
            bool[] validatieChecks = new bool[3];

            if (EmailAdres == null || Gebruikersnaam == null || Wachtwoord == null || WachtwoordBevestiging == null)
            {
                VeldenLeegMessage = "1 of meerdere velden zijn leeg!";
            }
            else
            {
                bool accountToevoegen = false;

                validatieChecks = [EmailValidatie(EmailAdres), GebruikersnaamValidatie(Gebruikersnaam, clientList), WachtwoordValidatie(Wachtwoord, WachtwoordBevestiging)];

                if (validatieChecks.All(check => check))
                {
                    AddNieuwAccountToClientList(clientList);

                    Application.Current.MainPage?.Navigation.PopModalAsync();

                    Application.Current.MainPage.DisplayAlert("Account aangemaakt", "U kunt nu inloggen met uw account", "OK");
                }
            }
        }
    }
}
