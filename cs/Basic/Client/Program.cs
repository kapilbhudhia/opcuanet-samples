// Copyright (c) Traeger Industry Components GmbH. All Rights Reserved.

namespace Client
{
    using System;
    using Opc.UaFx.Client;
    using Opc.UaFx;
    using System.Threading;

    /// <summary>
    /// This sample demonstrates how to implement a primitive OPC UA client.
    /// </summary>
    public class Program
    {
        #region ---------- Public static methods ----------

        public static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Enter ip address of the server (or localhost)");
            }
            string serverIP = args[0];

            //// If the server domain name does not match localhost just replace it
            //// e.g. with the IP address or name of the server machine.

            //string serverURL = "https://192.168.0.102:4840/SampleServer";
            //string serverURL = "opc.tcp://localhost:4840/SampleServer";
            string serverURL = $"opc.tcp://{serverIP}:4840/SampleServer";
            #region 1st Way: Use the OpcClient class.
            {
                // The OpcClient class interacts with one OPC UA server. While this class
                // provides session based access to the different OPC UA services of the
                // server, it does not implement a main loop.
                //var client = new OpcClient("opc.tcp://localhost:4840/SampleServer");

                try
                {
                    var client = new OpcClient(serverURL);
                    Console.WriteLine($"Connected on {serverURL}");
                    client.Connect();
                    var node = client.BrowseNode(OpcObjectTypes.ObjectsFolder);
                    Browse(node);

                    while (true)
                    {
                        Program.CommunicateWithServer(client);
                        Thread.Sleep(1000);

                    }

                    client.Disconnect();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            #endregion

            #region 2nd Way: Use the OpcClientApplication class.
            {
                //// The OpcClientApplication class uses a single OpcClient instance which is
                //// wrapped within a main loop.
                ////
                //// Remarks
                //// - The app instance starts a main loop when the session to the server has
                ////   been established.
                //// - Custom client/session dependent code have to be implemented within the event
                ////   handler of the Started event.
                //var app = new OpcClientApplication("opc.tcp://localhost:4840/SampleServer");
                //app.Started += Program.HandleAppStarted;

                //app.Run();
            }
            #endregion
        }

        #endregion

        #region ---------- Private static methods ----------

        private static void CommunicateWithServer(OpcClient client)
        {
            // Console.WriteLine("ReadNode: {0}", client.ReadNode("ns=2;s=Machine_1/IsActive"));
            // client.WriteNode("ns=2;s=Machine_1/IsActive", false);
            // Console.WriteLine("ReadNode: {0}", client.ReadNode("ns=2;s=Machine_1/IsActive"));

            Console.WriteLine("ReadNode: {0}", client.ReadNode("ns=2;s=Temperature"));
        }
        private static void Browse(OpcNodeInfo node, int level = 0)
        {
            Console.WriteLine("{0}{1}({2})",
                    new string('.', level * 4),
                    node.Attribute(OpcAttribute.DisplayName).Value,
                    node.NodeId);

            level++;

            foreach (var childNode in node.Children())
                Browse(childNode, level);
        }
        private static void HandleAppStarted(object sender, EventArgs e)
        {
            Program.CommunicateWithServer(((OpcClientApplication)sender).Client);
        }

        #endregion
    }
}
