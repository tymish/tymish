# Tymish

Connection string is stored in secret `~/.microsoft/usersecrets/...`
Use Environment Variable for PROD release


## PostgreSQL
* `sudo -i -u postgres`
* `createuser --interactive --pwprompt`
* user name is `dev`

### Drop database
* `sudo -i -u postgres`
* `psql`
* `\l`
* `drop database "Tymish";`

## Entity framework Code first migration
* the project requires `Microsoft.EntityFrameworkCore.Design`
* `dotnet ef migrations add migration-name -p Persistence -s WebApi`
* `dotnet ef database update -p Persistence -s WebApi`


## Nice to haves
* GraphQL for data fetching