using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Services
{
    public class ValidatieService : IValidatieService
    {
        public string EmailFailMessage { get; set; }
        public string GebruikersnaamFailMessage { get; set; }
        public string WachtwoordFailMessage { get; set; }
        public string veldenLeegMessage { get; set; }

        public bool EmailValidatie(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);
                EmailFailMessage = string.Empty;
                return true;
            }
            catch (Exception e)
            {
                EmailFailMessage = "Geen geldig e-mailadres ingevuld!";
                return false;
            }
        }
        public bool GebruikersnaamValidatie(string gebruikersnaam, List<Client> clientList)
        {
            if (gebruikersnaam.Length < 5)
            {
                GebruikersnaamFailMessage = "Gebruikersnaam bevat minder dan 5 karakters!";
                return false;
            }

            foreach (Client client in clientList)
            {
                if (client.Name == gebruikersnaam)
                {
                    GebruikersnaamFailMessage = "Gebruikersnaam bestaat al!";
                    return false;
                }
            }

            GebruikersnaamFailMessage = string.Empty;
            return true;
        }
        public bool WachtwoordValidatie(string wachtwoord, string wachtwoordBevestiging)
        {
            if (wachtwoord == wachtwoordBevestiging)
            {
                if (wachtwoord.Length < 8)
                {
                    WachtwoordFailMessage = "Wachtwoord bevat minder dan 8 karakters!";
                    return false;
                }
                WachtwoordFailMessage = string.Empty;
                return true;
            }
            WachtwoordFailMessage = "Wachtwoorden zijn niet hetzelfde!";
            return false;
        }

        public bool LegeVeldenValidatie(string email, string gebruikersnaam, string wachtwoord, string wachtwoordBevestiging)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(gebruikersnaam) || string.IsNullOrEmpty(wachtwoord) || string.IsNullOrEmpty(wachtwoordBevestiging))
            {
                veldenLeegMessage = "1 of meerdere velden zijn leeg!";
                return true;
            }
            veldenLeegMessage = string.Empty;
            return false;
        }
    }
}
