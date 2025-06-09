# Dokumentacja techniczna aplikacji internetowej CollectorsBay

## Wprowadzenie
**Opis projektu**: Celem tworzonego projektu jest stworzenie systemu aukcyjnego w architekturze REST z zachowaniem wzorca MVC, który pozwala na przeprowadzenie licytacji przedmiotów kolekcjonerskich w wybranej kategorii: ksi¹¿ki, monety i znaczki. 

**Wymagania funkcjonalne**:
- mo¿liwoœæ dodawania, edytowania, usuwania u¿ytkowników i pobierania informacji o danym u¿ytkowniku,
- u¿ytkownik mo¿e dodawaæ przedmioty do licytacji, edytowaæ i usuwaæ dodane przedmioty oraz pobieraæ informacje o przedmiotach wystawionych na aukcje,
- interfejs umo¿liwiaj¹cy przeprowadzenie licytacji w ró¿nych kategoriach.

**Wymagania techniczne**:
- zachowanie architektury REST i przestrzeganie modelu MVC,
- trwa³e przechowywanie danych.

**Technologie**:
- backend: ASP.NET Core REST Web API, Entity Framework, SQLite, 
- frontend: ASP.NET Razor Pages, Bootstrap
- komunikacja: HTTPClient

## Architektura systemu
Warstwa backendu: REST API obs³uguj¹ce logikê biznesow¹ i operacje na danych.
Warstwa frontendu: aplikacja ASP.NET Razor Pages wyœwietlaj¹ca dane i obs³uguj¹ca formularze.
Komunikacja: HTTPClient

Architektura systemu opiera siê na oddzieleniu warstwy prezentacji: frontendu od warstwy logiki biznesowej: backendu. Komunikacja miêdzy nimi odbywa siê za pomoc¹ protoko³u HTTP. 
Warstwa backendu (REST API) udostêpnia endpointy do obs³ugi operacji typu CRUD (tworzenie, odczyt, edycja, usuwanie) na danych. API realizuje logikê biznesow¹ i operuje na bazie danych.
Warstwa frontendu (Razor Pages) odpowiada za wyœwietlanie danych u¿ytkownikowi i obs³ugê interfejsu u¿ytkownika (formularze). 
¯eby pobraæ lub wys³aæ dane do backendu Razor Pages korzysta z klienta HTTP, który wysy³a zapytania HTTP (POST, PUT, DELETE, GET) do REST API i odbiera odpowiedzi w formacie JSON. 

## Symulacja licytacji
Symulacja licytacji polega na wylosowaniu jednej z 4 akcji okreœlaj¹cych przebieg aukcji za pomoc¹ prostego algorytmu wykorzystuj¹cego œredni¹ wa¿on¹ i element losowoœci. Scenariusze 1 i 2 pozwalaj¹ na kontunuowanie aukcji, scenariusze 3 i 4 koñcz¹ aukcjê. Losowanie trwa w pêtli do... while... a¿ do momentu wylosowania scenariusza 3 lub 4:
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
                    BiddingSteps.Add($"Podbi³eœ cenê. Nowa cena to: {Auction.CurrentPrice:F2} z³");
                    LatestUser = true;
                    break;
                case 2:
                    Auction.CurrentPrice = decimal.Round(Auction.CurrentPrice + (Auction.StartingPrice * 0.10m), 2);
                    BiddingSteps.Add($"Ktoœ inny podbi³ cenê. Nowa cena to: {Auction.CurrentPrice:F2} z³");
                    LatestUser = false;
                    break;
                case 3:
                    Auction.CurrentPrice = decimal.Round(Auction.CurrentPrice + (Auction.StartingPrice * 0.10m), 2);
                    BiddingSteps.Add($"Wygra³eœ licytacjê! Cena koñcowa to: {Auction.CurrentPrice:F2} z³");
                    Auction.IsAuctionOver = true;
                    break;
                case 4:
                    BiddingSteps.Add($"Przegra³eœ licytacjê!");
                    Auction.IsAuctionOver = true;
                    break;
            }
```
Losowanie scenariuszy od 1 do 4 z równym prawdopodobieñstwem mo¿e prowadziæ do szybkiego zakoñczenia losowania, poniewa¿ wtedy ka¿dy ze scenariuszy ma po 25% szans na wyst¹pienie, a wiêc tak prawdopodobieñstwo kontynuacji jak i zakoñczenia losowania wynosi po 50% . Do tego przy uwzglêdnieniu niemo¿liwoœci przelicytowania samego siebie czyli wylosowania dwa razy pod rz¹d scenariusza 1 prawdopodobieñstwo na zakoñczenie aukcji wzrasta do 66%. 
Dlatego, aby zapobiec zbyt szybkiemu zakoñczeniu licytacji, stosowana jest œrednia wa¿ona, w której scenariusz 1 ma 30% szans na wyst¹pienie, 2 – 40%, 3 – tylko 10%, a 4 – 20%. Poniewa¿ zastosowano warunek zabezpieczaj¹cy przed przelicytowaniem samego siebie, scenariusz 1 nie mo¿e wyst¹piæ dwa razy z rzêdu, co w przypadku losowania po uprzednim wylosowaniu 1 zwiêksza szansê na wyst¹pienie pozosta³ych scenariuszy: 2 – 57%, 3 – 14%, 4 – 29%. W tym przypadku najwiêksze prawdopodobieñstwo zakoñczenia aukcji to i tak tylko 43%, a to mniej ni¿ w przypadku losowania z równym prawdopodobieñstwem.
Cena przedmiotu wzrasta o 10% od ceny wywo³awczej z ka¿dym podbiciem ceny. Kroki licytacji zapisywane s¹ do listy BiddingSteps.

## Testowanie
Zastosowano dwa rodzaje testów: testy jednostkowe oraz testy manualne funkcjonalne.

**Testy Jednostkowe**: 
Zastosowane testy jednostkowe sk³adaj¹ siê z nastêpuj¹cych testów:
- test sprawdzaj¹cy, czy metoda POST kontrolera UsersController poprawnie dodaje u¿ytkownika do bazy danych i zwraca oczekiwan¹ odpowiedŸ HTTP (201) z poprawnymi danymi:
```C#
	[Fact]
	public async Task Post_AddsUser()
```
- test sprawdzaj¹cy, czy metoda GET zwraca u¿ytkownika, który istnieje w bazie danych  i zwraca oczekiwan¹ odpowiedŸ HTTP (200) z poprawnymi danymi:
```C#
	[Fact]
	public async Task GetUserById()
```
- test sprawdzaj¹cy, czy metoda GET zwraca u¿ytkownika, który nie istnieje w bazie danych i zwraca oczekiwan¹ odpowiedŸ HTTP (404 NotFound):
```C#
	[Fact]
	public async Task GetWhenUserIdDoesNoExist()
```

**Testy manualne (UI)**:
Raport z wybranych testów zakoñczonych powodzeniem:

| L.p. | Funkcja | Kroki testowe | Oczekiwany rezultat |
|------|---------|---------------|---------------------|
| 1 | Rejestracja nowego u¿ytkownika | WejdŸ na stronê rejestracji. Wype³nij wszystkie pola formularza. Kliknij "ZatwierdŸ". | Przeniesienie na widok konta u¿ytkownika. |
| 2 | Edycja profilu | Kliknij "Edytuj profil", zostaniesz przeniesiony/a na stronê formularza. Wype³nij wszystkie pola formularza. Kliknij "Zapisz zmiany". | Pojawia siê informacja "Profil zosta³ zaktualizowany". |
| 3 | Usuwanie u¿ytkownika | Kliknij przycisk "Usuñ konto", pojawi siê alert z pytaniem: "Czy na pewno chcesz usun¹æ konto?". Kliknij "Tak". | Przeniesienie na widok g³ówny z linkiem do formularza rejestracji. |
| 4 | Wystawianie przedmiotu | Kliknij "Wystaw przedmiot", zostaniesz przeniesiony/a na stronê formularza. Wype³nij wszystkie pola formularza. Kliknij "Dodaj". | Pojawia siê informacja "Przedmiot zosta³ wystawiony". |
| 5 | Licytacja | Kliknij "Licytuj". Powtarzaj do momentu zakoñczenia licytacji. | Pojawia siê informacja "Wygra³eœ licytacjê!" lub "Przegra³eœ licytacjê!". |
| 6 | Brak mo¿liwoœci usuniêcia przedmiotu, którego aukcja zosta³a zakoñczona | Na liœcie twoich przedmiotów do licytacji kliknij na ten, który chcesz edytowaæ lub usun¹æ. | Pojawia siê informacja "Aukcja zosta³a zakoñczona. Nie mo¿esz edytowaæ ani usun¹æ przedmiotu." |
| 7 | Wybór przedmiotów wed³ug kategorii | Kliknij na przycisk z nazw¹ wybranej kategorii aukcji: "Ksi¹¿ki", "Monety", "Znaczki". | Przeniesienie na widok listy z nazw¹ wybranej kategorii i nale¿¹cymi do niej przedmiotami. |
