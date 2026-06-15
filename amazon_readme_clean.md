# 🛒 E-Commerce Platform (Amazon you could say :))

## 📚 Table of Contents

- 🚀 Technologies / Concepts
- 🚀 Microservices Architecture
- 🚀 Domain-Driven Design (DDD)
- 🚀 Clean Architecture
- 🚀 OIDC
- 🚀 SAGAs
- 🚀 API Gateway
- 🚀 Event-Driven Design (RabbitMQ)
- 🚀 CQRS
- 🚀 NoSQL Data Model
- 🚀 Caching
- 🚀 CAP
- 🚀 Event Sourcing
- 🏗️ Architecture
- 🧱 Tech Stack
- 🧱 Technical Decisions
- 🧱 Applied Patterns / Practices

A scalable, modular e-commerce application built with .NET, following Domain-Driven Design (DDD), Clean Architecture, and microservices principles.


## 🚀 Technologies/Concepts included

- Microservices architecture
- Domain-Driven Design (DDD)
- Clean Architecture
- OIDC
- SAGAs (Orchestrator)
- API Gateway
- Containerization(Docker & Docker Swarm)
- Event-driven design (RabbitMQ)
- CQRS
- Logging (Serilog + Seq)
- NoSql Data model
- SQL Data model
- Caching (Http/Application)
- CAP(favoring Consistency when adding items to cart)
- Event Sourcing


## 🚀 Microservices architecture
    ## divide the system into individual components to allow for
        - easier deployments
        
        - different data models(NoSql,SQL)
        
        - different tech(Inventory uses gRPC for faster communication since it`s only accessed internally)
        
        - different development teams

## 🚀 Domain-Driven Design (DDD)
    ## Several/Different Bounded Contexts
        - (where each Bounded Context has it`s own definition/meaning of their domain objects)
            - ex: : ORDERs service/Bounded Context defines a domain object called Stakeholder
                - which can be of THREE kinds
                    - Admin => can access any queried order
                    - Customer => only accesses owned orders
                    - Delivery Employee => should only access orders they received by the shipping company assigned to that order ONLY at the required stage(Not implemented, but the code is ready to do that)
                
                - in the Identity service/Bounded Context these same objects are called users and differentiaited by roles

                - in the Customers service/Bounded Context we only have customers domain objects

    ## AGGREGATES
        - Responsible of transactional consistency
            - NOT as a "has" or "contains" relation
            - BUT as scope/invariants controller
        
        - ex: CUSTOMERs service
            - a customer can have at max THREE saved payment cards
            - a customer must always have a default delivery address

    ## Domain Events
        - When a product is deleted from the catalog
            - cart service should
                - disable using it for adding new items 
                - invalidate it`s cache 

## 🚀 Clean Architecture
    ## divide the microservice into seperate LAYERs, each responsible for ONLY ONE concern
        - API               => only concerned with routing requests to specific route handlers and delegate the work to use cases/application LAYER 
        
        - Application       => implement use cases by orchestrataing domain objects/services and commit the changes

        - Infrastructre     => this is where low level details(actual implementations) reside 
                            => persist the domain objects into specific data store  
                            => other concerns like (HTTP, gRPC Clients)

        - DOMAIN            => contains CRITICAL business logic

## 🚀 OIDC
    ## based on the oAuth protocol that specifies how to authorize user requests to different resource servers/APIs, seamlesly for different Identity providers
        - adds authentication layer to get user info

## 🚀 SAGAs
    ## to better handle order workflow and see the process in one place
        - after we recieve a OrderPaymentConfirmedEvent the order process Orchestrator begins
            - first sends a ReserveInventoryCommandInventoryReservedEvent
            - when INVENTORY receievs this command, it does it`s part
                - if it succeeds it sends a InventoryReservedEvent back to the orchestrator
                    - orchestrator then, sends CreateShipmentCommand
                        - the SHIPMENT service then does it`s part
                            - if it succeeds it sends a ShipmentCreatedEvent back to the orchestrator
                                - then it update`s the order status accordingly
                            - if it fails it sends a CreateShipmentFailedEvent
                                - which in this case i think we should JUST try again later
                                
                - if it fails it sends a InventoryReservationFailedEvent back to the orchestrator
                    - orchestrator then, recoves/compensantes for the order
                        - so that we can create a refund for the customer
                        - or we can if we need to serve the order to the customer from somewhere else IF POSSIBLE

## 🚀 API Gateway
    ## YARP: Yet Another Reverese Proxy: have lots of benefits like
        - ONLY expose the Gateway publicly to the client/frontend
            - and depending on each request, it can route it to the correct service

        - with this approach, we will never need to expose the real services hosts/IPs to the public

        - we do SSL termination at the Gateway
            - so we need to maintain ONLY one SSL Certificate
                - of course we trust the internal/private network that services are inside
        
        - we can configure CORS policies ONLY once at the Gateway
            - this also defend accessing our APIs from origins we dont trust
        
        - apply Ratelimiting

        - put any other cross-cutting concerns at the Gateway level

## 🚀 Event-driven design (RabbitMQ)
    ## to help the achieve services autonomy
        - we leverage the async communication using message brokers like RabbitMQ
            - so that if one service(Inventory) is down for some time
                - the order process is not blocked
                - till it`s back again, it can listen to the persisted commands/events
                    - to do it`s parts of the process 

## 🚀 CQRS
    ## to aggregate customer data (Delivery addresses, payment cards)
        - we created a CQRS view(in a seperate READ database) that contain these info in one place(every row has CustomerId,Addresses as Json, PaymentCards as Json)
            - so no SQL joins needed here to gather all these front end data
                - all in just one request
            - with the help of Domain events raised on the Customer domain objects
                - we can sync the data of that view synchronously
                - that`s why when a customer adds a payment card, it may take some seconds to appear in his profile
                    - also perfectly fits with the business, cause we may need to validate the card details with it`s provider in the background
                        - this cannot be achieved using transactional consistency

## 🚀 NoSql Data model
    ## ONE-TO-MANY relationships fits perfectly with document databases/NoSql
        - the data locality property helps execute the query in a fast manner
            - it means the whole document/object with it`s rleated data altogether exists in one location on disk
                - so we only need one physical read to access it

        - we used this to model the CART document with it`s cart items
            - previously i modeled it as two SQL relations(Cart, Cart Items)
                - and read the data using joins

## 🚀 Caching (Http/Application)
    ## public data like(product images & lookups, etc..) are good candiates for data that should be cached
        - we can cache data on two levels
            - HTTP => using http headers
                - for product images, we cached them to the client/browser
                    - so that next time you reload the catalog
                        - the browser JUST serves them locally without need for round trips to the server

            - applicaion => either in memory or a distrbuted storage
                - for scalability we used REDIS to access the same cached data from multile instances of the same service
                    - this have huge benefit on the database
                        - as we wont hit the database for every display of a product image(catalog/cart)

## 🚀 CAP
    ## in a distributed system, eitehr you take Consistency OR Availabilty
        -  in case of adding cart items to your cart, the most important thing is not ADDING them
            - but ADDING them IF AND ONLY IF they are really available
                - so we must not depend on a stale info that that product has available items in INVENTORY
            - so we accept some latency to check with the Inventory the availabilty of products

## 🚀 Event Sourcing(did not fully utilize it)
    ## to persist data we have two patterns
        - Current State
            - whenever a new change/state is needed we just persist it by overriding the "Current" one

        - Events
            - we store every change seperatly
                - and use it to ask for the current state

            -ex: we used it for manaing Order states
                - as the current state is the last state event
                    - while keeping auidts of what happened over the lifetime of each order
                
## 🏗️ Architecture

The system is built using a microservices approach where each service is independently deployable and loosely coupled.

### Core Services/Domains

## These are the important services to spend resources(Development & People) on

- **Catalog Service** → Manages products & categories
- **Cart & Checkout** → add products to the cart & checkout
- **Order Service** → Handles order creation & workflow
- **Inventory Service** → manages/updates products availabilty
- **Shipment Solution** → a very simple service/algorithm to pick a shipping company to ship orders to customers
- **Identity Management** → handle admins, new customer registrations
- **Customer Profile Management** → allow customer to update their info(add payment cards & deliver addresses, etc..)
- **Shared-Kernel** → an agreed upon domain objects/concepts (common events to the domain)


### Generic Services/Domains

## These are the services that can be obtained as third party tools or integerated with by other means, should not waste resources(Development & People) on 

- **API Gateway** → The only public available endpoint that route requests to other services
- **Media Service** → File/image handling
- **Lookups Service** → hold countries & cities information


## 🧱 Tech Stack

- .NET 9
- ASP.NET Core Controllers
- ASP.NET Core Minimal APIs
- REST for the public facing
- gRPC for internal communication(Inventory)
- YARP API Gateway
- Ef Core 9
- Ms SQL Server
- MongoDb
- Redis
- RabbitMQ
- MassTransit
- Fluent Validation
- Docker & Docker swarm
- Serilog + Seq for logging
- JWT / Duende to implement OIDS/oAuth
- Angular 17 for the front end
- SignalR
- Framework Hosted Services
- Framework Dependency Container
- Stripe as a Payment Gateway


## 🧱 Techincal Decisions
    - Error handling
        - i am being pragmatic here, at the end of the day we are developing a "web" e-commerce application
            - that will be served to customers over HTTP
                - either from a browser or mobile/desktop app
            - so instead of throwing exceptions as an execution flow method
                - every action must report it`s success or failure
                    - failure has types(from HTTP POV)
                        - a client mistake(400)
                        - a not found resource(404)
                        - a server mistake(500)
                        - ...
            - but it has downsides(acceptable by me)
                - the pattern propagates eveywhere
                    - any method call must be checked if it`s a succeess/failure


## 🧱 Applied patterns/practices
    - Outbox message
        - to gurantee messages delivery
    
    - DDD factories
        - they are not always normal factories(a seperate factory class) that just has a method the creates objects
            - ex: CARTs
                - to make sure each added cart item is correct in state
                    - i ask the product(factory) itself to construct/generate item for me
                    
                - if there is no product(soft deleted) or not exist, there is no way with the help of the Compiler
                    - to generate/obtain an invalid cart item
    
    - DDD repositories
        - we shall not access/query entities controlled by an Aggregate directly from it`s persistence store
            - to protect some invariants/business rules through their aggregate root

    - DDD aggregate with document model
        - NoSql or document databases works very well with DDD aggregates
            - a cart WHICH embeds it`s cart items inside it({cartId:"",customerId:"",items:[{productId:"",quantity:5}]})
                - designs a very good aggregate
                    - as there is no direct way you can write a global query to access random cart items!
                        - there is even no seperate collection called CartItems you can query against
                            - cart items is just a colelction of objects exit only under a specific cart

    - DDD specification
        - to validate a cart just before checking it out
            - maybe a product was deleted while shopping

    - DDD VALUE OBJECTs
        - Value Object`S capability of being shared/copied safely
            - allowed the CART service to use/share/refernce the same product info/value of a specific product entity instance
                - which allowed these products to be cached/reused

    - Ef Core inheritance capabilities
        - To represent Stakeholder domain objects(ORDERs service)
            - and not worry how to obtain the concrete object a runtime
- 

