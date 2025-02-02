# ESkinet
ESkinet

## Running Locally

### Prereqs

- Create account in Stripe, https://stripe.com/au
  - Copy `PublishableKey` and `SecretKey` into User Secrets
- download and install Stripe cli locally
  - https://docs.stripe.com/stripe-cli
  - Try later use Docker image instead, when I tried I always got a connection refused message
- Docker Desktop
- Create an account in upstash https://upstash.com/ for Prod Redis
- probably more but I can't remember what

### Debug Locally

- start docker
  - `docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d` or `.\docker-app-up.ps1`
- build ui
  - `ng serve`
- start stripe
  - `stripe login` (optional - need to login first, could probably use the api key instead)
  - `stripe listen --forward-to https://localhost:5151/api/payments/webhook --events=payment_intent.succeeded`
  - The command line generates a WH Secret copy that value into User Secrets
- Start Debugging the `ESkitNet.API` project with the `https` profile

## Docker
`docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d`

`docker-compose down`

deletes the volumes as well
`docker-compose down -v`

## EF Migrations

Migrations are automatically applied on startup and also data is seeded if it doesn't exists

### create
`dotnet ef migrations add InitialCreate -s API\ESkitNet.API -p Infrastructure\ESkitNet.Infrastructure -o Data\Migrations`

### remove
`dotnet ef migrations remove -s API\ESkitNet.API -p Infrastructure\ESkitNet.Infrastructure`


### update
`dotnet ef database update -s API\ESkitNet.API -p Infrastructure\ESkitNet.Infrastructure`


### drop
`dotnet ef database drop -s API\ESkitNet.API -p Infrastructure\ESkitNet.Infrastructure`

## Stripe

In order to trigger a stripe event when a Order is created.

`stripe login`

`stripe listen --forward-to https://localhost:5151/api/payments/webhook --events=payment_intent.succeeded`


## Client App

### Angular Material Components
had to install `ng add @angular/material@18` instead as by default it was trying to in stall v17, why don't know I thinkt its a known bug though

### Angular Tailwind CSS 
installing https://v3.tailwindcss.com/docs/installation instead of version 4, just I can use the version he was using in the course. V4 looked like a pretty big change.

Added lint in the setting to ignore the scss warning when adding the @s ine the style.scss file
`"scss.lint.unknownAtRules": "ignore"`

Settings -> type in folders -> uncheck `Explorer : Compact Folders`


### Deployment

## Redis