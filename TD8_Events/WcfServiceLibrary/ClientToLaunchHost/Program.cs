﻿using EventsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace ClientToLaunchHost
{
    class Program
    {
        static void Main(string[] args)
        {

            //Create ServiceHost
            ServiceHost host  = new ServiceHost(typeof(CalcService));
            //Start the Service
            host.Open();
            Console.WriteLine("Service is host at " + DateTime.Now.ToString());
            Console.WriteLine("Host is running... Press <Enter> key to stop");
            Console.ReadLine();
            host.Close();
        }
    }
}
