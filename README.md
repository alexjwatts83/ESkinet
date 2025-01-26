# ESkinet
ESkinet

## docker
`docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d`

`docker-compose down`

## migrations

### create
`dotnet ef migrations add InitialCreate -s API\ESkitNet.API -p Infrastructure\ESkitNet.Infrastructure -o Data\Migrations`

### remove
`dotnet ef migrations remove -s API\ESkitNet.API -p Infrastructure\ESkitNet.Infrastructure`


### update
`dotnet ef database update -s API\ESkitNet.API -p Infrastructure\ESkitNet.Infrastructure`


### drop
`dotnet ef database drop -s API\ESkitNet.API -p Infrastructure\ESkitNet.Infrastructure`


## Client App

### Angular Material Components
had to install `ng add @angular/material@18` instead as by default it was trying to in stall v17, why don't know I thinkt its a known bug though

### Angular Tailwind CSS 
installing https://v3.tailwindcss.com/docs/installation instead of version 4, just I can use the version he was using in the course. V4 looked like a pretty big change.

Added lint in the setting to ignore the scss warning when adding the @s ine the style.scss file
`"scss.lint.unknownAtRules": "ignore"`

Settings -> type in folders -> uncheck `Explorer : Compact Folders`