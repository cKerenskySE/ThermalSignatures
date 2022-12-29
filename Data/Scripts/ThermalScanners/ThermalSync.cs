using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using static ThermalScanners.ThermalSignatures;

namespace ThermalScanners
{
    public static class ThermalSync
    {
        public static ushort UModId = 19209;
        public static List<HeatGenerator> HeatGenerators;
        public static GlobalHeatSettings HeatSettings;

        public static HeatGenerator windValue;
        public static HeatGenerator solarValue;
        public static HeatGenerator hydrogenValue;
        public static HeatGenerator batteryValue;
        public static HeatGenerator reactorValue;
        public static List<HeatGenerator> thrusterValues = new List<HeatGenerator>();
        public static List<HeatGenerator> otherBlocks = new List<HeatGenerator>();


        public static void RegisterServer()
        {
            if (MyAPIGateway.Multiplayer.IsServer)
            {
               // MyLog.Default.WriteLineAndConsole($"Setting up Server Register");
                MyAPIGateway.Multiplayer.RegisterMessageHandler(UModId, serverResponse);
            }
        }

        public static void serverResponse(byte[] message)
        {
            var userID = MyAPIGateway.Utilities.SerializeFromBinary<long>(message);
           // MyLog.Default.WriteLineAndConsole($"Request Recieved from: {userID}");
            Send(new MessagePacket { heatGenerators = HeatGenerators, settings = HeatSettings });
        }

        public static void clientResponse(byte[] message)
        {
           // MyLog.Default.WriteLineAndConsole($"Message Recieved from Server");

            var heatObject = MyAPIGateway.Utilities.SerializeFromBinary<MessagePacket>(message);


           // MyLog.Default.WriteLineAndConsole($"Got Parseable object From Server");

            HeatSettings = heatObject.settings;
           // MyLog.Default.WriteLineAndConsole($"Set Heat Settings");

            HeatGenerators = heatObject.heatGenerators; ;
           // MyLog.Default.WriteLineAndConsole("Got updated list");
            foreach (var generator in ThermalSync.HeatGenerators)
            {
                if (generator.BlockCategory == BlockCategory.thrust)
                {
                    ThermalSync.thrusterValues.Add(generator);
                }
                if (generator.BlockCategory == BlockCategory.power)
                {
                    ThermalSync.otherBlocks.Add(generator);
                }
            }

            reactorValue = ThermalSync.HeatGenerators.Find(a =>
            {
                if (a.BlockCategory == BlockCategory.reactor)
                {
                    return true;
                }
                return false;
            });

            batteryValue = ThermalSync.HeatGenerators.Find(a =>
            {
                if (a.BlockCategory == BlockCategory.battery)
                {
                    return true;
                }
                return false;
            });

            hydrogenValue = ThermalSync.HeatGenerators.Find(a =>
            {
                if (a.BlockCategory == BlockCategory.hydrogen)
                {
                    return true;
                }
                return false;
            });

            solarValue = ThermalSync.HeatGenerators.Find(a =>
            {
                if (a.BlockCategory == BlockCategory.solar)
                {
                    return true;
                }
                return false;
            });

            windValue = ThermalSync.HeatGenerators.Find(a =>
            {
                if (a.BlockCategory == BlockCategory.wind)
                {
                    return true;
                }
                return false;
            });


        }

        public static void RequestUpdate(long userid)
        {
           // MyLog.Default.WriteLineAndConsole($"Requesting Update");
            if (!MyAPIGateway.Utilities.IsDedicated)
            {
               // MyLog.Default.WriteLineAndConsole($"Sending Message to Server");
                var serializedData = MyAPIGateway.Utilities.SerializeToBinary(userid);
                MyAPIGateway.Multiplayer.SendMessageToServer(UModId, serializedData);
            }

        }
        public static void Send(MessagePacket settings)
        {
           // MyLog.Default.WriteLineAndConsole($"Sending Info");
            var serializedData = MyAPIGateway.Utilities.SerializeToBinary(settings);
           // MyLog.Default.WriteLineAndConsole($"Sending Information");
            MyAPIGateway.Multiplayer.SendMessageToOthers(UModId, serializedData);
        }

        public static void Register()
        {
           // MyLog.Default.WriteLineAndConsole($"Registering with Server");
            MyAPIGateway.Multiplayer.RegisterMessageHandler(UModId, clientResponse);
        }


    }
}