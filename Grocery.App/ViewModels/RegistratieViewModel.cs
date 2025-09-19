using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Data.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Diagnostics;

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
        private string wachtwoordtValidatieFailMessage;

        [ObservableProperty]
        private string veldenLeegMessage;

        [RelayCommand]
        private void Annuleren()
        {
            Application.Current.MainPage?.Navigation.PopModalAsync();
        }

        private bool EmailValidatie(string email)
        {
            if (email.Contains('@')) return true;

            return false;
        }
        private bool GebruikersnaamValidatie(string gebruikersnaam, List<Client> clientList)
        {
            foreach (Client client in clientList)
            {
                if (client.Name == gebruikersnaam) return false;
            }
            return true;
        }
        private bool WachtwoordValidatie(string wachtwoord, string wachtwoordBevestiging)
        {
            if (wachtwoord == wachtwoordBevestiging) return true;

            return false;
        }

        private void AddNieuwAccountToClientList(List<Client> clientList)
        {
            int id = clientList.Count() + 1;
            Client client = new Client(id, Gebruikersnaam, EmailAdres, Wachtwoord);
            clientRepository.AddClient(client);
        }

        [RelayCommand]
        private void Registratie()
        {
            List<Client> clientList = clientRepository.GetAll();
            bool[] validatieChecks = new bool[3];

            if (EmailAdres == string.Empty || Gebruikersnaam == string.Empty || Wachtwoord == string.Empty || WachtwoordBevestiging == string.Empty)
            {
                Debug.WriteLine("1 of meerdere velden zijn leeg!");
            }
            else
            {
                validatieChecks = [EmailValidatie(EmailAdres), GebruikersnaamValidatie(Gebruikersnaam, clientList), WachtwoordValidatie(Wachtwoord, WachtwoordBevestiging)];

                if (validatieChecks.All(check => check))
                {
                    AddNieuwAccountToClientList(clientList);

                    // Terug naar loginscherm

                    // Melding dat account is aangemaakt
                }
            }
        }
    }
}
