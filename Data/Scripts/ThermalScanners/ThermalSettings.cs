using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using static ThermalScanners.ThermalSignatures;

namespace ThermalScanners

{
    [ProtoContract(SkipConstructor =true,UseProtoMembersOnly =true),Serializable]
    public class GlobalHeatSettings
    {
        [ProtoMember(1), XmlElement]
        public float LargeGridBaseRange { get; set; }
        [ProtoMember(2), XmlElement]
        public float SmallGridBaseRange { get; set; }
        [ProtoMember(3), XmlElement]
        public float WeatherMultiplier { get; set; }
        [ProtoMember(4), XmlElement]
        public float AtmosphericDensity { get; set; }
        [ProtoMember(5), XmlElement]
        public float GravityMultiplier { get; set; }

    }

    [ProtoContract, Serializable]
    public class HeatGenerator
    {
        [ProtoMember(6), XmlElement]
        public string SubtypeId { get; set; }
        [ProtoMember(7), XmlElement]
        public float HeatOutput { get; set; }
        [ProtoMember(8), XmlElement]
        public BlockCategory BlockCategory { get; set; }
        [ProtoMember(9), XmlElement]
        public float AirMultiplier { get; set; }
        [ProtoMember(10), XmlElement]
        public float WeatherMultiplier { get; set; }
        [ProtoMember(11), XmlElement]
        public GridSize GridSize { get; set; }
        [ProtoMember(12), XmlElement]
        public float SmallGridMultiplier { get; set; }
        [ProtoMember(13), XmlElement]
        public float LargeGridMultiplier { get; set; } = 1.0f;
        [ProtoMember(14), XmlElement]
        public float StationMultiplier { get; set; } = 1.0f;
    }
    [ProtoContract, Serializable]
    public struct MessagePacket
    {
        [ProtoMember(15), XmlElement]
        public List<HeatGenerator> heatGenerators { get; set; }
        [ProtoMember(16), XmlElement]
        public GlobalHeatSettings settings { get; set; }
    }
}
