# Architektura Systemu: CatFact Integration Service

**Autor:** Jakub Filipiak
**Data:** Wrzesień 2026

Dokumentacja techniczna dla usługi integrującej dane z zewnętrznego API (CatFact) z systemem lokalnym oraz emulowanym środowiskiem chmurowym Microsoft Azure (Azurite). Projekt zaprojektowano z wykorzystaniem nowoczesnych wzorców inżynierii oprogramowania w ekosystemie .NET.

## Założenia Architektoniczne
Aplikacja została zaimplementowana w oparciu o szablon **.NET Worker Service**. Zapewnia to natywne wsparcie dla wstrzykiwania zależności (Dependency Injection), zarządzania cyklem życia aplikacji oraz logowania. Zewnętrzne API (`https://catfact.ninja/fact`) jest odpytywane ze stałą częstotliwością 1 zapytania na sekundę przy użyciu zoptymalizowanego mechanizmu `PeriodicTimer`.

## Wzorce Projektowe i Sieć
* **Clean Architecture**: Logika podzielona na warstwy `Core`, `Infrastructure` i `Worker`, z pełną separacją interfejsów i konfiguracją wyciągniętą do `appsettings.json`.
* **IHttpClientFactory**: Użyty w celu uniknięcia problemu wyczerpania gniazd sieciowych (socket exhaustion).
* **Polly (Resilience)**: Zaimplementowano politykę *Wait and Retry* obsługującą ewentualne problemy z dostępnością zewnętrznego endpointu.
* **Wydajne I/O**: Zapis do lokalnego pliku `.txt` oraz strumieniowanie do chmury realizowane jest całkowicie asynchronicznie, co redukuje narzut systemowy.

## Infrastruktura i Środowisko Lokalne (Docker & Azurite)
Aby zapewnić pełną hermetyczność środowiska deweloperskiego, zastosowano konteneryzację za pomocą `Docker` (wieloetapowy `Dockerfile`) oraz `docker-compose`. W ramach lokalnej chmury uruchamiany jest **Azurite** – oficjalny emulator Microsoft Azure Storage. Dzięki temu aplikacja może bezpiecznie symulować wysyłanie kopii zapasowych bezpośrednio do Azure Blob Storage za pomocą `AppendBlobClient`, bez konieczności podpinania rzeczywistej subskrypcji chmurowej przez rekrutera.

## Zapewnienie Jakości (QA) i CI/CD
* **Testy Jednostkowe**: Logika biznesowa i integracyjna (m.in. klient HTTP oraz Worker) została pokryta testami z użyciem **xUnit** oraz biblioteki **Moq** do tworzenia atrap interfejsów i symulowania odpowiedzi sieciowych w izolacji.
* **Continuous Integration (GitHub Actions)**: Wdrożono w pełni zautomatyzowany potok CI. Przy każdym zrzucie kodu na gałąź `main` GitHub automatycznie weryfikuje poprawność budowania solucji, uruchamia testy jednostkowe oraz testowo buduje finalny obraz Dockera.