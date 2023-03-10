# MongoDB timeouts on insert
This repo holds the example application to demonstrate the timeouts we are experiencing when using MongoDB Atlas with Azure Serverless.


| Atlas backend                      | Logs                                                                                                                                                                                                                                                                                                                                                                                                                                                                     | Result |
|------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------|
| Azure Serverless (hosted in NL)    | [logs/azure-serverless-log-try1.txt](logs/azure-serverless-log-try1.txt)<br>[logs/azure-serverless-log-try2.txt](logs/azure-serverless-log-try2.txt)<br>[logs/azure-serverless-log-try3.txt](logs/azure-serverless-log-try3.txt)<br>[logs/azure-serverless-log-try4.txt](logs/azure-serverless-log-try4.txt)<br>[logs/azure-serverless-log-try5.txt](logs/azure-serverless-log-try5.txt)<br>[logs/azure-serverless-log-try6.txt](logs/azure-serverless-log-try6.txt)<br> | NOK    |
| Azure Dedicated M30 (hosted in NL) | [logs/azure-dedicated-m30-log.txt](logs/azure-dedicated-m30-log.txt)                                                                                                                                                                                                                                                                                                                                                                                                     | OK     |
| Google Serverless (hosted in BE)   | [logs/google-serverless-log.txt](logs/google-serverless-log.txt)                                                                                                                                                                                                                                                                                                                                                                                                         | OK     |


> **NOK** for Azure Serverless is because, sometimes the application takes very long to insert the document.  
Then we see times of: `Completed in 953285ms` this is `15+` minutes.....


## What does the application do

1. Create a connection to MongoDB Atlas (with the provided connectionString)
2. Enable `debug` logging for Mongo Driver
3. Insert a very simple document every 5 minutes
4. Report the time needed to insert the document
5. If the insert takes longer then 5 seconds stop and alert

#### Tested with:
- net7
- MongoDB.Driver version 2.19.0

### Normal output/log of 1 insert

```log
dbug: MongoDB.SDAM[0]
      1 Cluster opening

dbug: MongoDB.SDAM[0]
      1 Cluster opened
3/9/2023 5:47:58 PM
Inserting with ref 0ea21353-7472-4256-949f-8fcdf032fa44
dbug: MongoDB.SDAM[0]
      1 Description changed
dbug: MongoDB.SDAM[0]
      1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 Server opening
dbug: MongoDB.Connection[0]
      1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 Connection pool opening 600000 0 100 2 120000 500
dbug: MongoDB.Connection[0]
      1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 Connection pool created 600000 0 100 2 120000 500
dbug: MongoDB.Connection[0]
      1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 Connection pool ready
dbug: MongoDB.SDAM[0]
      1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 Description changed { ServerId: "{ ClusterId : 1, EndPoint : "Unspecified/timeout-testing-jeff-lb.izjha.mongodb.net:27017" }", EndPoint: "Unspecified/timeout-testing-jeff-lb.izjha.mongodb.net:27017", ReasonChanged: "Initialized", State: "Connected", ServerVersion: , TopologyVersion: , Type: "LoadBalanced", WireVersionRange: "", LastHeartbeatTimestamp: null, LastUpdateTimestamp: "2023-03-09T17:47:58.7237907Z" }
dbug: MongoDB.SDAM[0]
      1 Description changed
dbug: MongoDB.Connection[0]
      1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 Connection checkout started
dbug: MongoDB.Connection[0]
      1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 Connection adding
dbug: MongoDB.Connection[0]
      1 1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 Connection created
dbug: MongoDB.Connection[0]
      1 1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 Connection opening
dbug: MongoDB.SDAM[0]
      1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 Server opened
dbug: MongoDB.Connection[0]
      1 1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 Connection ready
dbug: MongoDB.Connection[0]
      1 1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 Connection added
dbug: MongoDB.Connection[0]
      1 1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 Connection checked out
dbug: MongoDB.Command[0]
      1 1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 25326 4 1 Command started insert ftp-uploader { "insert" : "uploadstest123", "ordered" : true, "txnNumber" : NumberLong(1), "$db" : "ftp-uploader", "lsid" : { "id" : CSUUID("b9318af2-9d3e-487f-aefd-fd590cec4009") }, "apiVersion" : "1", "documents" : [{ "_id" : ObjectId("640a1bceb81f131c6bbf2952"), "CreatedOn" : ISODate("2023-03-09T17:47:58.68Z"), "ModifiedOn" : null, "SchemaVersion" : 1, "Data" : { "ConnectionId" : "conn-1234", "BlobReference" : CSUUID("0ea21353-7472-4256-949f-8fcdf032fa44"), "Filename" : "testing123.txt", "Status" : "New", "ErrorDescription" : null } }] } 6408d0cb1ff0b2b854a867ab
dbug: MongoDB.Command[0]
      1 1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 25326 4 1 Command succeeded insert 38.4526 { "n" : 1, "ok" : 1.0, "$clusterTime" : { "clusterTime" : Timestamp(1678384079, 1), "signature" : { "hash" : new BinData(0, "gzCDjo7cWc4oUVg1I1P4Oyf89Go="), "keyId" : NumberLong("7206850029133234182") } }, "operationTime" : Timestamp(1678384079, 1) } 6408d0cb1ff0b2b854a867ab
dbug: MongoDB.Connection[0]
      1 1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 Connection checking in
dbug: MongoDB.Connection[0]
      1 1 timeout-testing-jeff-lb.izjha.mongodb.net 27017 Connection checked in
Inserted with ref 0ea21353-7472-4256-949f-8fcdf032fa44
Completed in 529ms
```

## When the timeout occurs

```log
dbug: MongoDB.Connection[0]
      1 1 p9cd6-3m-acceptance-lb.izjha.mongodb.net 27017 Connection failed MongoDB.Driver.MongoConnectionException: An exception occurred while receiving a message from the server.
       ---> System.IO.IOException: Unable to read data from the transport connection: Connection timed out.
       ---> System.Net.Sockets.SocketException (110): Connection timed out
         --- End of inner exception stack trace ---
         at System.Net.Sockets.Socket.AwaitableSocketAsyncEventArgs.ThrowException(SocketError error, CancellationToken cancellationToken)
         at System.Net.Sockets.Socket.AwaitableSocketAsyncEventArgs.System.Threading.Tasks.Sources.IValueTaskSource<System.Int32>.GetResult(Int16 token)
         at System.Net.Security.SslStream.EnsureFullTlsFrameAsync[TIOAdapter](CancellationToken cancellationToken)
         at System.Runtime.CompilerServices.PoolingAsyncValueTaskMethodBuilder`1.StateMachineBox`1.System.Threading.Tasks.Sources.IValueTaskSource<TResult>.GetResult(Int16 token)
         at System.Net.Security.SslStream.ReadAsyncInternal[TIOAdapter](Memory`1 buffer, CancellationToken cancellationToken)
         at System.Runtime.CompilerServices.PoolingAsyncValueTaskMethodBuilder`1.StateMachineBox`1.System.Threading.Tasks.Sources.IValueTaskSource<TResult>.GetResult(Int16 token)
         at System.Threading.Tasks.ValueTask`1.ValueTaskSourceAsTask.<>c.<.cctor>b__4_0(Object state)
      --- End of stack trace from previous location ---
         at MongoDB.Driver.Core.Misc.StreamExtensionMethods.ReadAsync(Stream stream, Byte[] buffer, Int32 offset, Int32 count, TimeSpan timeout, CancellationToken cancellationToken)
         at MongoDB.Driver.Core.Misc.StreamExtensionMethods.ReadBytesAsync(Stream stream, Byte[] buffer, Int32 offset, Int32 count, TimeSpan timeout, CancellationToken cancellationToken)
         at MongoDB.Driver.Core.Connections.BinaryConnection.ReceiveBufferAsync(CancellationToken cancellationToken)
         --- End of inner exception stack trace ---
dbug: MongoDB.Command[0]
      1 1 p9cd6-3m-acceptance-lb.izjha.mongodb.net 27017 35647 5 2 Command failed insert 953047.9342 MongoDB.Driver.MongoConnectionException: An exception occurred while receiving a message from the server.
       ---> System.IO.IOException: Unable to read data from the transport connection: Connection timed out.
       ---> System.Net.Sockets.SocketException (110): Connection timed out
         --- End of inner exception stack trace ---
         at System.Net.Sockets.Socket.AwaitableSocketAsyncEventArgs.ThrowException(SocketError error, CancellationToken cancellationToken)
         at System.Net.Sockets.Socket.AwaitableSocketAsyncEventArgs.System.Threading.Tasks.Sources.IValueTaskSource<System.Int32>.GetResult(Int16 token)
         at System.Net.Security.SslStream.EnsureFullTlsFrameAsync[TIOAdapter](CancellationToken cancellationToken)
         at System.Runtime.CompilerServices.PoolingAsyncValueTaskMethodBuilder`1.StateMachineBox`1.System.Threading.Tasks.Sources.IValueTaskSource<TResult>.GetResult(Int16 token)
         at System.Net.Security.SslStream.ReadAsyncInternal[TIOAdapter](Memory`1 buffer, CancellationToken cancellationTo... 640855d4039f92600d14c171
```

For more details see the log files in the logs directory. Above log is from [logs/azure-serverless-log-try2.txt](logs/azure-serverless-log-try2.txt).