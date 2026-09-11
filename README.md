# CatFact Integration Service

[![CI Pipeline](https://github.com/JKBFilip/netwisetask/actions/workflows/ci.yml/badge.svg)](https://github.com/TWOJ_NICK/NAZWA_REPOZYTORIUM/actions/workflows/ci.yml)

Profesjonalna usługa w tle (Worker Service) integrująca dane z zewnętrznego REST API z lokalnym systemem plików oraz chmurą Microsoft Azure. Projekt zrealizowany w oparciu o zasady Clean Architecture oraz wzorce projektowe dla aplikacji chmurowych.

**Technologie:** C# (.NET 8), Docker, Azure Blob Storage (Azurite), Polly, xUnit, Moq, Dependency Injection.

## Funkcjonalności
* Asynchroniczne odpytywanie zewnętrznego endpointu (`https://catfact.ninja/fact`) z użyciem zoptymalizowanego mechanizmu `PeriodicTimer` (1 zapytanie/sekundę).
* **Resilience & Transient-Fault-Handling:** Wykorzystanie biblioteki Polly do obsługi ewentualnych braków odpowiedzi z API (polityka Wait and Retry).
* **Zapis Lokalny:** Wydajny zapis pobranych danych do lokalnego pliku `.txt` na dysku.
* **Integracja z Azure:** Automatyczny, strumieniowy backup każdego pobranego faktu do Azure Blob Storage przy użyciu `AppendBlobClient`.
* **Lokalna Chmura:** Wykorzystanie emulatora Azurite w środowisku kontenerowym, co pozwala na pełne testowanie integracji chmurowej bez potrzeby podpinania płatnej subskrypcji.
* **Automatyzacja CI/CD i Testy:** Pokrycie testami jednostkowymi (xUnit, Moq) sprawdzane automatycznie przez potok GitHub Actions, który weryfikuje również poprawność budowania obrazów Dockerowych.

## Architektura i Wzorce
Projekt został podzielony na trzy warstwy w duchu Separation of Concerns:
1. **Core:** Modele danych i interfejsy. Całkowity brak zewnętrznych zależności.
2. **Infrastructure:** Logika połączeń sieciowych (klient HTTP) oraz I/O (zapis na dysk, integracja z Azure Blob).
3. **Worker:** Punkt wejścia aplikacji, konfiguracja kontenera IoC i orkiestracja zadań.

Dodatkowo w folderze `/docs` znajduje się sformalizowany raport architektoniczny w formacie PDF (wygenerowany w systemie LaTeX).

## Uruchomienie projektu (Docker)

Aplikacja oraz emulowane środowisko chmurowe zostały w pełni skonteneryzowane. Do uruchomienia projektu wymagany jest jedynie Docker.

1. Sklonuj repozytorium.
2. W głównym katalogu projektu wykonaj polecenie:
   ```bash
   docker-compose up --build -d
   ```
3. Aplikacja rozpocznie działanie w tle. Pobrane fakty o kotach będą pojawiać się w katalogu output/catfacts.txt na Twoim hoście.