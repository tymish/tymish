# Tymish

A dotnet core web api with postgres.

## Dev Setup

### 1. Install tools

```bash
node --version # v14.15.0
yarn --version # 1.22.5
psql --version # 12.4
dotnet --version # 5.0.100
```

### 2. Setup database
```bash
sudo -i -u postgres                     # use postgres account
createuser --interactive --pwprompt     # create a user to use for VSCode and other GUI tools
# provide username and password         # Remember these for the connection string
```

### 3. Setup connection string with dotnet `secrets-manager`
Replace `<REPLACE>` with your new postgres user name and password

```bash
echo '{ "ConnectionStrings:TymishContext": "Host=localhost;Port=5432;Database=Tymish;Username=<REPLACE>;Password=<REPLACE>;" }' \
> ~/./usersecrets/d2078994-b580-4445-aa97-ed51fbb20f6b/secrets.json
```

### 4. Run Entity Framework Migrations
```bash
yarn build                              # ...
yarn drop-db                            # drops the db
yarn add-migration <migration-name>     # <migration-name> can be 'init'
yarn update-db                          # updates the database
```

### 5. Run the WebApi
We run the api without TLS because we let NGINX do the certificate on testing servers
```bash
yarn start
# http://127.0.0.1:5000/swagger
```

## Snippets for development
### Manually Drop database
```bash
sudo -i -u postgres
psql
\l
drop database "Tymish";
```

## VS Code Extensions
Use the `ckolkman.vscode-postgres` extension to view postgres db.
I am using the `dev` user not the `postgres` default user because it has no password.