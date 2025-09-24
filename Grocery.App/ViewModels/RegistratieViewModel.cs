using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Data.Repositories;
using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Grocery.App.ViewModels
{
    public partial class RegistratieViewModel : BaseViewModel
    {
        private readonly IClientRepository _clientRepository;
        private readonly IValidatieService _validatieService;
        public RegistratieViewModel(IClientRepository clientRepository, IValidatieService validatieService)
        {
            _clientRepository = clientRepository;
            _validatieService = validatieService;
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

            bool veldenLeeg = _validatieService.LegeVeldenValidatie(EmailAdres, Gebruikersnaam, Wachtwoord, WachtwoordBevestiging);

            if (veldenLeeg)
            {
                VeldenLeegMessage = _validatieService.veldenLeegMessage;
            }
            else
            {
                VeldenLeegMessage = _validatieService.veldenLeegMessage;

                bool emailResult = _validatieService.EmailValidatie(EmailAdres);
                EmailValidatieFailMessage = _validatieService.EmailFailMessage;

                bool gebruikersnaamResult = _validatieService.GebruikersnaamValidatie(Gebruikersnaam, clientList);
                GebruikersnaamValidatieFailMessage = _validatieService.GebruikersnaamFailMessage;

                bool wachtwoordResult = _validatieService.WachtwoordValidatie(Wachtwoord, WachtwoordBevestiging);
                WachtwoordValidatieFailMessage = _validatieService.WachtwoordFailMessage;

                validatieChecks = [emailResult, gebruikersnaamResult, wachtwoordResult];

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
