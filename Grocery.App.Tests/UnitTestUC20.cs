using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Microsoft.Data.Sqlite;
using Moq;
using System.Globalization;

namespace Grocery.App.Tests
{
    public class BaseModelTestsUC20
    {
        public Mock<IGroceryListService> _mockGroceryListService;
        public ValidationService _validationService;
        public List<GroceryList> mockGroceryLists;

        [SetUp]
        public void Setup()
        {
            _mockGroceryListService = new Mock<IGroceryListService>();
            _validationService = new ValidationService();

            mockGroceryLists = new List<GroceryList>()
            {
                new GroceryList(1, "Boodschappen familieweekend", DateOnly.Parse("2024-12-14"), "#FF6A00", 1),
                new GroceryList(2, "Kerstboodschappen", DateOnly.Parse("2024-12-07"), "#626262", 1),
                new GroceryList(3, "Weekend boodschappen", DateOnly.Parse("2024-11-30"), "#003300", 1)
            };

            _mockGroceryListService.Setup(x => x.GetAll()).Returns(mockGroceryLists);
            _mockGroceryListService.Setup(x => x.Add(It.IsAny<GroceryList>()))
                  .Callback<GroceryList>(item =>
                  {
                      mockGroceryLists.Add(item);
                  });
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");

            TestContext.WriteLine("Setup complete!");
        }
    }

    [TestFixture]
    public class CreateGroceryListEmptyFieldsTests : BaseModelTestsUC20
    {
        [Test]
        public void CreateGroceryListEmptyFields_NoEmptyFields_ReturnFalse()
        {
            string name = "Vakantie boodschappen";

            bool emptyFieldsCheck = _validationService.EmptyFieldValidation(name);

            Assert.IsFalse(emptyFieldsCheck);
            Assert.AreEqual(string.Empty, _validationService.EmptyFieldMessage);

            TestContext.WriteLine("Test success: No empty fields detected!");
        }

        [Test]
        public void CreateGroceryListEmptyFields_EmptyFields_ReturnTrue()
        {
            string name = "";
            bool emptyFieldsCheck = _validationService.EmptyFieldValidation(name);

            Assert.IsTrue(emptyFieldsCheck);
            Assert.AreEqual("Vul een naam voor de boodschappenlijst in!", _validationService.EmptyFieldMessage);
            TestContext.WriteLine($"Test success: name field was detected as empty!");
        }
    }

    [TestFixture]
    public class GroceryListNameValidationTests : BaseModelTestsUC20
    {
        [Test]
        public void GroceryListNameValidation_ValidGroceryListName_ReturnTrue()
        {

            string name = "Vakantie boodschappen";

            bool productNameCheck = _validationService.NameValidation(name, _mockGroceryListService.Object.GetAll());

            Assert.IsTrue(productNameCheck);
            Assert.IsEmpty(_validationService.NameFailMessage);

            _mockGroceryListService.Verify(x => x.GetAll(), Times.Once);
            TestContext.WriteLine("Test success: valid grocery list name detected");
        }

        [Test]
        public void GroceryListNameValidation_ExistingGroceryListName_ReturnFalse()
        {
            string name = "Kerstboodschappen";
            bool productNameCheck = _validationService.NameValidation(name, _mockGroceryListService.Object.GetAll());

            Assert.IsFalse(productNameCheck);
            Assert.AreEqual("Lijst naam bestaat al!", _validationService.NameFailMessage);

            _mockGroceryListService.Verify(x => x.GetAll(), Times.Once);
            TestContext.WriteLine("Test success: existing grocery list name detected");
        }

        [Test]
        public void GroceryListNameValidation_ShortGroceryListName_ReturnFalse()
        {
            string name = "1";
            bool productNameCheck = _validationService.NameValidation(name, _mockGroceryListService.Object.GetAll());

            Assert.IsFalse(productNameCheck);
            Assert.AreEqual("Lijst naam bevat minder dan 3 karakters!", _validationService.NameFailMessage);

            _mockGroceryListService.Verify(x => x.GetAll(), Times.Once);
            TestContext.WriteLine("Test success: grocery list name with less then 3 characters detected");
        }
    }

    public class CreateGroceryList() : BaseModelTestsUC20
    {
        [Test]
        public void CreateGroceryList_NewGroceryListCreated_ReturnTrue()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            using var createCmd = connection.CreateCommand();
            createCmd.CommandText = @"CREATE TABLE GroceryLists (
                                                    [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                                    [Name] NVARCHAR(80) UNIQUE NOT NULL,
                                                    [Date] DATE NOT NULL,
                                                    [Color] NVARCHAR(12) NOT NULL,
                                                    [ClientId] INTEGER NOT NULL);";
            createCmd.ExecuteNonQuery();

            using var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = "INSERT INTO GroceryLists (Name, Date, Color, ClientId) VALUES ('Test boodschappen', '2025-11-22','#FFFFFF', 1);";
            insertCmd.ExecuteNonQuery();

            using var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT Name FROM GroceryLists;";
            var result = selectCmd.ExecuteScalar().ToString();

            Assert.AreEqual("Test boodschappen", result);
            TestContext.WriteLine("Test success: grocery list successfully created");
        }
    }
}
