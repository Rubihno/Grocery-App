using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.App.Tests.Interfaces
{
    public interface IClientRepositoryMock
    {
        public MockClient? Get(string email);
        public MockClient? Get(int id);
        public List<MockClient> GetAll();
        public void AddClient(MockClient clientAccount);
    }
}
