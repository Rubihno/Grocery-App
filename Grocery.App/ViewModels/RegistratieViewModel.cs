using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Data.Repositories;
using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Grocery.App.ViewModels
{
    public partial class RegistratieViewModel : BaseViewModel
    {
        private readonly IClientRepository _clientRepository;
        public RegistratieViewModel(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public string EmailAdres { get; set; }
        public string Gebruikersnaam { get; set; }
        public string Wachtwoord { get; set; }
        public string WachtwoordBevestiging { get; set; }

        [ObservableProperty]
        public bool isPassword = true;

        [ObservableProperty]
        private string emailValidatieFailMessage;

        [ObservableProperty]
        private string gebruikersnaamValidatieFailMessage;

        [ObservableProperty]
        private string wachtwoordValidatieFailMessage;

        [ObservableProperty]
        private string veldenLeegMessage;

        [RelayCommand]
        private async void Annuleren()
        {
            bool annuleren = await Application.Current.MainPage?.DisplayAlert("Registratie annuleren?", string.Empty, "Ja", "Nee");
            if (annuleren)
            {
                Application.Current.MainPage?.Navigation.PopModalAsync();
            }
        }

        private bool EmailValidatie(string email)
        {
            if (email.Contains('@'))
            {
                // Failmessage wordt nu niet weergegeven bij een 2e poging waar email correct is en een ander veld niet
                EmailValidatieFailMessage = string.Empty;
                return true;
            }
            EmailValidatieFailMessage = "Geen geldig e-mailadres ingevuld!";
            return false;
        }
        private bool GebruikersnaamValidatie(string gebruikersnaam, List<Client> clientList)
        {
            if (gebruikersnaam.Length < 5)
            {
                gebruikersnaamValidatieFailMessage = "Gebruikersnaam bevat minder dan 5 karakters!";
                return false;
            }

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

        private void AddNieuwAccountToClientList(List<Client> clientList)
        {
            int id = clientList.Count + 1;
            string hashWachtwoord = PasswordHelper.HashPassword(Wachtwoord);

            Client client = new Client(id, Gebruikersnaam, EmailAdres, hashWachtwoord);

            _clientRepository.AddClient(client);
        }

        [RelayCommand]
        private async Task Registratie()
        {
            List<Client> clientList = _clientRepository.GetAll();
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

                    Application.Current.MainPage?.DisplayAlert("Account aangemaakt", "U kunt nu inloggen met uw account", "OK");
                }
            }
        }
    }
}
