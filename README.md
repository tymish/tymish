# Tymish

Connection string is stored in secret `~/.microsoft/usersecrets/...`
Use Environment Variable for PROD release


## PostgreSQL
* `sudo -i -u postgres`
* `createuser --interactive --pwprompt`
* user name is `dev`

## Entity framework Code first migration
* the project requires `Microsoft.EntityFrameworkCore.Design`
* `dotnet ef migrations add migration-name -p ../Persistence`
* `dotnet ef database update`


## Nice to haves
* GraphQL for data fetching