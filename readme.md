# esoteric-finance

Self-Contained Finance Management Web App

# esoteric-finance-abstractions

Types and Abstractions for esoteric-finance

# esoteric-finance-api

.NET 6 Backend Api

## Entity Framework

- Run `dotnet tool install --global dotnet-ef`
- To generate new migration, run `dotnet ef migrations add {MIGRATION_NAME} --startup-project esoteric-finance-api --project esoteric-finance-data`
- To update the database programattically, run `dotnet ef database update --startup-project esoteric-finance-api --project esoteric-finance-data`
- To recreate, delete `%APPDATA%/local/esoteric-finance.db`, delete the esoteric-finance-data/Migrations folder and rerun the previous two commands
- To generate idemptotent script for update, run `dotnet ef migrations script -i --startup-project esoteric-finance-api --project esoteric-finance-data -o migration.sql` 

## Testing

A sample request to create a transaction, HTTP PUT `{{url}}/transaction`
``` json
{
  "request": {
    "id": 0,
    "transactionDate": "2022-09-05T15:48:38.855Z",
    "postedDate": "2022-09-07T00:00:00.000Z",
    "initiator": {
      "id": 0,
      "name": "Account Owner"
    },
    "recipient": {
      "id": 0,
      "name": "Walmart"
    },
    "method": {
      "id": 0,
      "name": "Cash",
    },
    "details": [
      {
        "id": 0,
        "description": "Groceries",
        "multiplier": 0.80,
        "category": {
          "id": 0,
          "name": "Food"
        }
      },
      {
        "id": 0,
        "description": "Snack",
        "multiplier": 0.20,
        "category": {
          "id": 0,
          "name": "Food"
        }
      }
    ]
  }
}
```

# esoteric-finance-app

VueJs SPA

# esoteric-finance-data

Entity Framework Core 6 

Schema

- Tables
    - GeneralLog
    - Category
    - Detail
    - Initiator
    - Method
    - Recipient
    - Transaction
- Relationships
    - Detail(CategoryId) M-1 Category(DetailId)
    - Transaction(DetailId) 1-1 Detail(TransactionId)
    - Transaction(InitiatorId) M-1 Initiator
    - Transaction(MethodId) M-1 Method
    - Transaction(RecipientId) M-1 Recipient

# esoteric-finance-services

Supplemental Services

- Spreadsheet Import/Export