# iHeartLinks.Core

[![Build Status](https://dev.azure.com/marlon-hizole/iHeartLinks/_apis/build/status/iHeartLinks.Core.CI?branchName=master)](https://dev.azure.com/marlon-hizole/iHeartLinks/_build/latest?definitionId=13&branchName=master)

**iHeartLinks.Core** is a class library for implementing [HATEOAS](https://en.wikipedia.org/wiki/HATEOAS) in a RESTful API. It is a framework agnostic library that should be implemented and extended in a specific framework. See [iHeartLinks.AspNetCore](https://github.com/ponki-d-monkey/iHeartLinks.AspNetCore) as an example - an implementation in ASP.NET core.

## Installation

Install the package from [NuGet.org](https://www.nuget.org/packages/iHeartLinks.Core/).

```powershell
PM> Install-Package iHeartLinks.Core
```

## Implementation

Once installed, implement the [IHypermediaService](src/iHeartLinks.Core/IHypermediaService.cs) interface. It is an interface for building links. See the [implementation](https://github.com/ponki-d-monkey/iHeartLinks.AspNetCore/blob/master/src/iHeartLinks.AspNetCore/HypermediaService.cs) for ASP.NET core as a guide.

## Extending

The **IHypermediaService** interface contains methods for retrieving URL's and it is used by the internal [HypermediaBuilder](src/iHeartLinks.Core/HypermediaBuilder.cs) class to build and add links to an instance of `IHypermediaDocument`. For convenience of creating and adding links, create extension methods of the [IHypermediaBuilder](src/iHeartLinks.Core/IHypermediaBuilder.cs) interface. See the [extension methods](https://github.com/ponki-d-monkey/iHeartLinks.AspNetCore/blob/master/src/iHeartLinks.AspNetCore/HypermediaBuilderExtension.cs) for ASP.NET core as a guide.

## Adding links

Start by adding a _self_ link.

```csharp
hypermediaService
  .AddSelf(model)
  .Document;
```

where `model` is an instance of a class implementing `IHypermediaDocument`.

In case a _self_ link is not needed, use the `Prepare()` method. This can be used when adding links to a property of a class implementing `IHypermediaDocument` is needed.

```csharp
hypermediaService
  .AddSelf(model)
  .AddLinksToChild((m, svc) => svc
    .Prepare(m.Child)
    .AddLink("child", $"https://your.api.com/person/child/{m.Child.Id}"))
  .Document;
```

`m.Child` must also be an instance of a class implementing `IHypermediaDocument`.

To add a link, call the `AddLink()` method.

```csharp
hypermediaService
 .AddSelf(model)
 .AddLink("update", $"https://your.api.com/person/{model.Id}"))
 .Document;
```

A condition can also be passed to `AddLink()`.

```csharp
hypermediaService
 .AddSelf(model)
 .AddLink("update", $"https://your.api.com/person/{model.Id}", m => m.IsActive))
 .Document;
```

As seen in the example before, adding a link to a property of a class implementing `IHypermediaDocument` is possible. Another possible use of this method is when the property is a collection.

```csharp
hypermediaService
  .AddSelf(model)
  .AddLinksToChild((m, svc) =>
  {
      foreach (var item in m.Items)
      {
          svc
              .Prepare(item)
              .AddLink("update", $"https://your.api.com/person/child/{m.Child.Id}");
      }
  })
  .Document;
```

Multiple links can be added based on a single condition.

```csharp
hypermediaService
  .AddSelf(model)
  .AddLinksPerCondition(m => m.IsActive, b => b
    .AddLink("update", $"https://your.api.com/person/{model.Id}", "POST")
    .AddLink("deactivate", $"https://your.api.com/person/{model.Id}", "PATCH"))
  .Document;
```
