# Dokumentacja techniczna aplikacji internetowej CollectorsBay

## Wprowadzenie
**Opis projektu**: Celem tworzonego projektu jest stworzenie systemu aukcyjnego w architekturze REST z zachowaniem wzorca MVC, kt�ry pozwala na przeprowadzenie licytacji przedmiot�w kolekcjonerskich w wybranej kategorii: ksi��ki, monety i znaczki. 

**Wymagania funkcjonalne**:
- mo�liwo�� dodawania, edytowania, usuwania u�ytkownik�w i pobierania informacji o danym u�ytkowniku,
- u�ytkownik mo�e dodawa� przedmioty do licytacji, edytowa� i usuwa� dodane przedmioty oraz pobiera� informacje o przedmiotach wystawionych na aukcje,
- interfejs umo�liwiaj�cy przeprowadzenie licytacji w r�nych kategoriach.

**Wymagania techniczne**:
- zachowanie architektury REST i przestrzeganie modelu MVC,
- trwa�e przechowywanie danych.

**Technologie**:
- backend: ASP.NET Core REST Web API, Entity Framework, SQLite, 
- frontend: ASP.NET Razor Pages, Bootstrap
- komunikacja: HTTPClient

## Architektura systemu
Warstwa backendu: REST API obs�uguj�ce logik� biznesow� i operacje na danych.
Warstwa frontendu: aplikacja ASP.NET Razor Pages wy�wietlaj�ca dane i obs�uguj�ca formularze.
Komunikacja: HTTPClient

Architektura systemu opiera si� na oddzieleniu warstwy prezentacji: frontendu od warstwy logiki biznesowej: backendu. Komunikacja mi�dzy nimi odbywa si� za pomoc� protoko�u HTTP. 
Warstwa backendu (REST API) udost�pnia endpointy do obs�ugi operacji typu CRUD (tworzenie, odczyt, edycja, usuwanie) na danych. API realizuje logik� biznesow� i operuje na bazie danych.
Warstwa frontendu (Razor Pages) odpowiada za wy�wietlanie danych u�ytkownikowi i obs�ug� interfejsu u�ytkownika (formularze). 
�eby pobra� lub wys�a� dane do backendu Razor Pages korzysta z klienta HTTP, kt�ry wysy�a zapytania HTTP (POST, PUT, DELETE, GET) do REST API i odbiera odpowiedzi w formacie JSON. 

## Symulacja licytacji
Symulacja licytacji polega na wylosowaniu jednej z 4 akcji okre�laj�cych przebieg aukcji za pomoc� prostego algorytmu wykorzystuj�cego �redni� wa�on� i element losowo�ci. Scenariusze 1 i 2 pozwalaj� na kontunuowanie aukcji, scenariusze 3 i 4 ko�cz� aukcj�. Losowanie trwa w p�tli do... while... a� do momentu wylosowania scenariusza 3 lub 4:
```C#
            var random = new Random();
            int scenario;
            int[] weightedScenarios = { 1, 1, 1, 2, 2, 2, 2, 3, 4, 4 };
            do
            {
                scenario = weightedScenarios[random.Next(weightedScenarios.Length)];
            }
            while (scenario == 1 && LatestUser == true);   

            switch (scenario)
            {
                case 1:
                    Auction.CurrentPrice = decimal.Round(Auction.CurrentPrice + (Auction.StartingPrice * 0.10m), 2);
                    BiddingSteps.Add($"Podbi�e� cen�. Nowa cena to: {Auction.CurrentPrice:F2} z�");
                    LatestUser = true;
                    break;
                case 2:
                    Auction.CurrentPrice = decimal.Round(Auction.CurrentPrice + (Auction.StartingPrice * 0.10m), 2);
                    BiddingSteps.Add($"Kto� inny podbi� cen�. Nowa cena to: {Auction.CurrentPrice:F2} z�");
                    LatestUser = false;
                    break;
                case 3:
                    Auction.CurrentPrice = decimal.Round(Auction.CurrentPrice + (Auction.StartingPrice * 0.10m), 2);
                    BiddingSteps.Add($"Wygra�e� licytacj�! Cena ko�cowa to: {Auction.CurrentPrice:F2} z�");
                    Auction.IsAuctionOver = true;
                    break;
                case 4:
                    BiddingSteps.Add($"Przegra�e� licytacj�!");
                    Auction.IsAuctionOver = true;
                    break;
            }
```
Losowanie scenariuszy od 1 do 4 z r�wnym prawdopodobie�stwem mo�e prowadzi� do szybkiego zako�czenia losowania, poniewa� wtedy ka�dy ze scenariuszy ma po 25% szans na wyst�pienie, a wi�c tak prawdopodobie�stwo kontynuacji jak i zako�czenia losowania wynosi po 50% . Do tego przy uwzgl�dnieniu niemo�liwo�ci przelicytowania samego siebie czyli wylosowania dwa razy pod rz�d scenariusza 1 prawdopodobie�stwo na zako�czenie aukcji wzrasta do 66%. 
Dlatego, aby zapobiec zbyt szybkiemu zako�czeniu licytacji, stosowana jest �rednia wa�ona, w kt�rej scenariusz 1 ma 30% szans na wyst�pienie, 2 � 40%, 3 � tylko 10%, a 4 � 20%. Poniewa� zastosowano warunek zabezpieczaj�cy przed przelicytowaniem samego siebie, scenariusz 1 nie mo�e wyst�pi� dwa razy z rz�du, co w przypadku losowania po uprzednim wylosowaniu 1 zwi�ksza szans� na wyst�pienie pozosta�ych scenariuszy: 2 � 57%, 3 � 14%, 4 � 29%. W tym przypadku najwi�ksze prawdopodobie�stwo zako�czenia aukcji to i tak tylko 43%, a to mniej ni� w przypadku losowania z r�wnym prawdopodobie�stwem.
Cena przedmiotu wzrasta o 10% od ceny wywo�awczej z ka�dym podbiciem ceny. Kroki licytacji zapisywane s� do listy BiddingSteps.

## Testowanie
Zastosowano dwa rodzaje test�w: testy jednostkowe oraz testy manualne funkcjonalne.

**Testy Jednostkowe**: 
Zastosowane testy jednostkowe sk�adaj� si� z nast�puj�cych test�w:
- test sprawdzaj�cy, czy metoda POST kontrolera UsersController poprawnie dodaje u�ytkownika do bazy danych i zwraca oczekiwan� odpowied� HTTP (201) z poprawnymi danymi:
```C#
	[Fact]
	public async Task Post_AddsUser()
```
- test sprawdzaj�cy, czy metoda GET zwraca u�ytkownika, kt�ry istnieje w bazie danych  i zwraca oczekiwan� odpowied� HTTP (200) z poprawnymi danymi:
```C#
	[Fact]
	public async Task GetUserById()
```
- test sprawdzaj�cy, czy metoda GET zwraca u�ytkownika, kt�ry nie istnieje w bazie danych i zwraca oczekiwan� odpowied� HTTP (404 NotFound):
```C#
	[Fact]
	public async Task GetWhenUserIdDoesNoExist()
```

**Testy manualne (UI)**:
Raport z wybranych test�w zako�czonych powodzeniem:

| L.p. | Funkcja | Kroki testowe | Oczekiwany rezultat |
|------|---------|---------------|---------------------|
| 1 | Rejestracja nowego u�ytkownika | Wejd� na stron� rejestracji. Wype�nij wszystkie pola formularza. Kliknij "Zatwierd�". | Przeniesienie na widok konta u�ytkownika. |
| 2 | Edycja profilu | Kliknij "Edytuj profil", zostaniesz przeniesiony/a na stron� formularza. Wype�nij wszystkie pola formularza. Kliknij "Zapisz zmiany". | Pojawia si� informacja "Profil zosta� zaktualizowany". |
| 3 | Usuwanie u�ytkownika | Kliknij przycisk "Usu� konto", pojawi si� alert z pytaniem: "Czy na pewno chcesz usun�� konto?". Kliknij "Tak". | Przeniesienie na widok g��wny z linkiem do formularza rejestracji. |
| 4 | Wystawianie przedmiotu | Kliknij "Wystaw przedmiot", zostaniesz przeniesiony/a na stron� formularza. Wype�nij wszystkie pola formularza. Kliknij "Dodaj". | Pojawia si� informacja "Przedmiot zosta� wystawiony". |
| 5 | Licytacja | Kliknij "Licytuj". Powtarzaj do momentu zako�czenia licytacji. | Pojawia si� informacja "Wygra�e� licytacj�!" lub "Przegra�e� licytacj�!". |
| 6 | Brak mo�liwo�ci usuni�cia przedmiotu, kt�rego aukcja zosta�a zako�czona | Na li�cie twoich przedmiot�w do licytacji kliknij na ten, kt�ry chcesz edytowa� lub usun��. | Pojawia si� informacja "Aukcja zosta�a zako�czona. Nie mo�esz edytowa� ani usun�� przedmiotu." |
| 7 | Wyb�r przedmiot�w wed�ug kategorii | Kliknij na przycisk z nazw� wybranej kategorii aukcji: "Ksi��ki", "Monety", "Znaczki". | Przeniesienie na widok listy z nazw� wybranej kategorii i nale��cymi do niej przedmiotami. |
