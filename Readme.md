#Sample of getting the DocumentDB partitioning key

This is the DocumentDB sample.
Using this sample code, you can get the partition key from your key value, and you can determine which partition is used for the document. (This sample is targeting only the hash partitioning.)
The algorithm of the server-side partitioning for hash is the hex binary encoding of Murmur Hash. This source code refers the DocumentDB SDK for .NET.

Please see https://blogs.msdn.microsoft.com/tsmatsuz for details.
