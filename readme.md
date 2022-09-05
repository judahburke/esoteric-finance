# esoteric-finance

Self-Contained Finance Management Web App

## Terraform 

- Editted using VSCode 
- Infrastructure as Code (IAC) functionality enabled 
- Implemented in CI/CD Pipelines 
- NOTE: Assumes that apply will be run within an environment that has Azure CLI installed and is already logged in

## Entity Framework

- Using EF 5 Code First 
- Run `dotnet tool install --global dotnet-ef`
- To generate new migration, run `dotnet ef migrations add {MIGRATION_NAME} --startup-project XeroPay.API --project XeroPay.Data`
- To generate idemptotent script for update, run `dotnet ef migrations script -i --startup-project XeroPay.API --project XeroPay.Data -o migration.sql` 
- To update the database programattically, run `dotnet ef database update --startup-project esoteric-finance-api --project esoteric-finance-data`

# esoteric-finance-app

VueJs SPA

# esoteric-finance-api

.NET 6 Backend Api

## Local Environment

- Optionally create appsettings.local.json
	- Add `"UseLocalhostDb": true`
- Set appsettings.json connection string to your localhost environment
	- Or, if using the hosted dev db, first install Azure CLI (https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows)
	- Run `az login`
	- Run `az account get-access-token --resource-type oss-rdbms`
	- Add `Password={accessTokenFromAzure}` to the existing connection string (note that the token will expire after 60 minutes)

## Testing

A sample request to create a transaction
``` json
{
  "transactionDate": "2022-08-09T16:29:30.987Z",
  "postedDate": "2022-08-09T16:29:30.987Z",
  "recipient": "Racetrac",
  "methods": [
    {
      "method": "Discover",
      "amount": 9
    }
  ],
  "categories": [
    {
      "category": "Food",
      "subCategory": "Snack",
      "multiplier": 1
    }
  ]
}
```

# esoteric-finance-data

Entity Framework Core 6 

Schema

- Tables
    - GeneralLog
        - 
    - Transaction
        - 
    - Recipient
        -
    - TransactionSubCategory (many-to-many)
        -
    - SubCategory
        -
    - Category
        -
    - TransactionMethod (many-to-many)
        -
    - Method
        -
- Relationships
    - Transaction(RecipientId) M-1 Recipient
    - TransactionSubCategory(TransactionId) M-1 Transaction(TransactionId)
    - TransactionSubCategory(SubCategoryId) M-1 SubCategory(SubCategoryId)
    - SubCategory(CategoryId) M-1 Category(CategoryId)
    - TransactionMethod(TransactionId)  M-1 Transaction(TransactionId)
    - TransactionMethod(MethodId) M-1 Method(MethodId)

# esoteric-finance-services

Supplemental Services

- Spreadsheet Import/Export

# esoteric-finance-types

Types and Abstractions for esoteric-finance