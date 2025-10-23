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
    public class BaseModelTests
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
                  .Callback<int, string, string, string>((id, username, email, wachtwoord) =>
                  {
                      var hashPassword = PasswordHelper.HashPassword(wachtwoord);
                      var newClient = new Client(id, username, email, hashPassword);
                      mockClientList.Add(newClient);
                  });
        }
    }

    [TestFixture]
    public class EmptyFieldsTests : BaseModelTests
    {
        [Test]
        public void RegistrationEmptyFields_NoEmptyFields_ReturnFalse()
        {
            string email = "ruben@mail.com";
            string gebruikersnaam = "Ruben";
            string wachtwoord = "wachtwoord123";
            string wachtwoordBevestiging = "wachtwoord123";

            bool emptyFieldsCheck = _validatieService.EmptyFieldValidation(email, gebruikersnaam, wachtwoord, wachtwoordBevestiging); ;

            Assert.IsFalse(emptyFieldsCheck);
            Assert.AreEqual(string.Empty, _validatieService.EmptyFieldMessage);
        }

        [TestCase ("", "Ruben", "wachtwoord123", "wachtwoord123")]
        [TestCase("", "", "", "")]
        public void RegistrationEmptyFields_EmptyFields_ReturnTrue(string email, string gebruikersnaam, string wachtwoord, string wachtwoordBevestiging)
        {
            bool emptyFieldsCheck = _validatieService.EmptyFieldValidation(email, gebruikersnaam, wachtwoord, wachtwoordBevestiging);

            Assert.IsTrue(emptyFieldsCheck);
            Assert.AreEqual("1 of meerdere velden zijn leeg!", _validatieService.EmptyFieldMessage);
        }
    }

    [TestFixture]
    public class EmailValidationTests : BaseModelTests
    {
        [Test]
        public void EmailValidation_ValidEmail_ReturnTrue()
        {
            string validEmail = "user@mail.com";

            bool emailCheck = _validatieService.EmailValidation(validEmail);

            Assert.IsTrue(emailCheck);
            Assert.IsEmpty(_validatieService.EmailFailMessage);
        }

        [TestCase("user@")]
        [TestCase("usermail.com")]
        [TestCase("@mail.com")]
        [TestCase("")]
        public void EmailValidation_InvalidEmail_ReturnFalse(string email)
        {
            bool emailCheck = _validatieService.EmailValidation(email);

            Assert.IsFalse(emailCheck);
            Assert.AreEqual("Geen geldig e-mailadres ingevuld!", _validatieService.EmailFailMessage);
        }
    }

    [TestFixture]
    public class UsernameValidationTests : BaseModelTests 
    {
        [Test]
        public void UsernameValidation_ValidUsername_ReturnTrue()
        {

            string username = "Gebruiker01";

            bool usernameCheck = _validatieService.NameValidation(username, _mockClientRepository.Object.GetAll());

            Assert.IsTrue(usernameCheck);
            Assert.IsEmpty(_validatieService.NameFailMessage);

            _mockClientRepository.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void UsernameValidation_ExistingUsername_ReturnFalse()
        {
            string username = "A.J. Kwak";
            bool usernameCheck = _validatieService.NameValidation(username, _mockClientRepository.Object.GetAll());

            Assert.IsFalse(usernameCheck);
            Assert.AreEqual("Gebruikersnaam bestaat al!", _validatieService.NameFailMessage);

            _mockClientRepository.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void UsernameValidation_UsernameTooShort_ReturnFalse()
        {
            string username = "1";
            bool usernameCheck = _validatieService.NameValidation(username, _mockClientRepository.Object.GetAll());

            Assert.IsFalse(usernameCheck);
            Assert.AreEqual("Gebruikersnaam bevat minder dan 5 karakters!", _validatieService.NameFailMessage);

            _mockClientRepository.Verify(x => x.GetAll(), Times.Once);
        }
    }

    [TestFixture]
    public class WachtwoordValidatieTests : BaseModelTests
    {
        [Test]
        public void PasswordValidation_PasswordsAreEqual_ReturnTrue()
        {
            string password = "wachtwoord123";
            string passwordConfirmation = "wachtwoord123";

            bool passwordCheck = _validatieService.PasswordValidation(password, passwordConfirmation);

            Assert.IsTrue(passwordCheck);
            Assert.IsEmpty(_validatieService.PasswordFailMessage);
        }

        [Test]
        public void PasswordValidation_PasswordsAreNotEqual_ReturnFalse()
        {
            string password = "wachtwoord123";
            string passwordConfirmation = "wachtwoord321";

            bool passwordCheck = _validatieService.PasswordValidation(password, passwordConfirmation);

            Assert.IsFalse(passwordCheck);
            Assert.AreEqual("Wachtwoorden zijn niet hetzelfde!", _validatieService.PasswordFailMessage);
        }

        [Test]
        public void PasswordValidation_PasswordsTooShort_ReturnFalse()
        {
            string password = "ww123";
            string passwordConfirmation = "ww123";

            bool passwordCheck = _validatieService.PasswordValidation(password, passwordConfirmation);

            Assert.IsFalse(passwordCheck);
            Assert.AreEqual("Wachtwoord bevat minder dan 8 karakters!", _validatieService.PasswordFailMessage);
        }
    }

    [TestFixture]
    public class HashPasswordTest : BaseModelTests
    {
        [Test]
        public void HashPassword_PasswordIsHashed_IsTrue()
        {
            int id = mockClientList.Count + 1;
            string email = "gebruiker@mail.com";
            string username = "user123";
            string password = "gebruiker123";

            _mockClientService.Object.AddNieuwAccountToClientList(id, email, username, password);       

            Client addedClient = mockClientList.FirstOrDefault(c => c.Id == id);

            Assert.IsTrue(PasswordHelper.VerifyPassword(password, addedClient.Password));
        }
    }
}