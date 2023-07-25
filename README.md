# DISCLAIMER: NO WARRANTY AND SOURCE DISCLAIMER

This repository contains a work-in-progress software project. You are granted the freedom to copy, modify, and distribute it as it is. However, please be aware that there is absolutely NO WARRANTY of any kind, whether explicit or implied.

It is important to note that many lines of code used in this project have been sourced from the internet community. As such, this repository serves as a collection of various code snippets and implementations found online, brought together for convenience.

## Please be advised that:

This software is provided "AS IS," without any guarantees or assurances of its accuracy, functionality, or fitness for any particular purpose.
The author(s) of this repository take no responsibility for any direct or indirect damages, losses, or adverse effects resulting from the use or misuse of this software.
Users are solely responsible for conducting their own thorough assessments, testing, and validation before using this software in any critical or production environment.
By accessing, using, or interacting with this repository, you acknowledge and accept the absence of any warranty and the origin of various code segments from the internet community.

## Keep in mind that:

Although efforts have been made to ensure proper attribution and adherence to relevant licenses for externally sourced code, there may be instances where the original sources are not fully documented. If you notice any discrepancies or if any code requires proper accreditation, please bring it to the attention of the repository's maintainers.
Use of certain code snippets may be subject to specific licenses or restrictions imposed by their respective authors or owners. It is your responsibility to verify and comply with these licenses before using the associated code.
In conclusion, this repository is offered in good faith, with the intent to share knowledge and facilitate collaborative development. However, users must be fully aware of the absence of warranties and the origin of code segments from various internet sources.

If you disagree with these terms or do not wish to accept them, you are kindly requested to refrain from using this repository.

# MsBase
This repo was inpired in the Eduardo Pires course `Enterprise Applications` + Internet content + Relevant jobs I've done. It holds only opensource code, that's necessary to run a microservices backend using:
- ASP .NET
- PostgreSQL
- Seq
- Redis for caching


## Dependencies
The repo relies on the following dependencies:

- Entity Framework Core
- MediatR
- FluentValidation
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Hosting
- Swagger/OpenAPI
Make sure to restore these dependencies before running the application.


# Summary
* [Core](#core)
  * [Setup Program.cs](#setup-program.cs)
    * [Setup program.cs](#setup-program.cs)
      * [API Configurations](#api-configurations)
  * [Abstractions](#abstractions)
    * [Base Entity](#base-entity)
* [Microservices](#microservices)
  * [Audit](#audit)
  * [Auth](#auth)
  * [Email](#email)
  * [Gateway](#gateway)
* [Samples](#samples)
  * [Resort - Sample Entity](#resort---sample-entity)
  * [CQRS](#cqrs)
    * [Controllers](#controllers)
    * [Commands](#commands)
      * [Create](#create)
      * [Handler](#handler)
      * [Input](#input)
      * [Result](#result)
      * [Validator](#validator)
  * [Program.cs](#programcs)

# Core
<b>This is a Boiler Plate Application</b> built using the Entity Framework Core and the repository pattern. The application provides a modular and extensible architecture with abstractions for repositories, database contexts, and command handlers.
The Boiler Plate application showcases the usage of the abstractions and the Stock entity.

## Setup Program.cs
You should add after builder.Service.AddControllers();
```csharp
builder.Services.SetupFullEnvironment<Program>(builder);
```

### API-Configurations

``ApiConfigurations`` is an important class for every microservice that you create and use.

It holds the configuration for every environment, controling several different expected behaviours.


```csharp
public sealed class ApiConfigurations
{
    public Type? AssemblyType { get; set; }
    public bool EnableAudits { get; set; }
    public bool EnableMediatrExceptionPipeline { get; set; }
    public bool EnableMediatrFailFastPipeline { get; set; }
    public bool EnableMediatrLogPipeline { get; set; }
    public bool EnableMediatrPerformancePipeline { get; set; }
    public bool RunMigrations { get; set; }
    public int StatisticBagLimit { get; set; }
    public bool UseKeyCloak { get; set; }
    public bool UseRabbitMq { get; set; }
    public bool UseRedis { get; set; }
    public bool UseApiNotifications { get; set; }
}
```



## Abstractions
The following abstractions are used in this application:

- `IUnitOfWork`: Represents a unit of work for managing transactions and committing changes to the database.
- `IBaseRepository<TEntity>`: Defines the base interface for repositories, providing common CRUD operations and access to the unit of work.
- `IRepository<T>`: Represents a repository interface for working with entities that implement the IAggregateRoot interface.
- `ICommandHandler<TInput, TResult>`: Defines an interface for handling commands with input and result types.
- `ICommand<TResult>`: Represents a marker interface for command inputs.
- `AbstractValidator<T>`: An abstract class from the FluentValidation library used for defining validation rules.
- `Result`: Represents the result of an operation that does not return a value. It indicates whether the operation was successful or not and provides an optional error message.
- `Result<T>`:  class is designed to provide implicit conversion for command and query results to make it easier to work with the result objects. It inherits from the Result class, which represents the result of an operation that does not return a value.

### Base Entity
This entity should be inhirited in every entity of your domain, so the basic audit will be filled.
```csharp
namespace BoilerPlate.Core.Domain.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    protected Entity()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    public Guid Id { get; private init; }
    public DateTime CreationDate { get; }
    public DateTime? UpdateDate { get; set; }

    public static bool operator ==(Entity? first, Entity? second) =>
        first is not null && second is not null && first.Equals(second);

    public static bool operator !=(Entity? first, Entity? second) =>
        !(first == second);

    public bool Equals(Entity? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return other.Id == Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        if (obj is not Entity entity)
        {
            return false;
        }

        return entity.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode() * 41;
}
```
# Microservices
Placeholder -> TODO docs

## Audit
Placeholder
## Auth
Placeholder
## Email
Placeholder
## Gateway
Placeholder


# Samples
## Resort - Sample Entity
The Hotel entity is a sample on how to create your entity. If you need a sample just copy and paste, replace name and properties, respecting inheritance.
```csharp

```

## CQRS 

This project uses the CQRS Design Pattern to segregate concerns: Commands (Create, Update, Delete) and Queries (Read) from CRUD operations.

### Controllers

```csharp

```
### Commands

#### Create
Every command should be stored in: `Application/Commands/{YourEntityOrName}`. 
Here is a sample of the `CreateResort` command:

---
#### Handler
This class is responsable for receiving your command a process it.
```csharp

```
---
#### Input:
Create your input class in your controller and send it to MediatR.

```csharp

```

---
#### Result:
Remember that this class will be implicitly converted to `Result<CreateResortCommandResult>` once your return it in your [Handler](#handler)

```csharp
using Fischer.Core.Domain.Shared;

namespace Fischer.BoilerPlate.Application.Commands.ResortCreate;

internal sealed record CreateResortCommandResult (bool IsSuccess, Error Error);
```

---
#### Validator:
We use FluentValidation pipeline behaviour with MediatR. So when you create a class that inherits from `AbstractValidator<>` with your input class, it will automatically pass through the Validation Pipeline.

```csharp
using FluentValidation;

namespace Fischer.BoilerPlate.Application.Commands.ResortCreate;

internal sealed class CreateResortInputValidator : AbstractValidator<CreateResortCommandInput>
{
	public CreateResortInputValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.WithMessage("The property Name is required.");
	}
}
```


--- 

## Program.cs
The `Program.cs` file serves as the entry point of the application and contains the following configuration and setup:

Adds necessary services and dependencies, such as controllers, Swagger/OpenAPI, and the `%Your%DbContext`.
Sets up the full environment for the application using the `SetupFullEnvironment` extension method.
Configures the database context and registers the `IResortRepository` implementation (StockRepository) with the dependency injection container.
Performs startup preparation using the `StartupPreparation` method.
Configures middleware for cross-origin requests `(CorsPolicy)`, HTTPS redirection, authentication, and authorization.
Maps the controllers and health checks endpoints.
Runs the application.

