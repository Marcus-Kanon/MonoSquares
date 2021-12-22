using MonoSquaresClient;
using System;
using System.Threading.Tasks;

namespace MonoSquares
{
    public static class Program
    {
        //[STAThread]
        static async Task Main( string[] args)
        {
            /*using (var game = new Game1())
                game.Run();*/
            //MonoSquaresClient.Client client = new MonoSquaresClient.Client();
            await Client.Run();

            Console.Read();

        }
    }
}
