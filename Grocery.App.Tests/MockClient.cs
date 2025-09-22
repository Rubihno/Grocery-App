using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.App.Tests
{
    public class MockClient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public MockClient(int id, string name, string emailAddress, string password)
        {
            Id = id;
            Name = name;
            EmailAddress = emailAddress;
            Password = password;
        }
    }
}
