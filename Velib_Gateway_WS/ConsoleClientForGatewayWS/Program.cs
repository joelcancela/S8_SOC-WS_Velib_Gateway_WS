using System;
using VelibGatewayService;
namespace ConsoleClientForGatewayWS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Console client for VelibGatewayWS (async)");
            Console.WriteLine("see help for commands");
            Console.Write("#> ");
            string userinput = Console.ReadLine();
            while (true)
            {
                process(userinput);
                Console.Write("#> ");
                userinput = Console.ReadLine();
            }
        }

        static async void process(string input)
        {
            if (input.Contains("getcities"))
            {
                String[] cities = await new VelibServiceClient().GetCitiesAsync();
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
                catch (Exception e)
                {
                    Console.WriteLine("bad syntax, see help for details");
                }

                String[] stations = null;
                try
                {
                   stations = await new VelibServiceClient().GetStationsAsync(city);
                }
                catch (Exception e)
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
                    bikes = await new VelibServiceClient().GetAvailableVelibsAsync(input.Split("getbikes")[0]);
                }
                catch (Exception e)
                {
                    Console.WriteLine("bad station provided");
                    Console.Write("#> ");
                    return;
                }
                Console.WriteLine("Available bikes for " + input.Split("getbikes")[0]+": "+bikes);
                Console.Write("#> ");
            }
            else if (input.Contains("help"))
            {
                Console.WriteLine("Available commands:");
                Console.WriteLine("getcities - Retrieves the available cities");
                Console.WriteLine("getstations <city> - Lists the stations name for the city");
                Console.WriteLine("getbikes <stationName> - Gets the number of the available bikes for the station provided");
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
