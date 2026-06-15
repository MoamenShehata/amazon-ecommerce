Amazon E-Commerce — Microservices Reference

Summary

This repository is a microservice-based e-commerce reference implementation built with .NET 9 and Domain-Driven Design (DDD). It demonstrates pragmatic patterns for building resilient, observable, and evolvable distributed systems: bounded contexts, aggregates, value objects, domain events, outbox, orchestrator SAGA, event sourcing for order history, a Result-based error handling pattern, and a shared kernel for cross-cutting concerns.

Key Technologies

- .NET 9 (per-project targeting)
- Razor Pages (identity UI), minimal APIs and ASP.NET Core APIs
- MongoDB / EF Core (per-service persistence patterns vary by service)
- AesGcm-based text encryption utility in shared kernel (ITextServices)
- Message broker (async integration), Outbox pattern
- Event sourcing for order history
- Docker + Kubernetes (intended deployment patterns)

Solution Structure (high level)

- shared-kernel/Amazon.SharedKernel
  - Shared cross-cutting concerns: Result<T> pattern (SharedKernel/Common/Result.cs), RestResponse, repositories, NoSQL helpers (MongoDbRepository), domain event interceptors and common services (ITextServices, OtpService, etc.).
- customers, cart, product-catalog, orders, shipping, inventory, media, notifications, identity-management
  - Each feature/subdomain is implemented as its own service (API, Application, Domain, Infrastructure projects) following DDD boundaries.
- gateway/Amazon.Apis.Gateway
  - API Gateway project that composes/aggregates APIs at the edge.

Domain-Driven Design (DDD)

This solution follows DDD principles across services:

- Bounded Contexts: Each microservice is its own bounded context (orders, cart, product-catalog, customers, etc.).
- Aggregates & Entities: Domain models encapsulate invariants. For example, Order and Product aggregate roots live in their respective domain projects.
- Value Objects: Several value objects (e.g., ProductInfo, ProductPrice, PaymentCardNumber) encapsulate small, immutable concepts and domain validation.
- Domain Events: Domain events are used to decouple side-effects; product-catalog and other services raise events handled asynchronously (see DomainEventsPublisherInterceptor and domain Events in each domain project).

Result pattern (application-level success/failure)

This codebase avoids using exceptions to drive normal control flow. Instead it uses a Result / Result<T> pattern (shared-kernel/Common/Result.cs). Application and domain services return Result objects to represent success/failure and carry error messages and optional exception details. Controllers and API adapters translate Results into appropriate HTTP responses (RestResponse wrapper).

Sagas / Orchestrator pattern

Order workflows are implemented with an orchestrator-style Saga that centralizes the long-running order creation process. Look for the Orders.Application Processes (Orders/Application/Processes/OrderCreationProcess.cs) to see the orchestration of steps (reserve inventory, payment challenge/confirmation, shipment creation, etc.). This approach keeps process coordination explicit and easier to reason about than ad-hoc distributed transactions.

Outbox & Event-driven Integration

Product-catalog uses an Outbox approach (see product-catalog infrastructure) and a domain-events publisher interceptor to ensure domain events are reliably published to the message broker. This yields eventual consistency and avoids distributed transaction coordination across services.

Event Sourcing

Order history is persisted using event sourcing for audit/history purposes while the current order state is kept in a read model. Check the Orders.Domain and Orders.Infrastructure projects for event storage and replay logic.

Persistence & Repositories

The Shared Kernel includes repository helpers (e.g., NoSql/MongoDbRepository) and each service exposes repository interfaces/implementations appropriate to its store (relational, NoSQL, or event store). Unit-of-work abstractions are used in infrastructure projects where needed.

Security & Secrets

- Token-based authentication via Jwt/OpenID Connect patterns is used in APIs (see shared-kernel extension registrations and Identity project).
- For decryptable secrets used by code (ITextServices), prefer to set TEXT_SERVICES_KEY through secure configuration or secret managers so encryption/decryption survive process restarts.

Testing

- Unit and domain tests are present under tests for multiple services (e.g., Orders.Domain.Tests, ProductCatalog.Domain.Tests, Customers.Domain.Tests).
- Integration tests and contract-style tests are encouraged for cross-service interactions.

Local development

- Open the solution in Visual Studio 2022/2026 or use dotnet CLI targeting .NET 9.
- Each microservice has its own startup/project; run services independently or wire them with docker-compose that you provide.
- Set TEXT_SERVICES_KEY env variable if you need repeatable encryption across runs.

Recommended developer conventions

- Use Result<T> for returning operation outcomes from application/domain services.
- Use domain events for side-effects inside the same bounded context; publish integration events through outbox for cross-service communication.
- Keep orchestrator logic (Saga) in a single process/service when it owns the workflow; keep compensations explicit.
- Prefer value objects to primitive types for domain consistency and validation.

Contributing

- Follow existing project layering: Api -> Application -> Domain -> Infrastructure.
- Add unit tests for domain invariants and application workflows.
- Open an issue and submit a PR. Provide a short description and link to related domain docs.

Further work / TODOs

- Centralize configuration for secrets (KeyVault/SecretManager) and remove process-local keys.
- Improve observability (traces, correlation IDs) across async flows if not fully wired.
- Add contract tests for critical integrations (e.g., cart <> orders, product-catalog <> inventory).

Contact

This README summarizes architecture and patterns in the repository. For detailed questions about specific services or files, see the per-service README or open an issue in the repository.