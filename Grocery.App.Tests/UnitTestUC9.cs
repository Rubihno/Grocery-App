using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Grocery.Core.Helpers;
using Moq;
using NUnit.Framework;
using System.Runtime.CompilerServices;
using Grocery.Core.Interfaces.Services;

namespace Grocery.App.Tests
{
    public class BaseViewModelTests
    {
        public Mock<IClientRepository> _mockClientRepository = new();
        public Mock<IClientService> _mockClientService;
        public ValidationService _validatieService;
        public List<Client> mockClientList;

        [SetUp]
        public void Setup()
        {
            _mockClientRepository = new Mock<IClientRepository>();
            _validatieService = new ValidationService();
            _mockClientService = new Mock<IClientService>();

            mockClientList = new List<Client>()
            {
                new Client(1, "M.J. Curie", "user1@mail.com", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08="),
                new Client(2, "H.H. Hermans", "user2@mail.com", "dOk+X+wt+MA9uIniRGKDFg==.QLvy72hdG8nWj1FyL75KoKeu4DUgu5B/HAHqTD2UFLU="),
                new Client(3, "A.J. Kwak", "user3@mail.com", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=")
            };

            _mockClientRepository.Setup(x => x.GetAll()).Returns(mockClientList);
            _mockClientService.Setup(x => x.AddNieuwAccountToClientList(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                  .Callback<int, string, string, string>((id, gebruikersnaam, email, wachtwoord) =>
                  {
                      var hashWachtwoord = PasswordHelper.HashPassword(wachtwoord);
                      var newClient = new Client(id, gebruikersnaam, email, hashWachtwoord);
                      mockClientList.Add(newClient);
                  });
        }
    }

    [TestFixture]
    public class LegeVeldenTests : BaseViewModelTests
    {
        [Test]
        public void RegistratieLegeVelden_GeenLegeVelden_ReturnFalse()
        {
            string email = "ruben@mail.com";
            string gebruikersnaam = "Ruben";
            string wachtwoord = "wachtwoord123";
            string wachtwoordBevestiging = "wachtwoord123";

            bool legeVeldenCheck = _validatieService.EmptyFieldValidation(email, gebruikersnaam, wachtwoord, wachtwoordBevestiging); ;

            Assert.IsFalse(legeVeldenCheck);
            Assert.AreEqual(string.Empty, _validatieService.EmptyFieldMessage);
        }

        [TestCase ("", "Ruben", "wachtwoord123", "wachtwoord123")]
        [TestCase("", "", "", "")]
        public void RegistratieLegeVelden_LegeVeldenMessage_ReturnTrue(string email, string gebruikersnaam, string wachtwoord, string wachtwoordBevestiging)
        {
            bool legeVeldenCheck = _validatieService.EmptyFieldValidation(email, gebruikersnaam, wachtwoord, wachtwoordBevestiging);

            Assert.IsTrue(legeVeldenCheck);
            Assert.AreEqual("1 of meerdere velden zijn leeg!", _validatieService.EmptyFieldMessage);
        }
    }

    [TestFixture]
    public class EmailadresValidatieTests : BaseViewModelTests
    {
        [Test]
        public void EmailAdresValidatie_GeldigeEmail_ReturnTrue()
        {
            string geldigEmail = "user@mail.com";

            bool emailCheck = _validatieService.EmailValidation(geldigEmail);

            Assert.IsTrue(emailCheck);
            Assert.IsEmpty(_validatieService.EmailFailMessage);
        }

        [TestCase("user@")]
        [TestCase("usermail.com")]
        [TestCase("@mail.com")]
        [TestCase("")]
        public void EmailAdresValidatie_OngeldigeEmails_ReturnFalse(string email)
        {
            bool emailCheck = _validatieService.EmailValidation(email);

            Assert.IsFalse(emailCheck);
            Assert.AreEqual("Geen geldig e-mailadres ingevuld!", _validatieService.EmailFailMessage);
        }
    }

    [TestFixture]
    public class GebruikersValidatieTests : BaseViewModelTests 
    {
        [Test]
        public void GebruikersnaamValidatie_GeldigeGebruikersnaam_ReturnTrue()
        {

            string gebruikersnaam = "Gebruiker01";

            bool gebruikersnaamCheck = _validatieService.NameValidation(gebruikersnaam, _mockClientRepository.Object.GetAll());

            Assert.IsTrue(gebruikersnaamCheck);
            Assert.IsEmpty(_validatieService.NameFailMessage);

            _mockClientRepository.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void GebruikersnaamValidatie_BestaandeGebruikersnaam_ReturnFalse()
        {
            string gebruikersnaam = "A.J. Kwak";
            bool gebruikersnaamCheck = _validatieService.NameValidation(gebruikersnaam, _mockClientRepository.Object.GetAll());

            Assert.IsFalse(gebruikersnaamCheck);
            Assert.AreEqual("Gebruikersnaam bestaat al!", _validatieService.NameFailMessage);

            _mockClientRepository.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void GebruikersnaamValidatie_TeKorteGebruikersnaam_ReturnFalse()
        {
            string gebruikersnaam = "1";
            bool gebruikersnaamCheck = _validatieService.NameValidation(gebruikersnaam, _mockClientRepository.Object.GetAll());

            Assert.IsFalse(gebruikersnaamCheck);
            Assert.AreEqual("Gebruikersnaam bevat minder dan 5 karakters!", _validatieService.NameFailMessage);

            _mockClientRepository.Verify(x => x.GetAll(), Times.Once);
        }
    }

    [TestFixture]
    public class WachtwoordValidatieTests : BaseViewModelTests
    {
        [Test]
        public void WachtwoordValidatie_WachtwoordenZijnGelijk_ReturnTrue()
        {
            string wachtwoord = "wachtwoord123";
            string wachtwoordBevestiging = "wachtwoord123";

            bool wachtwoordCheck = _validatieService.PasswordValidation(wachtwoord, wachtwoordBevestiging);

            Assert.IsTrue(wachtwoordCheck);
            Assert.IsEmpty(_validatieService.PasswordFailMessage);
        }

        [Test]
        public void WachtwoordValidatie_WachtwoordenZijnNietGelijk_ReturnFalse()
        {
            string wachtwoord = "wachtwoord123";
            string wachtwoordBevestiging = "wachtwoord321";

            bool wachtwoordCheck = _validatieService.PasswordValidation(wachtwoord, wachtwoordBevestiging);

            Assert.IsFalse(wachtwoordCheck);
            Assert.AreEqual("Wachtwoorden zijn niet hetzelfde!", _validatieService.PasswordFailMessage);
        }

        [Test]
        public void WachtwoordValidatie_WachtwoordenZijnTeKort_ReturnFalse()
        {
            string wachtwoord = "ww123";
            string wachtwoordBevestiging = "ww123";

            bool wachtwoordCheck = _validatieService.PasswordValidation(wachtwoord, wachtwoordBevestiging);

            Assert.IsFalse(wachtwoordCheck);
            Assert.AreEqual("Wachtwoord bevat minder dan 8 karakters!", _validatieService.PasswordFailMessage);
        }
    }

    [TestFixture]
    public class HashPasswordTest : BaseViewModelTests
    {
        [Test]
        public void HashPassword_PasswordIsGehasht_IsTrue()
        {
            int id = mockClientList.Count + 1;
            string email = "gebruiker@mail.com";
            string gebruikersnaam = "user123";
            string wachtwoord = "gebruiker123";

            _mockClientService.Object.AddNieuwAccountToClientList(id, email, gebruikersnaam, wachtwoord);       

            Client addedClient = mockClientList.FirstOrDefault(c => c.Id == id);

            Assert.IsTrue(PasswordHelper.VerifyPassword(wachtwoord, addedClient.Password));
        }
    }
}