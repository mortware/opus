# opus
Demonstration of a Job sheet processing system using .NET Core 3.1 & Angular 9+

### You will need
 - [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)

### Request example

```
POST /job/create
```

```
{
  "referenceLabourInMinutes": 330,
  "referencePrice": 670,
  "items": [
    {
      "$type": "TyreReplacement",
      "position": "NearsideFront"
    },
    {
      "$type": "TyreReplacement",
      "position": "OffsideFront"
    },
    {
      "$type": "Exhaust"
    },
    {
      "$type": "OilChange"
    }
  ]
}
```
