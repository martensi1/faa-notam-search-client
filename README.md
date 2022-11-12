# Notam Search API Client

Small and simple API client for the REST API provided by FAA's NotamSearch service.

Features:

* Fetch NOTAMs

Link:
https://notams.aim.faa.gov/notamSearch/nsapp.html

## Example
```csharp
List<NotamRecord> notams = NotamClient.FetchNotams("ESSA");

Console.WriteLine("===ESSA NOTAMs===");
foreach (var notam in notams)
{
    Console.WriteLine(notam.Message);
}
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
