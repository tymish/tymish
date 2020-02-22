# Tymish

Connection string is stored in secret `~/.microsoft/usersecrets/...`
Use Environement Variable for PROD release


## PostgreSQL
* `sudo -i -u postgres`
* `createuser --interactive --pwprompt`
* user name is `dev`

## Entity framework Code first migration
* the project requires `Microsoft.EntityFrameworkCore.Design`
* `dotnet ef migrations add migration-name`
* `dotnet ef database update`