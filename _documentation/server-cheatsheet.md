# Cheatsheet reference for server management

`ssh -i .ssh/<private-key> <user>@<server>`

`systemctl start|stop|status <service>`

`*.service` files -> `/etc/systemd/system/`
## Nginx

nginx config files -> `/etc/nginx/sites-available` these get symlinked to `sites-enabled`


## Certbot

certbot files are in -> `/etc/letsencrypt`

also modifies nginx config files

## PostgreSQL

`postgresql` app home directory -> `/var/lib/postgresql`