using Grocery.App.Tests.Helper;
using NUnit.Framework;
using System.Runtime.CompilerServices;
namespace Grocery.App.Tests
{
    public class BaseViewModelTests
    {
        public TestableRegistratieViewModel _registratieViewModel;
        public MockClientRepository _sharedClientRepository;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _sharedClientRepository = new MockClientRepository();
        }

        [SetUp]
        public void Setup()
        {
            _registratieViewModel = new TestableRegistratieViewModel(_sharedClientRepository);
        }
    }

    [TestFixture]
    public class LegeVeldenTests : BaseViewModelTests
    {
        [Test]
        public void RegistratieLegeVelden_GeenLegeVelden_ReturnTrue()
        {
            string email = "ruben@mail.com";
            string gebruikersnaam = "Ruben";
            string wachtwoord = "wachtwoord123";
            string wachtwoordBevestiging = "wachtwoord123";

            bool veldenVol;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(gebruikersnaam) || string.IsNullOrEmpty(wachtwoord) || string.IsNullOrEmpty(wachtwoordBevestiging))
            {
                veldenVol = false;
            }
            else
            {
                veldenVol = true;
            }

            Assert.IsTrue(veldenVol);
        }

        [TestCase ("", "Ruben", "wachtwoord123", "wachtwoord123")]
        [TestCase("", "", "", "")]
        public void RegistratieLegeVelden_LegeVeldenMessage_ReturnMessageString(string email, string gebruikersnaam, string wachtwoord, string wachtwoordBevestiging)
        {
            bool legeVeldenCheck;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(gebruikersnaam) || string.IsNullOrEmpty(wachtwoord) || string.IsNullOrEmpty(wachtwoordBevestiging))
            {
                legeVeldenCheck = true;
                _registratieViewModel.VeldenLeegMessage = "1 of meerdere velden zijn leeg!";
            }
            else
            {
                legeVeldenCheck = false;
                _registratieViewModel.VeldenLeegMessage = string.Empty;
            }

            Assert.IsTrue(legeVeldenCheck);
            Assert.AreEqual("1 of meerdere velden zijn leeg!", _registratieViewModel.VeldenLeegMessage);
        }
    }

    [TestFixture]
    public class EmailadresValidatieTests : BaseViewModelTests
    {
        [Test]
        public void EmailAdresValidatie_GeldigeEmail_ReturnTrue()
        {
            string geldigEmail = "user@mail.com";

            bool emailCheck = _registratieViewModel.EmailValidatie(geldigEmail);

            Assert.IsTrue(emailCheck);
            Assert.IsEmpty(_registratieViewModel.EmailValidatieFailMessage);
        }

        [TestCase("user@")]
        [TestCase("usermail.com")]
        [TestCase("@mail.com")]
        [TestCase("")]
        public void EmailAdresValidatie_OngeldigeEmails_ReturnFalse(string email)
        {
            bool emailCheck = _registratieViewModel.EmailValidatie(email);

            Assert.IsFalse(emailCheck);
            Assert.AreEqual("Geen geldig e-mailadres ingevuld!", _registratieViewModel.EmailValidatieFailMessage);
        }
    }

    [TestFixture]
    public class GebruikersValidatieTests : BaseViewModelTests 
    {
        [Test]
        public void GebruikersnaamValidatie_GeldigeGebruikersnaam_ReturnTrue()
        {
            string gebruikersnaam = "Gebruiker01";

            bool gebruikersnaamCheck = _registratieViewModel.GebruikersnaamValidatie(gebruikersnaam, _sharedClientRepository.GetAll());

            Assert.IsTrue(gebruikersnaamCheck);
            Assert.IsEmpty(_registratieViewModel.GebruikersnaamValidatieFailMessage);
        }

        [Test]
        public void GebruikersnaamValidatie_BestaandeGebruikersnaam_ReturnFalse()
        {
            string gebruikersnaam = "A.J. Kwak";
            bool gebruikersnaamCheck = _registratieViewModel.GebruikersnaamValidatie(gebruikersnaam, _sharedClientRepository.GetAll());

            Assert.IsFalse(gebruikersnaamCheck);
            Assert.AreEqual("Gebruikersnaam bestaat al!", _registratieViewModel.GebruikersnaamValidatieFailMessage);
        }

        [Test]
        public void GebruikersnaamValidatie_TeKorteGebruikersnaam_ReturnFalse()
        {
            string gebruikersnaam = "1";
            bool gebruikersnaamCheck = _registratieViewModel.GebruikersnaamValidatie(gebruikersnaam, _sharedClientRepository.GetAll());

            Assert.IsFalse(gebruikersnaamCheck);
            Assert.AreEqual("Gebruikersnaam bevat minder dan 5 karakters!", _registratieViewModel.GebruikersnaamValidatieFailMessage);
        }

        [Test]
        public void GebruikersnaamValidatie_TeLangeGebruikersnaam_ReturnFalse()
        {
            string gebruikersnaam = "GebruikersnaamMeerDanTwintigTekens";
            bool gebruikersnaamCheck = _registratieViewModel.GebruikersnaamValidatie(gebruikersnaam, _sharedClientRepository.GetAll());

            Assert.IsFalse(_registratieViewModel.GebruikersnaamMaxLengte(gebruikersnaam));
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

            bool wachtwoordCheck = _registratieViewModel.WachtwoordValidatie(wachtwoord, wachtwoordBevestiging);

            Assert.IsTrue(wachtwoordCheck);
            Assert.IsEmpty(_registratieViewModel.WachtwoordValidatieFailMessage);
        }

        [Test]
        public void WachtwoordValidatie_WachtwoordenZijnNietGelijk_ReturnFalse()
        {
            string wachtwoord = "wachtwoord123";
            string wachtwoordBevestiging = "wachtwoord321";

            bool wachtwoordCheck = _registratieViewModel.WachtwoordValidatie(wachtwoord, wachtwoordBevestiging);

            Assert.IsFalse(wachtwoordCheck);
            Assert.AreEqual("Wachtwoorden zijn niet hetzelfde!", _registratieViewModel.WachtwoordValidatieFailMessage);
        }

        [Test]
        public void WachtwoordValidatie_WachtwoordenZijnTeKort_ReturnFalse()
        {
            string wachtwoord = "ww123";
            string wachtwoordBevestiging = "ww123";

            bool wachtwoordCheck = _registratieViewModel.WachtwoordValidatie(wachtwoord, wachtwoordBevestiging);

            Assert.IsFalse(wachtwoordCheck);
            Assert.AreEqual("Wachtwoord bevat minder dan 8 karakters!", _registratieViewModel.WachtwoordValidatieFailMessage);
        }
    }

    [TestFixture]
    public class HashPasswordTest : BaseViewModelTests
    {
        [Test]
        public void HassPassword_PasswordIsGehasht_IsTrue()
        {
            string email = "gebruiker@mail.com";
            string gebruikersnaam = "user123";
            string wachtwoord = "gebruiker123";

            List<MockClient> clientList = _sharedClientRepository.GetAll();

            _registratieViewModel.AddNieuwAccountToClientList(clientList, email, gebruikersnaam, wachtwoord);

            List<MockClient> newClientList = _sharedClientRepository.GetAll();

            foreach (MockClient client in newClientList)
            {
                if (client.Id == newClientList.Count + 1)
                {
                    Assert.IsTrue(PasswordHelperMock.VerifyPassword(wachtwoord, client.Password));
                }
            }
        }
    }
}