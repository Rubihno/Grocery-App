using Grocery.App.Tests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.App.Tests
{
    public class MockClientRepository : IClientRepositoryMock
    {
        private readonly List<MockClient> clientList;

        public MockClientRepository()
        {
            clientList = [
                new MockClient(1, "M.J. Curie", "user1@mail.com", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08="),
                new MockClient(2, "H.H. Hermans", "user2@mail.com", "dOk+X+wt+MA9uIniRGKDFg==.QLvy72hdG8nWj1FyL75KoKeu4DUgu5B/HAHqTD2UFLU="),
                new MockClient(3, "A.J. Kwak", "user3@mail.com", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=")
            ];
        }

        public MockClient? Get(string email)
        {
            MockClient? client = clientList.FirstOrDefault(c => c.EmailAddress.Equals(email));
            return client;
        }

        public MockClient? Get(int id)
        {
            MockClient? client = clientList.FirstOrDefault(c => c.Id == id);
            return client;
        }

        public List<MockClient> GetAll()
        {
            return clientList;
        }

        public void AddClient(MockClient clientAccount)
        {
            clientList.Add(clientAccount);
        }
    }
}
