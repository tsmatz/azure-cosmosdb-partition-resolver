# Example for getting Azure Cosmos DB partitioning range

You cannot control physical partitions in Azure Cosmos DB, however, using this sample code, you can get the server-side partitioning range in partitioning collection, and you can determine which partition is used for document with some partition key.    
This uses the hash partitioning and the hash for getting the partitioning range in the server is the hex binary encoding of Murmur Hash.

This source code refers Azure Cosmos DB SDK for .NET. (This task is implicitly used inside the SDK.)

Please see my post "[Azure Cosmos DB (NoSQL) – How it works](https://tsmatz.wordpress.com/2016/12/07/azure-documentdb-and-mongodb-partitioning-replication-indexing/)" for details.
