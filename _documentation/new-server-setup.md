# Server Setup
* Login `ssh -i .ssh/<private-key> root@<server-ip>`
* set the key in `.ssh` and name it `root`

## Required software
* dotnet aspnetcore runtime
* nginx web server
* postgres database
* Certbot SSL tool

## Dotnet Core Runtime 5.0
```bash
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb

sudo dpkg -i packages-microsoft-prod.deb

sudo apt update
sudo apt install -y aspnetcore-runtime-5.0
dotnet --info # verify install
```

## Nginx Web Server
``` bash
sudo apt update
sudo apt install nginx
nginx -v # verify install
```

## Add SSL to Nginx
Make sure to point the domain to the server first.
### Install Certbot
``` bash
sudo apt install certbot python3-certbot-nginx
```

### Run Certbot
``` bash
sudo certbot --nginx
```

### Renew SSL (Only do this if expired)
``` bash
certbot renew
```

## PostgreSQL
### Install
``` bash
sudo apt update
sudo apt install postgresql postgresql-contrib
psql --version # verify install
```
### Restore Db

Put the `create-db.sql` on the server.

``` bash
scp -i ~/.ssh/<private-key> /path/to/create-db.sql <user>@<server>:/path/to/dest
```

Move the scrip to postgres default user

``` bash
cp create-db.sql /var/lib/postgresql
```

Run the script

``` bash
sudo -i -u postgres 
dropdb Tymish
createdb Tymish
psql -d Tymish -f create-db.sql
psql # opens psql repl to run commands
\l   # verify Tymish db is present
```

### Create a db user that EF Core connection string will use
``` bash
sudo -i -u postgres                     # use postgres account
createuser --interactive --pwprompt     # create a user 
```

## Forward Nginx calls to Kestrel
### Modify `/etc/nginx/sites-available/default`
```
location / {
    proxy_pass         http://localhost:5000;
    proxy_http_version 1.1;
    proxy_set_header   Upgrade $http_upgrade;
    proxy_set_header   Connection keep-alive;
    proxy_set_header   Host $host;
    proxy_cache_bypass $http_upgrade;
    proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
    proxy_set_header   X-Forwarded-Proto $scheme;
}
```

### Add Kestrel to `systemd` service manager
https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-5.0

### Add the Kestrel service to systemd
Create the file `/etc/systemd/system/<name>.service`
```
[Unit]
Description=Tymish Api running on .NET Core Kestrel

[Service]
WorkingDirectory=/var/www/tymish-api
ExecStart=/usr/bin/dotnet /var/www/tymish-api/WebApi.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=dotnet-tymish-api
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

# Secret environment
Environment=ConnectionStrings__TymishContext="<secret>"
Environment=ApiKeys__SendGrid="<secret>"

[Install]
WantedBy=multi-user.target
```

`systemctl` commands
``` bash
sudo systemctl enable <name>.service # only need to enable it once
sudo systemctl start <name>.service
sudo systemctl status <name>.service
sudo systemctl stop <name>.service
```