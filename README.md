**Treść zadania rekrutacyjnego:**

"Stwórz REST API, które będzie modułem biblioteki (wypożyczalni książek) odpowiedzialnym za zarządzanie książkami.

Każda książka posiada: tytuł, autora, ISBN (unikalny) oraz status.

Możliwe statusy:

- Na półce          – może zostać ustawiony tylko ze Zwrócona lub Uszkodzona

- Wypożyczona  – może zostać ustawiony tylko z Na półce

- Zwrócona        – może zostać ustawiony tylko z Wypożyczona

                             (książka została zwrócona, ale jeszcze nie znalazła się fizycznie na półce)

- Uszkodzona     – specjalny status, który może zostać ustawiony tylko ze statusu Na półce lub Zwrócona

                   (książki uszkodzonej nie można wypożyczyć)

 

Na potrzeby zadania nie przejmujemy się tym, kto książkę wypożyczył.

Konsument API musi mieć możliwość wykonywania operacji CRUD oraz możliwość sortowania i stronicowania listy książek."

**Uwagi:**

Do wykonania zadania wykorzystany został .Net 8, WebApi, EF in-memory, MediatR, FluentValidation oraz Swagger.

Zastosowane zostały wzorce projektowe CQRS, oraz Mediator.

Dodatkowo zostały dodane testy jednostkowe kontrolera, oraz commands/queries z wykorzystaniem NUnit oraz NSubstitute.
