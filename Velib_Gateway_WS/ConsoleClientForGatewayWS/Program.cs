using System;
using VelibGatewayService;

namespace ConsoleClientForVelibGatewayWS
{
    class Program
    {
        static VelibServiceClient client = new VelibServiceClient();
        static void Main(string[] args)
        {
            Console.WriteLine("Console client for VelibGatewayWS (async)");
            Console.WriteLine("type help for commands");
            Console.Write("#> ");
            string userinput = Console.ReadLine();
            while (true)
            {
                Process(userinput);
                Console.Write("#> ");
                userinput = Console.ReadLine();
            }
        }

        static async void Process(string input)
        {
            if (input.Contains("getcities"))
            {
                String[] cities = await client.GetCitiesAsync();
                foreach (string city in cities)
                {
                    Console.Write(city + " ");
                }
                Console.Write("\n#> ");
            }
            else if (input.Contains("getstations"))

            {
                string city = null;
                try
                {
                    city = input.Split(" ")[1];
                }
                catch (Exception)
                {
                    Console.WriteLine("bad syntax, see help for details");
                }

                String[] stations = null;
                try
                {
                   stations = await client.GetStationsAsync(city);
                }
                catch (Exception)
                {
                    Console.WriteLine("bad city provided");
                    Console.Write("#> ");
                    return;
                }
                foreach (string station in stations)
                {
                    Console.WriteLine(station + " ");
                }
                Console.Write("#> ");
            }
            else if (input.Contains("getbikes"))
            {
                int bikes = 0;
                try
                {
                    string stationName = input.Split("getbikes ")[1];
                    bikes = await client.GetAvailableVelibsAsync(stationName);
                }
                catch (Exception)
                {
                    Console.WriteLine("bad station provided");
                    Console.Write("#> ");
                    return;
                }
                Console.WriteLine("Available bikes for station: '" + input.Split("getbikes ")[1]+"': "+bikes);
                Console.Write("#> ");
            }
            else if (input.Contains("help"))
            {
                Console.WriteLine("Available commands:");
                Console.WriteLine("getcities - Retrieves the available cities");
                Console.WriteLine("getstations <city> - Lists the stations name for the city");
                Console.WriteLine("getbikes <stationName> - Gets the number of the available bikes for the station provided");
                Console.WriteLine("exit - Close the client");
            }
            else if (input.Contains("exit"))
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("bad command, see help for details");

            }
        }
    }
}
