using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Interfaces.Services
{
    public interface IValidationService
    {
        public string EmailFailMessage { get; set; }
        public string NameFailMessage { get; set; }
        public string PasswordFailMessage { get; set; }
        public string EmptyFieldMessage { get; set; }
        public string PriceFailMessage { get; set; }
        public string DateFailMessage { get; set; }

        public List<bool> validationList { get; set; }

        public bool EmailValidation(string email);
        public bool NameValidation(string name, List<Client> clientList);
        public bool NameValidation(string name, List<Product> productList);
        public bool NameValidation(string name, List<GroceryList> groceryLists);
        public bool PasswordValidation(string password, string passwordConfirmation);
        public bool EmptyFieldValidation(string email, string name, string password, string passwordConfirmation);
        public bool EmptyFieldValidation(string name, int? stock, decimal? price);
        public bool EmptyFieldValidation(string name);
        public bool PriceValidation(decimal price);
        public bool DateValidation(DateTime date);
        public List<bool> GetValidationCheckList();
        public void ClearValidationCheckList();
    }
}
