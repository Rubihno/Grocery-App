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
    public class ValidationService : IValidationService
    {
        public string EmailFailMessage { get; set; }
        public string NameFailMessage { get; set; }
        public string PasswordFailMessage { get; set; }
        public string emptyFieldMessage { get; set; }

        public bool EmailValidation(string email)
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
        public bool NameValidation(string name, List<Client> clientList)
        {
            if (name.Length < 5)
            {
                NameFailMessage = "Gebruikersnaam bevat minder dan 5 karakters!";
                return false;
            }

            foreach (Client client in clientList)
            {
                if (client.Name == name)
                {
                    NameFailMessage = "Gebruikersnaam bestaat al!";
                    return false;
                }
            }

            NameFailMessage = string.Empty;
            return true;
        }
        public bool PasswordValidation(string password, string passwordConfirmation)
        {
            if (password == passwordConfirmation)
            {
                if (password.Length < 8)
                {
                    PasswordFailMessage = "Wachtwoord bevat minder dan 8 karakters!";
                    return false;
                }
                PasswordFailMessage = string.Empty;
                return true;
            }
            PasswordFailMessage = "Wachtwoorden zijn niet hetzelfde!";
            return false;
        }

        public bool EmptyFieldValidation(string email, string name, string password, string passwordConfirmation)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordConfirmation))
            {
                emptyFieldMessage = "1 of meerdere velden zijn leeg!";
                return true;
            }
            emptyFieldMessage = string.Empty;
            return false;
        }
    }
}
