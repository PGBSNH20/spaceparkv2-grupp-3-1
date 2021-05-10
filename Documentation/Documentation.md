<h1 align="center">Spaceport v2 Dokumentation</h3>

<h3>Innehållsförteckning</h3>

* [Specifikation för uppgift](#Specifikation)
* [Förord](#pre)
* [Process](#pro)
* [Docker Compose](#dcompose)
* [Kodstruktur](#kod)
* [API](#apitext)
* [Problem vi stött på](#problem)
* [Api dokumentation](#apidocs)
  * [Endpoint user](#endpointuser)
  * [Endpoint admin](#endpointadmin)
* [Publicerad dokumentation](#published)  

<a name="pre"/>

### Förord

Projektarbete **Spaceport v2**, det första projektarbetet för kursen Webbutveckling backend på Teknikhögskolan 2021

<a name="Specifikation"/>

### Specifikation för uppgift

* Spacepark v1 ska bli en webbapplikation med en API First strategi
* Krav för Spacepark v1 
  * Registrera parkeringar i en databas med EF Code First
  * En spaceport ska stängas när den är full
  * Queries mot databasen ska göras med EF fluent API
  * Enbart karaktärer som medverkat i Star Wars filmer får parkera
    * Validering genom Swapi
  * Enbart skepp som medverkat i Star Wars filmer får parkera
    * Validering genom Swapi
  * Alla anrop mot Swapi ska vara asynkrona
  * När en karaktär lämnar (unpark) så ska det betalas och sparas ett kvitto
  * Data från Swapi får ej sparas i cache minnet
* Krav för Spacepark v2
  * All logik ska nu finnas inom vårt Rest API
  * Enbart Rest API ska ha tillgång till databasen
  * Support för att lägga till flera spaceports
  * Två olika typer av användare
    * Användare (users)
      * Kan registrera en parkering (parkera)
      * Hämta information om pågående parkering
      * Avsluta parkering och betala
      * Hämta information om sin parkering historik
    * Administratör (admin)
      * Lägga till nya spaceports till systemet
* Skapa ett Rest API med ASP.NET Core 
* Alla utfall av applikationen ska kunna göras genom förfrågningar till API:et
  * Parkera
  * Avsluta parkering och få kvitto
  * Hämta parkerings histori
  * Validering av karaktär / skepp
  * Lägga till spaceports
* Alla förfrågningar ska ha en HTTP header med en API nyckel
  * Utvärdering av api nyckel ska ske genom ett **custom middleware**
    * Om godkänd nyckel skickas ska förfrågan godkännas
    * Om godkänd nyckel ej skickas så ska förfrågan avvisas
  * Inget krav på olika nycklar för olika typer av användare
* Databasen ska vara definierad i en docker-compose fil

<a name="pro"/>

### Process

Vi har inte valt att dela upp arbetet mellan oss, där en person tar en del och en annan tar något annat, av anledningen att vi båda vill vara med och ta del av alla de lärdomar som kommer genom pair programming. 

Vi började med att läsa igenom och diskutera uppgiften. Därefter började vi spåna på lite tankar hur vi skulle påbörja processen, och det vi började med var att ta fram ER diagram samt databas diagram, och precis som alltid så var vi båda överens om att det vi i början tar fram enbart är en grov skiss över det hela, och kan komma att ändras under projektet. Med det sagt fick vi till underlag som höll genom hela projektet. 

![first](img/1.png)

![second](img/2.png)

<a name="dcompose"/>

### Docker Compose

Vi har skapat en docker compose fil där vi kör databasen, och genom att ha den i en compose fil så innebär det bland annat att vi båda kan använda oss utav samma connection string, vilket är ett moment som vi har insett spar väldigt mycket tid i slutändan. Är det någonting man alltid glömmer att göra innan man pushar eller hämta en commit så är det att ändra connectionstring. 

<a name="kod"/>

### Kodstruktur

Vi började med att lägga lite tid på att diskutera kring hur vår kodstruktur skulle se ut. Vi vill att det ska vara enkelt och gå fort att kolla igenom våra mappar för att snabbt få en bild av vart de olika beståndsdelarna finns, och det vi kom fram till var följande 

* Controller
  * UserController
    * ```GET: /ParkingHistory```
    * ```GET: /ActiveParkings```
    * ```PUT: /Park```
    * ```PUT: /Unpark```
  * AdminController
    * ```GET: /GetAllSpaceports```
    * ```POST: /AddSpaceport```
    * ```POST: /AddParking```
    * ```DEL: /DeleteSpaceport```
    * ```DEL: /RemoveParking```
* Data
  * Innehåller allting som är relevant för databasen
* Interfaces
  * Innehåller alla interfaces vi skapat
* Middleware
  * Innehåller ApiKeyMiddleware.cs vilket är vår middleware för api nyckeln
* Migrations
* Models
* Requests
* Swapi

<a name="apitext"/>

### API

När vi kom till delen där vi skulle börja med vårat RestAPI så bestämde vi oss för att lägga någon dag på att läsa på mer om just detta ämne för att underlätta starten av det hela, och så här med facit i hand är vi väldigt nöjda med att vi inte bara stressade igång med att skriva massa kod utan att egentligen ha speciellt stor koll på hur allting fungerar. 

Den delen vi tyckte var krångligast att få till med hela RestAPI:et var att lösa authentication med en ApiKey, och att hantera detta via en middleware. Detta steg kändes väldigt oklart i början av projektet då vi aldrig hörttalas om middleware innan. 

***Vi har valt att inte använda oss av async / await eftersom det inte kommer att förbättra prestandan något i vårat fall. Det som async / await skulle kunna hjälpa till med är att öka skalbarheten, så man kan ta emot fler requests, men det är enbart sant om vi skulle använda oss av fler distanser av databasen, eller exempelvis mongoDB.***

<a name="problem"/>

### Problem vi stött på

Ett problem som vi stötte på längst vägen var ett error som uppstod när vi unparkade. 
När vi gör en unpark så kallar vi på en metod för att räkna ut det totala priset baserat på tidpunkt för park samt unpark och därefter skapa ett kvitto. Problemet som uppstod var att den försökte skapa upp ett kvitto med ett ID i databasen som redan existerar alltså gick det endast att göra en unpark, och därefter fick vi följande error 

![third](img/3.png)

Detta fel uppstod eftersom vi använde oss av AddSingleton för injicering av dependency. 
```csharp
services.AddSingleton<IReceipt, Receipt>();
```
Det som AddSingleton gör är att när den skapar upp en instans av Receipt så lever den instansen genom hela programmets körning, och då vi försöker göra flera unparks så kommer den alltså atta använda samma instans varje gång, vilket är grunden till problemet. 
Det vi gjorde för att lösa detta problem var att byta från att använda Singleton till att använda Scroped istället
```csharp
services.AddScoped<IReceipt, Receipt>();
```
Det Scoped gör är att istället för att låta samma instans leva genom en hel körning av programmet, så lever den bara genom en HTTP request, alltså löser det då vårat problem genom att varje gång vi kallar på metoden för att skapa ett kvitto så kommer det skapas upp en ny instans av ett kvitto som enbart lever under den HTTP Requesten, alltså får detta kvitto nu ett nytt ID i databasen. 

<a name="apidocs"/>

## API Dokumentation

Strukturen för vårat RestAPI ser ut som följer

* BaseUrl ```https://localhost:44531/api/```
* Resources
  * ```/user```
  * ```/admin```
* Endpoints for User resource
  * ```/ParkingHistory``` -- GET
  * ```/ActiveParkings/``` -- GET
  * ```/Park``` -- PUT
  * ```/Unpark``` -- PUT
* Endpoints for Admin resource
  * ```/AddSpaceport``` -- POST
  * ```/AddParking``` -- POST
  * ```/GetAllSpaceports``` -- GET
  * ```/DeleteSpaceport``` -- DEL
  * ```/RemoveParking``` -- DEL

<a name="endpointuser"/>

## Endpoints for the user resource
 
```GET``` ```/User/ParkingHistory``` -- Hämtar alla kvitton över parkeringshistorik för vald person

Example Request 
```https://localhost:44531/api/user/parkinghistory```

<details>
 <summary>Header</summary>
  
 | KEY | VALUE | 
| --------------- | --------------- | 
| ApiKey | verystrongapikey |
 
</details>

<details>
<summary>Body</summary>
 
 ```"Han Solo"```
</details>

<details>
 <summary>Example Response</summary>

```json
[
    {
        "id": 19,
        "size": null,
        "sizeId": 4,
        "name": "Darth Vader",
        "starshipName": "Death star",
        "arrival": "2021-05-07T16:21:22.8141681",
        "departure": "2021-05-07T16:21:33.6418199",
        "totalAmount": 6000
    }
]
```
</details>

---

```GET``` ```/User/ActiveParkings``` -- Hämtar alla parkeringar där vald karaktär står parkerad

Example Request 
```https://localhost:44531/api/user/activeparkings```

<details>
 <summary>Header</summary>
  
 | KEY | VALUE | 
| --------------- | --------------- | 
| ApiKey | verystrongapikey |
 
</details>

<details>
<summary>Body</summary>
 
 ```"Darth vader"```
</details>

<details>
 <summary>Example Response</summary>

```json
[
    {
        "id": 22,
        "size": null,
        "sizeId": 4,
        "spacePortId": 2,
        "characterName": "Darth Vader",
        "spaceshipName": "Death star",
        "arrival": "2021-05-07T17:54:08.3012599"
    }
]
```
</details>

---

```PUT``` ```/User/Park/22``` -- Parkerar ett skepp på valfri spaceport

Example Request 
```https://localhost:44531/api/user/park```

<details>
 <summary>Header</summary>
  
 | KEY | VALUE | 
| --------------- | --------------- | 
| ApiKey | verystrongapikey |
 
</details>

<details>
<summary>Body</summary>
 
 ```json
{
    "personName": "Darth Vader",
    "shipName": "Death star"
}
 ```
</details>

<details>
 <summary>Example Response</summary>

```200 OK``` - ```Vehicle parked.```  
```400 Bad Request``` - ```No suitable parkings for your ship length.```  
```401 Unauthorized``` - ```Invalid character or ship.```
</details>

---

```PUT``` ```/User/Unpark/1``` -- Parkerar ett skepp på valfri spaceport

Example Request 
```https://localhost:44531/api/user/unpark/22```

<details>
 <summary>Header</summary>
  
 | KEY | VALUE | 
| --------------- | --------------- | 
| ApiKey | verystrongapikey |
 
</details>

<details>
<summary>Body</summary>
 
 ```json
{
    "personName": "Darth Vader",
    "shipName": "Death star"
}
 ```
</details>

<details>
 <summary>Example Response</summary>

```200 OK``` - ```Vehicle unparked.```  
```400 Bad Request``` - ```Incorrect name, ship or parking spot id.```  
</details>

---

<a name="endpointadmin"/>

## Endpoints for the admin resource

---

```GET``` ```/Admin/GetAllSpaceports``` -- Visar alla spaceports

Example Request 
```https://localhost:44531/api/admin/getallspaceports```

<details>
 <summary>Header</summary>
  
 | KEY | VALUE | 
| --------------- | --------------- | 
| ApiKey | verystrongapikey |
 
</details>

<details>
<summary>Body</summary>
</details>

<details>
 <summary>Example Response</summary>

```[
    {
        "id": 2,
        "name": "Spacepark Deluxe1",
        "parkings": [
            {
                "id": 12,
                "size": {
                    "id": 1,
                    "type": 1,
                    "receipts": null
                },
                "sizeId": 1,
                "spacePortId": 2,
                "characterName": null,
                "spaceshipName": null,
                "arrival": null
            },
            {
                "id": 13,
                "size": {
                    "id": 1,
                    "type": 1,
                    "receipts": null
                },
                "sizeId": 1,
                "spacePortId": 2,
                "characterName": null,
                "spaceshipName": null,
                "arrival": null
            },
            {
                "id": 14,
                "size": {
                    "id": 1,
                    "type": 1,
                    "receipts": null
                },
                "sizeId": 1,
                "spacePortId": 2,
                "characterName": null,
                "spaceshipName": null,
                "arrival": null
            },
            {
                "id": 15,
                "size": {
                    "id": 1,
                    "type": 1,
                    "receipts": null
                },
                "sizeId": 1,
                "spacePortId": 2,
                "characterName": null,
                "spaceshipName": null,
                "arrival": null
            },
            {
                "id": 16,
                "size": {
                    "id": 1,
                    "type": 1,
                    "receipts": null
                },
                "sizeId": 1,
                "spacePortId": 2,
                "characterName": null,
                "spaceshipName": null,
                "arrival": null
            },
            {
                "id": 17,
                "size": {
                    "id": 2,
                    "type": 2,
                    "receipts": null
                },
                "sizeId": 2,
                "spacePortId": 2,
                "characterName": null,
                "spaceshipName": null,
                "arrival": null
            },
            {
                "id": 18,
                "size": {
                    "id": 2,
                    "type": 2,
                    "receipts": null
                },
                "sizeId": 2,
                "spacePortId": 2,
                "characterName": null,
                "spaceshipName": null,
                "arrival": null
            },
            {
                "id": 19,
                "size": {
                    "id": 2,
                    "type": 2,
                    "receipts": null
                },
                "sizeId": 2,
                "spacePortId": 2,
                "characterName": null,
                "spaceshipName": null,
                "arrival": null
            },
            {
                "id": 20,
                "size": {
                    "id": 3,
                    "type": 3,
                    "receipts": null
                },
                "sizeId": 3,
                "spacePortId": 2,
                "characterName": null,
                "spaceshipName": null,
                "arrival": null
            },
            {
                "id": 21,
                "size": {
                    "id": 3,
                    "type": 3,
                    "receipts": null
                },
                "sizeId": 3,
                "spacePortId": 2,
                "characterName": null,
                "spaceshipName": null,
                "arrival": null
            },
            {
                "id": 22,
                "size": {
                    "id": 4,
                    "type": 4,
                    "receipts": null
                },
                "sizeId": 4,
                "spacePortId": 2,
                "characterName": "Darth Vader",
                "spaceshipName": "Death star",
                "arrival": "2021-05-07T17:54:08.3012599"
            }
        ]
    }
]
```  
</details>

---

```POST``` ```/Admin/AddSpaceport``` -- Skapa en ny spaceport

Example Request 
```https://localhost:44531/api/admin/addspaceport```

<details>
 <summary>Header</summary>
  
 | KEY | VALUE | 
| --------------- | --------------- | 
| ApiKey | verystrongapikey |
 
</details>

<details>
<summary>Body</summary>
 
 ```
 "Spaceport evilport"
 ```
</details>

<details>
 <summary>Example Response</summary>

```200 OK``` - ```SpacePort added id: 3 name: Spaceport evilport```  
</details>

---

```POST``` ```/Admin/AddParking``` -- Skapa en ny spaceport

Example Request 
```https://localhost:44531/api/admin/addparking```

<details>
 <summary>Header</summary>
  
 | KEY | VALUE | 
| --------------- | --------------- | 
| ApiKey | verystrongapikey |
 
</details>

<details>
<summary>Body</summary>
 
 ```json
{
  "id": 3,
  "size": {
    "id": 0,
    "type": 1,
    "receipts": [
      {
        "id": 0,
        "sizeId": 0,
        "name": "string",
        "starshipName": "string",
        "arrival": "2021-05-07T18:43:47.126Z",
        "departure": "2021-05-07T18:43:47.126Z",
        "totalAmount": 0
      }
    ]
  },
  "sizeId": 0,
  "spacePortId": 0,
  "characterName": "string",
  "spaceshipName": "string",
  "arrival": "2021-05-07T18:43:47.126Z"
}
 ```
</details>

<details>
 <summary>Example Response</summary>

```200 OK``` - ```Parking Added to Database.```  
```409 Conflict``` - ```An error occurred while adding new parking spot.```
</details>

---

```DEL``` ```/Admin/DeleteSpaceport/1``` -- Ta bort en spaceport

Example Request 
```https://localhost:44531/api/admin/deletespaceport/1```

<details>
 <summary>Header</summary>
  
 | KEY | VALUE | 
| --------------- | --------------- | 
| ApiKey | verystrongapikey |
 
</details>

<details>
<summary>Body</summary>
 

</details>

<details>
 <summary>Example Response</summary>

```202 Accepted``` - ```Space port deleted.```  
```400 Bad Request``` - ```Space port was not found.```
</details>

---

```DEL``` ```/Admin/RemoveParking``` -- Ta bort en parkeringsplats

Example Request 
```https://localhost:44531/api/admin/removeparking/1```

<details>
 <summary>Header</summary>
  
 | KEY | VALUE | 
| --------------- | --------------- | 
| ApiKey | verystrongapikey |
 
</details>

<details>
<summary>Body</summary>
 

</details>

<details>
 <summary>Example Response</summary>

```200 OK``` - ```Parking with id:27 was removed successfully.```  
```400 Bad Request``` - ```Parking with id:1 was not found.```
</details>

<a name="published"/>

### Publicerad dokumentation

Dokumentationen finns även publicerad [här](https://documenter.getpostman.com/view/15582637/TzRPjpe3) och går där att öppna direkt i postman.
