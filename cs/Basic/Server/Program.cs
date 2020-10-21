// Copyright (c) Traeger Industry Components GmbH. All Rights Reserved.

using System;
using System.Threading;
using Opc.UaFx;

namespace Server
{
    using Opc.UaFx.Server;

    /// <summary>
    /// This sample demonstrates how to implement a primitive OPC UA server.
    /// </summary>
    public class Program
    {
        #region ---------- Public static methods ----------

        public static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Enter a valid ip address for the server (or localhost)");
                return;
            }
            string serverIP = args[0];
            #region 1st Way: Use the OpcServer class.
            {
                //// The OpcServer class interacts with one or more OPC UA clients using one of
                //// the registered base addresses of the server. While this class provides the
                //// different OPC UA services defined by OPC UA, it does not implement a main loop.

                //string serverURL = "https://localhost:4840/SampleServer";
                //string serverURL = "opc.tcp://localhost:4840/SampleServer";
                string serverURL = $"opc.tcp://{serverIP}:4840/SampleServer";
                var temperatureNode = new OpcDataVariableNode<double>("Temperature", 100.0);

                //using var server = new OpcServer(serverURL, temperatureNode);
                using var server = new OpcServer(serverURL, temperatureNode);
                server.Start();

                Console.WriteLine("Started the server at {0}", serverURL);

                while (true)
                {
                    if (temperatureNode.Value == 110)
                        temperatureNode.Value = 100;
                    else
                        temperatureNode.Value++;

                    temperatureNode.ApplyChanges(server.SystemContext);
                    Thread.Sleep(1000);
                }
            }
            #endregion

            #region 2nd Way: Use the OpcServerApplication class.
            {
                // The OpcServerApplication class uses a single OpcServer instance which is
                // wrapped within a main loop.
                //
                // Remarks
                // - The app instance does start a main loop when the server has been started.
                // - Custom startup code have to be implemented within the event handler of the
                //   Started event of the app instance.
                // new OpcServerApplication("opc.tcp://localhost:4840/SampleServer", new SampleNodeManager()).Run();
            }
            #endregion

            #region 3rd Way: Use the OpcServerServiceApplication class.
            {
                //// The OpcServerServiceApplication class uses a single OpcServer instance which is
                //// wrapped within a main loop when it is started with an interactive user or in
                //// debug mode. Otherwise it will start the process as a windows service which
                //// allows the application can be registered as a service process.
                ////
                //// Remarks
                //// - The app instance does start a main loop when the server has been started.
                //// - Custom startup code have to be implemented within the event handler of the
                ////   Started event of the app instance.
                //new OpcServerServiceApplication("opc.tcp://localhost:4840/SampleServer", new SampleNodeManager()).Run();
            }
            #endregion
        }

        #endregion
    }
}
