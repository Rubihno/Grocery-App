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
        public string EmptyFieldMessage { get; set; }
        public string PriceFailMessage { get; set; }
        public string DateFailMessage { get; set; }

        public List<bool> validationList { get; set; } = [];

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

        public bool NameValidation(string name, List<Product> productList)
        {
            if (name.Length < 3)
            {
                NameFailMessage = "Productnaam bevat minder dan 3 karakters!";
                validationList.Add(false);
                return false;
            }

            foreach (Product product in productList)
            {
                if (product.Name == name)
                {
                    NameFailMessage = "Productnaam bestaat al!";
                    validationList.Add(false);
                    return false;
                }
            }

            NameFailMessage = string.Empty;
            validationList.Add(true);
            return true;
        }
        public bool NameValidation(string name, List<GroceryList> groceryLists)
        {
            if (name.Length < 3)
            {
                NameFailMessage = "Lijstnaam bevat minder dan 3 karakters!";
                validationList.Add(false);
                return false;
            }

            foreach (GroceryList item in groceryLists)
            {
                if (item.Name == name)
                {
                    NameFailMessage = "Lijstnaam bestaat al!";
                    validationList.Add(false);
                    return false;
                }
            }

            NameFailMessage = string.Empty;
            validationList.Add(true);
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

        public virtual bool EmptyFieldValidation(string email, string name, string password, string passwordConfirmation)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordConfirmation))
            {
                EmptyFieldMessage = "1 of meerdere velden zijn leeg!";
                return true;
            }
            EmptyFieldMessage = string.Empty;
            return false;
        }

        public bool EmptyFieldValidation(string name, int? stock, decimal? price)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(stock.ToString()) || string.IsNullOrEmpty(price.ToString()))
            {
                EmptyFieldMessage = "1 of meerdere velden zijn leeg!";
                return true;
            }
            EmptyFieldMessage = string.Empty;
            return false;
        }
        public bool EmptyFieldValidation(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                EmptyFieldMessage = "Vul een naam voor de boodschappenlijst in!";
                return true;
            }
            EmptyFieldMessage = string.Empty;
            return false;
        }

        public bool PriceValidation(decimal price)
        {
            if (!price.ToString().Contains(","))
            {
                PriceFailMessage = $"Prijs fout ingevoerd, gebruik een komma, bijvoorbeeld: 2,99";
                validationList.Add(false);
                return false;
            }
            else if (price.Scale != 2)
            {
                PriceFailMessage = "Prijs beschikt niet over 2 decimalen achter de komma";
                validationList.Add(false);
                return false;
            }
            PriceFailMessage = string.Empty;
            validationList.Add(true);
            return true;
        }

        public bool DateValidation(DateTime date)
        {
            if (date.Date <= DateTime.Today)
            {
                DateFailMessage = "Ingevoerde datum is vandaag of verlopen!";
                validationList.Add(false);
                return false;
            }
            DateFailMessage = string.Empty;
            validationList.Add(true);
            return true;
        }

        public List<bool> GetValidationCheckList()
        {
            return validationList;
        }

        public void ClearValidationCheckList()
        {
            validationList.Clear();
        }
    }
}
