Det här programmet simulerar en bank/bankomat. Programmet hanterar inloggning, kontosaldo, överföringar och uttag av pengar.


## Klass variabler
| Namn| Datatyp| Förklaring|
|-|-|-|
| `Usernames`           | `string[]`| En array som lagrar fördefinierade användarnamn för inloggning|
| `Passwords`           | `string[]`| En array som lagrar lösenord för användarna i `Usernames`|
| `BankAccountNames`    | `string[,]`| En 2d array som innehåller namnen på olika konton (t.ex. Lönekonto, Sparkonto) för varje användare. |
| `BankAccountBalances` | `double[,]`| En 2d array som lagrar saldot på användarnas konton|
| `currentUserIndex`    | `int`| En variabel som lagrar indexet för den inloggade användaren i `Usernames`|


