#Sample of getting the DocumentDB partitioning range

This is the DocumentDB sample.
Using this sample code, you can get the server-side partitioning range in partitioning collection, and you can determine which partition is used for the document for some partition key. (This uses the hash partitioning.)
The hash for getting the partitioning range in the server is the hex binary encoding of Murmur Hash. This source code refers the DocumentDB SDK for .NET. (This task is done inside the SDK.)

Please see https://blogs.msdn.microsoft.com/tsmatsuz for details.
