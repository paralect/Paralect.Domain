Paralect.Domain (EventStore)
==========

Usage
-----

```csharp
    // Event store configuration
    var dataTypeRegistry = new AssemblyQualifiedDataTypeRegistry();

    var transitionsRepository = new MongoTransitionRepository(
        new AssemblyQualifiedDataTypeRegistry(),
        settings.MongoWriteDatabaseConnectionString);

    var transitionsStorage = new TransitionStorage(transitionsRepository);

    // Here we are using StructureMap
    container.Configure(config =>
    {
        config.For<ITransitionStorage>().Singleton().Use(transitionsStorage);
        config.For<IDataTypeRegistry>().Singleton().Use(dataTypeRegistry);
        config.For<IEventBus>().Use<ParalectServiceBusEventBus>();

        // We are using default implementation of repository
        config.For<IRepository>().Use<Repository>();
    });
```