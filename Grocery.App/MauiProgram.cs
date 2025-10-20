using Grocery.Core.Services;
using Grocery.App.ViewModels;
using Grocery.App.Views;
using Microsoft.Extensions.Logging;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Data.Repositories;
using CommunityToolkit.Maui;

namespace Grocery.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IGroceryListService, GroceryListService>();
            builder.Services.AddSingleton<IGroceryListItemsService, GroceryListItemsService>();
            builder.Services.AddSingleton<IProductService, ProductService>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IClientService, ClientService>();
            builder.Services.AddSingleton<IFileSaverService, FileSaverService>();
            builder.Services.AddSingleton<IValidationService, ValidationService>();
            builder.Services.AddSingleton<IBoughtProductsService, BoughtProductsService>();
            builder.Services.AddSingleton<ICategoryService, CategoryService>();
            builder.Services.AddSingleton<IProductCategoryService, ProductCategoryService>();

            builder.Services.AddSingleton<IGroceryListRepository, GroceryListRepository>();
            builder.Services.AddSingleton<IGroceryListItemsRepository, GroceryListItemsRepository>();
            builder.Services.AddSingleton<IProductRepository, ProductRepository>();
            builder.Services.AddSingleton<IClientRepository, ClientRepository>();
            builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
            builder.Services.AddSingleton<IProductCategoryRepository, ProductCategoryRepository>();
            builder.Services.AddSingleton<GlobalViewModel>();

            builder.Services.AddTransient<GroceryListsView>().AddTransient<GroceryListViewModel>();
            builder.Services.AddTransient<GroceryListItemsView>().AddTransient<GroceryListItemsViewModel>();
            builder.Services.AddTransient<ProductView>().AddTransient<ProductViewModel>();
            builder.Services.AddTransient<ChangeColorView>().AddTransient<ChangeColorViewModel>();
            builder.Services.AddTransient<LoginView>().AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegistratieView>().AddTransient<RegistrationViewModel>();
            builder.Services.AddTransient<BestSellingProductsView>().AddTransient<BestSellingProductsViewModel>();
            builder.Services.AddTransient<BoughtProductsView>().AddTransient<BoughtProductsViewModel>();
            builder.Services.AddTransient<CategoriesView>().AddTransient<CategoriesViewModel>();
            builder.Services.AddTransient<ProductCategoriesView>().AddTransient<ProductCategoriesViewModel>();
            builder.Services.AddTransient<NewProductView>().AddTransient<NewProductViewModel>();
            builder.Services.AddTransient<NewGroceryListView>().AddTransient<NewGroceryListViewModel>();

            return builder.Build();
        }
    }
}
