# MT103 Swift Message Parser API

## Overview
ASP.NET Web API that parses SWIFT MT103 messages into a readable format and saves them in a SQLite database.

## Features:
- Upload and parse MT103 Swift messages (custom parser, no parsing library used)
- Save readable message data in the database with ADO.NET (no EF Core)
- Fully documented REST API using Swagger
- Logging with NLog

## Tech Stack
- .NET 10
- ASP.NET Core Web API (controllers)
- NLog
- SQLite (local)

## How to Run
1. Clone repository
2. `dotnet run`
3. Open https://localhost:7283/swagger

## API Endpoints
- POST /api/MT103/upload - Upload MT103 file
- GET /api/MT103 - Get all messages
- GET /api/MT103/{id} - Get message by ID

## Example request and responses

### Uploading and parsing `/api/MT103/upload`:
Example body: (content of .txt file)
```
{1:F21PRCBBGSFAXXX2082167565}{4:{177:1602161334}{451:0}}{1:F01PRCBBGSFAXXX2082167565}{2:I103COBADEFFXXXXN}{3:{119:STP}}{4:
:20:160216270075956
:23B:CRED
:32A:160217EUR540,00
:33B:EUR540,00
:50K:/BG95RZBB91556261234567
OKO 1000 OOD
TZAR IVAN SHISHMAN ? 11
SOFIA, BULGARIA
:57A:INGDDEFFXXX
:59:/DE83500105172667123456
FRANCA CEVALES
MUNCHENER STR. 35, GERMANY
:70:ACCOMODATION 11-11.02.16  INVOICE
027/2016
:71A:SHA
-}{5:{MAC:00000000}{CHK:6BC2D5BE9937}}
```

Example response:
```
{
  "message": "Successfully saved a MT103 Swift message with reference id: 160216270075956",
  "data": {
    "id": 10,
    "referenceNumber": "160216270075956",
    "bankOperationCode": "CRED",
    "valueDate": "2016-02-17T00:00:00",
    "currency": "EUR",
    "interbankSettled": 540,
    "orderingCustomer": "/BG95RZBB91556261794271\nOKO 1000 OOD\nTZAR IVAN SHISHMAN ? 11\nSOFIA, BULGARIA",
    "accountWithInstitution": "INGDDEFFXXX",
    "recepient": "/DE83500105172667785918\nFRANCA CEVALES\nMUNCHENER STR. 35, GERMANY",
    "remittanceInformation": "ACCOMODATION 11-11.02.16  INVOICE\n027/2016",
    "detailsOfCharge": "SHA"
  }
}
```

### Retrieving all MT103 messages `/api/MT103`:
Example response:
```
[
  {
    "id": 9,
    "referenceNumber": "160216270075956",
    "bankOperationCode": "CRED",
    "valueDate": "2016-02-17T00:00:00",
    "currency": "EUR",
    "interbankSettled": 540,
    "orderingCustomer": "/BG95RZBB91556261794271\nOKO 1000 OOD\nTZAR IVAN SHISHMAN ? 11\nSOFIA, BULGARIA",
    "accountWithInstitution": "INGDDEFFXXX",
    "recepient": "/DE83500105172667785918\nFRANCA CEVALES\nMUNCHENER STR. 35, GERMANY",
    "remittanceInformation": "ACCOMODATION 11-11.02.16  INVOICE\n027/2016",
    "detailsOfCharge": "SHA"
  },
  {
    "id": 10,
    "referenceNumber": "160216270075956",
    "bankOperationCode": "CRED",
    "valueDate": "2016-02-17T00:00:00",
    "currency": "EUR",
    "interbankSettled": 540,
    "orderingCustomer": "/BG95RZBB91556261794271\nOKO 1000 OOD\nTZAR IVAN SHISHMAN ? 11\nSOFIA, BULGARIA",
    "accountWithInstitution": "INGDDEFFXXX",
    "recepient": "/DE83500105172667785918\nFRANCA CEVALES",
    "remittanceInformation": "ACCOMODATION 11-11.02.16  INVOICE\n027/2016",
    "detailsOfCharge": "SHA"
  }
]
```

## Articles / Research:
https://wise.com/us/blog/what-is-mt103-swift and AI to understand what MT103 is and how it works
