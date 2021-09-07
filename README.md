# funda-assignment

This repository consists of 3 .NET Core 5.0 projects:

funda.razor
: Razor based web application

funda.service
: Library for the funda API

funda.tests
: Test project (stub)

## Setup

Install .NET 5.0 SDK from https://dotnet.microsoft.com/download if not already installed.
To verify, open a command prompt and try `dotnet --version`. You should see something like:

    $ dotnet --version
    5.0.103

## Usage

In the repository root:

    $ cd funda.razor
    $ dotnet run

You should see something like:

```
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

Next open a browser to http://localhost:5000. After a while, the top 10 real estate agents should be displayed.

## Tests

To run the tests:

    $ cd funda.tests
    $ dotnet test

Currently there is only a single test, because that's not the focus of the exercise. 
