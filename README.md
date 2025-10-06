# GroceryApp sprint5 Studentversie  
Dit is de startversie voor studenten voor sprint 5.  
 
## UC15 Toevoegen THT datum aan product 
Is compleet.  

## UC14 Toevoegen prijzen:  
- Prijs toevoegen aan product class en uitbreiden constructor chain.  
- ProductRepository --> prijsveld vullen met waarden.  
- ProductView uitbreiden met kolom voor de prijs (header en inhoud van de tabel).      

## UC12 Productcategoriën toevoegen --> zelfstandig uitwerken:  
Ontwerp:
>```mermaid
>classDiagram
>direction LR
>    class Product {
>	    +int Id
>	    +string Name
>	    +int Stock
>	    +DateOnly ShelfLife
>	    +Decimal Price
>   }
>    class ProductCategory {
>	    +int Id
>	    +string Name
>	    +int ProductId
>	    +int CategoryId
>    }
>    class Category {
>	    +int Id
>	    +string Name
>    }
>
>    Product "1" -- "*" ProductCategory
>    ProductCategory "*" -- "1" Category
> ```
Stappenplan:  
- Maak class Category  
- Maak class ProductCategory  
- Maak Interface en Repository voor Category  
- Maak Interface en Repository voor ProductCategory  
- Maak Interface en Service voor Category  
- Maak Interface en Service voor ProductCategory  
- Registreer de gemaakte Repo's en services in MauiProgramm  
- Maak CategoriesViewModel.  
- Maak CategoriesView.  
- Registreer De view en het ViewModel in MauiProgramm.  
- Maak een menu entry in de tabbar in AppShell.xaml en registreer route in AppShell.xaml.cs  
- Maak ProductCategoriesViewModel.  
- Maak ProductCategoriesView.  
- Registreer De view en het ViewModel in MauiProgramm.  
- Zorg dat de ProductCategoriesView gestart kan worden na het klikken op een Category in CategoriesView  
- Registreer route naar ProductCategoriesView in AppShell.xaml.cs  
