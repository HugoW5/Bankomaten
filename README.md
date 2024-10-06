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


