# Projekt: Aplikacja internetowa CollectorsBay

## Spis treœci
1. Lista i opis funkcjonalnoœci:
	* Metody HTTP
	* Widoki Razor Pages
	* Zabezpieczenia
2. Uruchomienie
	* Kroki
3. Przyk³adowe zapytanie

## Lista i opis funkcjonalnoœci
### Metody HTTP
Zaimplementowano nastêpuj¹ce metody: 
- POST/api/Auctions – dodanie przedmiotu do licytacji 
- GET/api/Auctions – pobieranie listy wszystkich aukcji 
- PUT/api/Autions/{id} – edycja przedmiotu do licytacji o podanym id 
- DELETE/api/Auctions/{id} – usuniêcie przedmiotu o podanym id z licytacji 
- GET/api/Auctions/{id} – pobranie aukcji o podanym id 
- GET/api/Auctions/user/{userId} – pobranie aukcji u¿ytkownika o podanym id 
- POST/api/Users – dodanie u¿ytkownika 
- PUT/api/Users/{id} – edycja danych u¿ytkownika o podanym id 
- DELETE/api/Users/{id} – usuniêcie u¿ytkownika o podanym id 
- GET/api/Users/{id} – pobranie danych u¿ytkownika o podanym id 

### Widoki Razor Pages 
Zaimplementowano nastêpuj¹ce widoki:
- /Index – strona g³ówna aplikacji z zak³adkami i linkiem do formularza rejestracji u¿ytkownika
- /Register – rejestracja: formularz rejestracji u¿ytkownika z obowi¹zkowymi polami i przyciskiem do zatwierdzenia
- /Account – konto u¿ytkownika z pobranymi danymi i list¹ wystawionych przedmiotów oraz przyciskami umo¿liwiaj¹cymi przejœcie do udzia³u w licytacji, wystawienie przedmiotu, edycjê profilu i usuniêcie profilu u¿ytkownika
- /SellerView – formularz do wystawienia nowego przedmiotu na aukcjê z obowi¹zkowymi polami i przyciskiem do dodania nowego przedmiotu i powrotu do poprzedniego widoku
- /EditProfile – formularz edycji profilu z przyciskami zapisu zmian i powrotu do poprzedniego widoku
- /EditItem – formularz edycji danych przedmiotu z przyciskami zapisu zmian, usuniêcia przedmiotu i powrotu do poprzedniego widoku
- /AuctionCategories – wybór kategorii aukcji spoœród 3 mo¿liwych: Ksi¹¿ki, Monety, Znaczki
- /AuctionList – lista aukcji z filtracj¹: wybrana kategoria, tylko niezakoñczone aukcje i tylko takie, których w³aœcicielem jest inny u¿ytkownik ni¿ u¿ytkownik bior¹cy udzia³ w aukcji
- /AuctionDetails – widok z pobranymi danymi dotycz¹cymi wybranej aukcji, przebiegiem licytacji oraz przyciskami umo¿liwiaj¹cymi licytacjê i powrót do poprzedniego widoku
- /Privacy – regulamin serwisu 

### Zabezpieczenia 
W aplikacji zastosowano nastêpuj¹ce zabezpieczenia chroni¹ce u¿ytkownika przed niespodziewanym i niepo¿¹danym zachowaniem aplikacji:
- Rejestracja u¿ytkownika wymaga uzupe³nienia wszystkich podanych w formularzu pól.
- U¿ytkownik mo¿e edytowaæ i usuwaæ tylko swoje aukcje.
- U¿ytkownik mo¿e edytowaæ i usuwaæ aukcje tylko przed ich zakoñczeniem.
- Edycja danych u¿ytkownika wymaga uzupe³nienia wszystkich podanych w formularzu pól.
- Wystawienie nowego przedmiotu wymaga uzupe³nienia wszystkich podanych w formularzu pól.
- U¿ytkownik nie mo¿e braæ udzia³u w aukcjach, które sam prowadzi.
- Lista aukcji dostêpnych dla u¿ytkownika przyjmuje tylko niezakoñczone licytacje.
- Cena wywo³awcza musi byæ wiêksza lub równa 1 z³.
- Usuniêcie u¿ytkownika powoduje usuniêcie równie¿ jego aukcji.
- U¿ytkownik nie mo¿e przelicytowaæ sam siebie. 

## Uruchomienie
Wymagania wstêpne: .NET SDK 8.0 lub nowszy 

### Kroki 
1. Skonfiguruj klienta API w pliku program.cs znajduj¹cym siê w czêœci frontendowej projektu: 
```
builder.Services.AddHttpClient("AuctionAPI", client => { client.BaseAddress = new Uri("https://localhost:****/api/");}); 
```

2. Dodaj wymagane pakiety NuGet w terminalu z poziomu katalogu, w którym znajduje siê plik *.csproj:
```
dotnet add package Microsoft.EntityFrameworkCore 
dotnet add package Microsoft.EntityFrameworkCore.Sqlite 
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

3. Uruchomienie testów wymaga osobnych pakietów. Dodaj je w terminalu z poziomu katalogu testowego *.Tests:
```
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Moq
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```
Uruchom testy z poziomu katalogu, w którym siê znajduj¹, czyli *.Tests:
```
dotnet test
```
## Przyk³adowe zapytanie
Przyk³adowe zapytanie dla endpointu POST/api/Users – dodanie u¿ytkownika.
```json
{
  "userID": 1,
  "firstName": "Jan",
  "lastName": "Kowalski",
  "login": "login",
  "password": "haslo",
  "isOver18": true,
  "acceptsRegulations": true,
  "auctions": [  ]
}
```

## TODO
Projekt mo¿na rozwin¹æ o sesjê logowania opart¹ o JWT.
