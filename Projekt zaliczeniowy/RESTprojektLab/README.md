# Projekt: Aplikacja internetowa CollectorsBay

## Spis tre�ci
1. Lista i opis funkcjonalno�ci:
	* Metody HTTP
	* Widoki Razor Pages
	* Zabezpieczenia
2. Uruchomienie
	* Kroki
3. Przyk�adowe zapytanie

## Lista i opis funkcjonalno�ci
### Metody HTTP
Zaimplementowano nast�puj�ce metody: 
- POST/api/Auctions � dodanie przedmiotu do licytacji 
- GET/api/Auctions � pobieranie listy wszystkich aukcji 
- PUT/api/Autions/{id} � edycja przedmiotu do licytacji o podanym id 
- DELETE/api/Auctions/{id} � usuni�cie przedmiotu o podanym id z licytacji 
- GET/api/Auctions/{id} � pobranie aukcji o podanym id 
- GET/api/Auctions/user/{userId} � pobranie aukcji u�ytkownika o podanym id 
- POST/api/Users � dodanie u�ytkownika 
- PUT/api/Users/{id} � edycja danych u�ytkownika o podanym id 
- DELETE/api/Users/{id} � usuni�cie u�ytkownika o podanym id 
- GET/api/Users/{id} � pobranie danych u�ytkownika o podanym id 

### Widoki Razor Pages 
Zaimplementowano nast�puj�ce widoki:
- /Index � strona g��wna aplikacji z zak�adkami i linkiem do formularza rejestracji u�ytkownika
- /Register � rejestracja: formularz rejestracji u�ytkownika z obowi�zkowymi polami i przyciskiem do zatwierdzenia
- /Account � konto u�ytkownika z pobranymi danymi i list� wystawionych przedmiot�w oraz przyciskami umo�liwiaj�cymi przej�cie do udzia�u w licytacji, wystawienie przedmiotu, edycj� profilu i usuni�cie profilu u�ytkownika
- /SellerView � formularz do wystawienia nowego przedmiotu na aukcj� z obowi�zkowymi polami i przyciskiem do dodania nowego przedmiotu i powrotu do poprzedniego widoku
- /EditProfile � formularz edycji profilu z przyciskami zapisu zmian i powrotu do poprzedniego widoku
- /EditItem � formularz edycji danych przedmiotu z przyciskami zapisu zmian, usuni�cia przedmiotu i powrotu do poprzedniego widoku
- /AuctionCategories � wyb�r kategorii aukcji spo�r�d 3 mo�liwych: Ksi��ki, Monety, Znaczki
- /AuctionList � lista aukcji z filtracj�: wybrana kategoria, tylko niezako�czone aukcje i tylko takie, kt�rych w�a�cicielem jest inny u�ytkownik ni� u�ytkownik bior�cy udzia� w aukcji
- /AuctionDetails � widok z pobranymi danymi dotycz�cymi wybranej aukcji, przebiegiem licytacji oraz przyciskami umo�liwiaj�cymi licytacj� i powr�t do poprzedniego widoku
- /Privacy � regulamin serwisu 

### Zabezpieczenia 
W aplikacji zastosowano nast�puj�ce zabezpieczenia chroni�ce u�ytkownika przed niespodziewanym i niepo��danym zachowaniem aplikacji:
- Rejestracja u�ytkownika wymaga uzupe�nienia wszystkich podanych w formularzu p�l.
- U�ytkownik mo�e edytowa� i usuwa� tylko swoje aukcje.
- U�ytkownik mo�e edytowa� i usuwa� aukcje tylko przed ich zako�czeniem.
- Edycja danych u�ytkownika wymaga uzupe�nienia wszystkich podanych w formularzu p�l.
- Wystawienie nowego przedmiotu wymaga uzupe�nienia wszystkich podanych w formularzu p�l.
- U�ytkownik nie mo�e bra� udzia�u w aukcjach, kt�re sam prowadzi.
- Lista aukcji dost�pnych dla u�ytkownika przyjmuje tylko niezako�czone licytacje.
- Cena wywo�awcza musi by� wi�ksza lub r�wna 1 z�.
- Usuni�cie u�ytkownika powoduje usuni�cie r�wnie� jego aukcji.
- U�ytkownik nie mo�e przelicytowa� sam siebie. 

## Uruchomienie
Wymagania wst�pne: .NET SDK 8.0 lub nowszy 

### Kroki 
1. Skonfiguruj klienta API w pliku program.cs znajduj�cym si� w cz�ci frontendowej projektu: 
```
builder.Services.AddHttpClient("AuctionAPI", client => { client.BaseAddress = new Uri("https://localhost:****/api/");}); 
```

2. Dodaj wymagane pakiety NuGet w terminalu z poziomu katalogu, w kt�rym znajduje si� plik *.csproj:
```
dotnet add package Microsoft.EntityFrameworkCore 
dotnet add package Microsoft.EntityFrameworkCore.Sqlite 
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

3. Uruchomienie test�w wymaga osobnych pakiet�w. Dodaj je w terminalu z poziomu katalogu testowego *.Tests:
```
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Moq
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```
Uruchom testy z poziomu katalogu, w kt�rym si� znajduj�, czyli *.Tests:
```
dotnet test
```
## Przyk�adowe zapytanie
Przyk�adowe zapytanie dla endpointu POST/api/Users � dodanie u�ytkownika.
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
Projekt mo�na rozwin�� o sesj� logowania opart� o JWT.
