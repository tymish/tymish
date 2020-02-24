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
* `yarn add-migration <migration-name>`
* `yarn update-db`


## Nice to haves
* GraphQL for data fetching