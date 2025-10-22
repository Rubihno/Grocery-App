using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Moq;
using NUnit.Framework;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Grocery.App.Tests
{
    public class BaseViewModelTestsUC19
    {
        public Mock<IProductService> _mockProductService;
        public ValidationService _validationService;
        public List<Product> mockProductsList;

        [SetUp]
        public void Setup()
        {
            _mockProductService = new Mock<IProductService>();
            _validationService = new ValidationService();

            mockProductsList = new List<Product>()
            {
                new Product(2, "Kaas", 100, 4.50m, new DateOnly(2025, 9, 30)),
                new Product(3, "Brood", 400, 1.99m, new DateOnly(2025, 9, 12)),
                new Product(1, "Melk", 300, 1.45m, new DateOnly(2025, 9, 25)),
                new Product(4, "Cornflakes", 0, 2.99m, new DateOnly(2025, 12, 31))
            };

            _mockProductService.Setup(x => x.GetAll()).Returns(mockProductsList);
            _mockProductService.Setup(x => x.Add(It.IsAny<Product>()))
                  .Callback<Product>(item =>
                  {
                      Product newProduct = new Product(item.Id, item.Name, item.Stock, item.Price, item.ShelfLife);
                      mockProductsList.Add(newProduct);
                  });
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");

            TestContext.WriteLine("Setup complete!");
        }
    }
    [TestFixture]
    public class CreateProductEmptyFieldsTests : BaseViewModelTestsUC19
    {
        [Test]
        public void CreateProductEmptyFields_NoEmptyFields_ReturnFalse()
        {
            string name = "Aardbei";
            int stock = 200;
            decimal price = 2.99m;

            bool emptyFieldsCheck = _validationService.EmptyFieldValidation(name, stock, price);

            Assert.IsFalse(emptyFieldsCheck);
            Assert.AreEqual(string.Empty, _validationService.EmptyFieldMessage);

            TestContext.WriteLine("Test success: No empty fields detected!");
        }

        [TestCase(1, "", 200, 2.99)]
        [TestCase(2, "Aardbei", 0, 2.99)]
        [TestCase(3, "Aardbei", 200, 0)]
        [TestCase(4, "", 0, 0)]
        public void CreateProductEmptyFields_EmptyFields_ReturnTrue(int testcase, string name, int nullableStock, decimal nullablePrice)
        {
            int? stock = nullableStock == 0 ? null : nullableStock;
            decimal? price = nullablePrice == 0 ? null : nullablePrice;
            bool emptyFieldsCheck = _validationService.EmptyFieldValidation(name, stock, price);

            Assert.IsTrue(emptyFieldsCheck);
            Assert.AreEqual("1 of meerdere velden zijn leeg!", _validationService.EmptyFieldMessage);
            TestContext.WriteLine($"Testcase {testcase} success: 1 or more empty fields detected");
        }
    }

    [TestFixture]
    public class ProductNameValidationTests : BaseViewModelTestsUC19
    {
        [Test]
        public void ProductNameValidation_ValidProductName_ReturnTrue()
        {

            string name = "Aardbei";

            bool productNameCheck = _validationService.NameValidation(name, _mockProductService.Object.GetAll());

            Assert.IsTrue(productNameCheck);
            Assert.IsEmpty(_validationService.NameFailMessage);

            _mockProductService.Verify(x => x.GetAll(), Times.Once);
            TestContext.WriteLine("Test success: valid productname detected");
        }

        [Test]
        public void ProductNameValidation_ExistingProductName_ReturnFalse()
        {
            string productName = "Melk";
            bool productNameCheck = _validationService.NameValidation(productName, _mockProductService.Object.GetAll());

            Assert.IsFalse(productNameCheck);
            Assert.AreEqual("Productnaam bestaat al!", _validationService.NameFailMessage);

            _mockProductService.Verify(x => x.GetAll(), Times.Once);
            TestContext.WriteLine("Test success: existing productname detected");
        }

        [Test]
        public void ProductNameValidation_ShortProductName_ReturnFalse()
        {
            string name = "1";
            bool productNameCheck = _validationService.NameValidation(name, _mockProductService.Object.GetAll());

            Assert.IsFalse(productNameCheck);
            Assert.AreEqual("Productnaam bevat minder dan 3 karakters!", _validationService.NameFailMessage);

            _mockProductService.Verify(x => x.GetAll(), Times.Once);
            TestContext.WriteLine("Test success: productname with less then 3 characters detected");
        }
    }

    public class PriceValidationTests() : BaseViewModelTestsUC19
    {
        [Test]
        public void PriceValidation_ValidFormat_ReturnTrue()
        {
            decimal price = 2.99m;
            bool passwordCheck = _validationService.PriceValidation(price);

            Assert.IsTrue(passwordCheck);
            Assert.AreEqual(string.Empty, _validationService.PriceFailMessage);
            TestContext.WriteLine("Test success: valid price format detected");
        }

        [Test]
        public void PriceValidation_InValidFormat_ReturnFalse()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            decimal price = 2.99m;
            bool passwordCheck = _validationService.PriceValidation(price);

            Assert.IsFalse(passwordCheck);
            Assert.AreEqual("Prijs fout ingevoerd, gebruik een komma, bijvoorbeeld: 2,99", _validationService.PriceFailMessage);
            TestContext.WriteLine("Test success: invalid price format detected");
        }

        [Test]
        public void PriceValidation_InValidDecimalCount_ReturnFalse()
        {
            decimal price = 2.999m;
            bool passwordCheck = _validationService.PriceValidation(price);

            Assert.IsFalse(passwordCheck);
            Assert.AreEqual("Prijs beschikt niet over 2 decimalen achter de komma", _validationService.PriceFailMessage);
            TestContext.WriteLine("Test success: invalid decimal count of more or less then 2 decimals detected");
        }
    }

    public class ShelfLifeDateTests : BaseViewModelTestsUC19
    {
        [Test]
        public void ShelfLifeDateTests_ValidDate_ReturnTrue()
        {
            DateTime date = DateTime.MaxValue;
            bool dateCheck = _validationService.DateValidation(date);

            Assert.IsTrue(dateCheck);
            Assert.AreEqual(string.Empty, _validationService.DateFailMessage);
            TestContext.WriteLine("Test success: valid shelflife date detected");
        }

        [Test]
        public void ShelfLifeDateTests_InValidDate_ReturnFalse()
        {
            DateTime date = DateTime.Now;
            bool dateCheck = _validationService.DateValidation(date);

            Assert.IsFalse(dateCheck);
            Assert.AreEqual("Ingevoerde datum is vandaag of verlopen!", _validationService.DateFailMessage);
            TestContext.WriteLine("Test success: invalid shelflife date detected");
        }
    }
}
