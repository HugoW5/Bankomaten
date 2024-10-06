# Bankomaten
Det här programmet simulerar en bank/bankomat. Programmet hanterar inloggning, kontosaldo, överföringar och uttag av pengar.


## Projektstruktur
**Metoder**  
`NavigationMenu()` Visar huvudmenyn och låter användaren navigera i programmet.  
`Login()` Hanterar inloggningen, använder sig utav `Authenticate()` för att autentisera användarnamn och lösenord.  
`Withdraw()` Låter anändaren ta ut pengar, använder sig utav `Authenticate()` och `CalculateBanknotes()`.  
`Transaction()` Låter användaren skicka pengar mellan sina egna konton.  
`AccountsOverview()` Visar en överblick över användarens konton med saldon och bankkonto nummer.  
`Logout()` Loggar ut användaren och återgår till inloggings sidan.  
`CalculateBanknotes()` Beräknar uttagsbeloppet i kontanter.  
`Authenticate()` Autentisera användarnamn och lösenord.  
`PrintMessage()` Skriver ut text i färg som tas genom parametrar.  
`ReadLineWithCancel()` Fungerar som en ReadLine men kan lyssna efter specifika knapp tryck. Tex ESC avbryter & går till huvudmenyn

### Klass variabler
| Namn| Datatyp| Förklaring|
|-|-|-|
| `Usernames`           | `string[]`| En array som lagrar fördefinierade användarnamn för inloggning|
| `Passwords`           | `string[]`| En array som lagrar lösenord för användarna i `Usernames`|
| `BankAccountNames`    | `string[,]`| En 2d array som innehåller namnen på olika konton (t.ex. Lönekonto, Sparkonto) för varje användare. |
| `BankAccountBalances` | `double[,]`| En 2d array som lagrar saldot på användarnas konton|
| `currentUserIndex`    | `int`| En variabel som lagrar indexet för den inloggade användaren i `Usernames`|

## Reflektion
Jag strukturerade programmet med olika metoder för varje fuktionalitet som till exempel `Authenticate()`, `Transaction()` och `ReadLineWithCancel()` för att ha återanvändningsbar kod och hålla koden organiserad, koden blir även mer lättläst och enklare att följa med i. Jag anser att detta är det bästa sättet att skapa det här projektet på baserat på förutsättningarna. Jag har även valt att begränsa mängden kod i Main metoden och istället fokusera på att metoderna kallar på varandra, det gör att programflödet blir enklare att följa.  

Jag hade kunnat göra färre metoder och skriva all logik i större metoder istället för att bryta ner programmet i mindre metoder. Däremot så hade det varit mycket sämre eftersom det hade gjort koden mer svårläst och svårare att felsöka. 

Ett exempel på en förbättring kan vara att om jag hade använt jagged arrayer `[][]` istället  2d arrayer `[,]` till att spara `BankAccountNames` och `BankAccountBalances` så hade programmet kunnat vara mer skalbart eftersom alla platser jagged arrayer inte behöver vara lika stora. Då kan ett användarkonto till exempel ha 5 olika bankkonton medans ett annat bara har 1 eller 2.
