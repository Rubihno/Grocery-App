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
        public string emptyFieldMessage { get; set; }

        public bool EmailValidation(string email);
        public bool NameValidation(string name, List<Client> clientList);
        public bool PasswordValidation(string password, string passwordConfirmation);
        public bool EmptyFieldValidation(string email, string name, string password, string passwordConfirmation);

    }
}
