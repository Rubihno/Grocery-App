
using CommunityToolkit.Mvvm.ComponentModel;
using Grocery.Core.Enums;

namespace Grocery.Core.Models
{
    public partial class Client : Model
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Role currentRole;
        public Client(int id, string name, string emailAddress, string password, Role role = Role.None) : base(id, name)
        {
            EmailAddress=emailAddress;
            Password=password;
            currentRole = role;
        }
    }
}
