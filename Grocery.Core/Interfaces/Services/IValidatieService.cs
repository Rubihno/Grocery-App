using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Interfaces.Services
{
    public interface IValidatieService
    {
        public string EmailFailMessage { get; set; }
        public string GebruikersnaamFailMessage { get; set; }
        public string WachtwoordFailMessage { get; set; }
        public string veldenLeegMessage { get; set; }

        public bool EmailValidatie(string email);
        public bool GebruikersnaamValidatie(string gebruikersnaam, List<Client> clientList);
        public bool WachtwoordValidatie(string wachtwoord, string wachtwoordBevestiging);
        public bool LegeVeldenValidatie(string email, string gebruikersnaam, string wachtwoord, string wachtwoordBevestiging);

    }
}
