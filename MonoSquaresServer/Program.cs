using MonoSquaresServer;
using MonoSquaresServer.Physics;

Console.WriteLine("Starting server....");

//var engine = new MonoSquaresServer.Physics.PhysicsEngine();

var server = new EchoServer();
server.Start();