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