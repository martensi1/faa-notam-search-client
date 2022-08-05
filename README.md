# Notam Search API Client

Small and simple API client for the REST API provided by FAA's NotamSearch service.

Features:

* Fetch NOTAMs

Link:
https://notams.aim.faa.gov/notamSearch/nsapp.html

## Example
```csharp
string notam = NotamClient.FetchNotam("ESSA");
Console.WriteLine(notam);
```

## Installation
Install with NuGet:

```
dotnet add package PilotAppLibs.Clients.FaaNotamSearch
```


## License

This repository is licensed with the [MIT](LICENSE) license

## Author

Simon Mårtensson (martensi)
