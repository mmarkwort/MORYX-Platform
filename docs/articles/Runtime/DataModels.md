---
uid: DataModels
---
# Data Models

The Runtime offers developer support for creating, using and maintaining data models.

## Features

Of course using the Runtimes datamodel structure does not only come with restrictions but also quite a lot of features. These feature range from easy database creation over query enhancements to model variance for different applications of a product.

### ModelConfigurator

When generating code from your model a ModelConfigurator is created. This provides connection configuration, database creation as well as Dump&Restore with a common API. This is the data models port used by the MaintenanceUI. The model configurator also collects all model setups designed for this model and executes them if requested.

### IUnitOfWork

Even though we use EntityFramework ^ it is fully hidden underneath generated classes and interfaces. This makes us less dependent on a specific version. Within the Runtime we never access the EF DbContext directly but rather open a UnitOfWork with an injected IUnitOfWorkFactory. A UnitOfWork is a transactions and, because it implements IDisposable, shall always be wrapped in a using-block. It provides two main features - retrieving repositories and saving changes.

### Repositories

Repositories hide the DbSets like the IUnitOfWork hide the DbContext. They wrap all its methods required for creating, modifying and deleting entities. This hiding mechanism is the key to features like model merge or date enhanced entities because we can not only wrap the EF methods but also redirect or manipulate the calls made to them.

### ModelSetups

Model setups are the Runtime standard way to import data into your database. A model setup is a class that implements [IModelSetup](xref:Moryx.Model.IModelSetup) and is decorated with [ModelSetupAttribute](xref:Moryx.Model.ModelSetupAttribute). They export a couple of properties displayed to the user and might specify a regex of files it uses as input. Once the setup is executed from the MaintenanceUI the Execute-method is invoked with an open database connection. If the setup specified a file regex the file path is also passed to the setup. It might than read the file and import its content into the relational model.

## Date Enhanced Entities

By default entity classes have three date properties "Created", "Updated" and "Deleted". If these properties remain in the class a special mechanism is activated. This mechanism activates additional features for this entity and modifies existing features to work differently. All three columns are computed by the database by reading the transaction timestamp. Each operation with the entity will set the "Updated" column. Creating it fills the "Created" column and deleting it fill the "Deleted" column. This results in a couple of effects on the repositories:

**Remove:** Repositories of DateEnhanced entities override remove. Instead of deleting the row from the database the "Deleted"-property is set. The database will override this value with the transaction timestamp to make sure it is identical with the "Updated" column.

Data enhanced entities are not supported for MSSQL Databases.

## Context Modes

All database operations are done within an open [IUnitOfWork](xref:Moryx.Model.IUnitOfWork). Such a connection is retrieved by importing the [IUnitOfWorkFactory](xref:Moryx.Model.IUnitOfWorkFactory) of the model and invoking the Create-method on it. By default this creates a context with all EntityFramework-features enabled. An overload of the Create-method accepts the enum [ContextMode](xref:Moryx.Model.ContextMode). This is flags enum that can be combined.

Effect of the flags:

|Name | Effect |
|-----|--------|
| Proxy | The generated POCO-classes are wrapped in runtime compiled proxy classes. These proxies provide the additional database logic. |
| LazyLoading | Associations to other entities are automatically resolved the first time get is invoked on the navigation-property. While this does has its advantages in complex transactions keep in mind that it does heavily affect performance. | 
| Tracking | The context tracks all entities and changes made to them. This is necessary for change events and deferred loading of associations of lazy loading is disabled. |

This flags can be combined in all possible combinations, however some of them are impossible or useless. It is recommended to use the prepared combinations of the [ContextMode](xref:Moryx.Model.ContextMode) enum.

## Query methods

In general our data models server two purposes - writing and reading data. When it comes to reading data we always specify some sort of criteria and the database responds with matching entries. Runtime enhanced data models come with pre-generated query methods. Those different methods are documented in the following sections.

### Linq2sql

The standard way for queries within the EntityFramework is LinQ2Entities. Simply said it translates the C# LinQ syntax to SQL and converts the returned records into objects. It can be as simple as a single where or as complex as multiple joins, conditions and group by. The query can return instances of our entity classes, POCOs or anonymous classes. For further details please refer to the [Microsoft documentation](https://msdn.microsoft.com/de-de/library/vstudio/bb399367(v=vs.100).aspx).

## Modification tracked entities

Several extionsion methods are provided by the [QueryableExtensions](xref:Moryx.Model.QueryableExtensions) to load entities related to the creation date, deletion date or similar.