# Reusable DB Contexts

These are a few reusable DB Contexts and interfaces for a few DB Contexts.

## Postgres Context

A reusable abstract class for DbContexts that connect to a PostgreSQL DB. More specifically, one that's hosted using HerokuPostgres. It expects the Database URL to be in the URI format used by Heroku.

```postgres://user:password@host:port/database```

It expects it to be in the environment variable `DATABASE_URL` (The one that Heroku uses), or the text file DBURL.txt for easier debugging locally. If no Database URI is found anywhere, it will write out `here` to the DBURL.txt file it expected to find, and throw exception.

## Context Interfaces

Interfaces for contexts that contain Notifications (From Notifier), Users (from ChopoAuth), and Images (From ChopoImageHandling) are also provided in this package
