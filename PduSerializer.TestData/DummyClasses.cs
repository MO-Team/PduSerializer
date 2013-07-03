using System;
using System.Collections;
using System.Collections.Generic;

namespace PduSerializer.TestData
{
    [PduMessage]
    public struct HeavyMessage
    {
        [Field(Position = 34)] public UInt16 AverageError;
        [Field(Position = 15), PaddedString(6)] public string Callsign;
        [Field(Position = 39)] public UInt16 CameraHeadingAngle;
        [Field(Position = 40)] public UInt16 CameraHeadingAngleError;
        [Field(Position = 32)] public Byte CameraKind;
        [Field(Position = 41)] public short CameraPitchAngle;
        [Field(Position = 42)] public UInt16 CameraPitchAngleError;
        [Field(Position = 43)] public short CameraRollAngle;
        [Field(Position = 44)] public UInt16 CameraRollAngleError;
        [Field(Position = 33)] public Byte ChannelKind;
        [Field(Position = 47)] public Byte Clearance;
        [Field(Position = 7)] public UInt32 Crc;
        [Field(Position = 5)] public MatipDate Date;
        [Field(Position = 37)] public Byte DesignationStatus;
        [Field(Position = 51)] public double DoubleField;

        // Identification

        [Field(Position = 9)] public UInt16 Environment;
        [Field(Position = 52)] public float FloatField;
        [Field(Position = 27)] public UInt16 HeightPrecision;
        [Field(Position = 35)] public UInt16 HorizontalFieldOfView;
        [Field(Position = 19)] public UInt32 IpAddress;
        [Field(Position = 11)] public Byte IsIdentified;
        [Field(Position = 31)] public Byte IsR;
        [Field(Position = 8)] public UInt32 MatrixId;
        [Field(Position = 2)] public UInt16 MessageId;
        [Field(Position = 1)] public UInt16 MessageSize;
        [Field(Position = 28)] public UInt16 Precision;
        [Field(Position = 17)] public Byte NumberInGroup;
        [Field(Position = 46)] public Byte Number;
        [Field(Position = 16), PaddedString(8)] public string OperCallsign;
        [Field(Position = 14), PaddedString(15)] public string OperationName;
        [Field(Position = 4)] public UInt16 Padding1;
        [Field(Position = 29)] public UInt16 Padding2;
        [Field(Position = 38)] public Byte Padding3;

        //  Position & Azimut
        [Field(Position = 20)] public Location ThreeDPos;
        [Field(Position = 21)] public UInt16 HeadingAngle;
        [Field(Position = 22)] public UInt16 HeadingAngleError;
        [Field(Position = 23)] public short PitchAngle;
        [Field(Position = 24)] public UInt16 PitchAngleError;
        [Field(Position = 25)] public short RollAngle;
        [Field(Position = 26)] public UInt16 RollAngleError;
        [Field(Position = 12)] public Byte PlatformType;
        [Field(Position = 49)] public UInt32 Reserved1;
        [Field(Position = 50)] public UInt32 Reserved2;
        [Field(Position = 10)] public UInt16 SquadronNumber;
        [Field(Position = 13)] public UInt16 TailNumber;
        [Field(Position = 6)] public UInt32 TimeTag;
        [Field(Position = 45)] public Location TraceCenter3DPos;
        public Location[] TraceInfo; //8
        [Field(Position = 30)] public Byte TraceShape;
        [Field(Position = 18)] public UInt16 UdpPort;
        [Field(Position = 3)] public UInt16 VersionMatip;
        [Field(Position = 36)] public UInt16 VerticalFieldOfView;
    }


    [PduMessage]
    public struct MatipDate
    {
        [Field(Position = 1)] public Byte Day;
        [Field(Position = 2)] public Byte Month;
        [Field(Position = 3)] public UInt16 Year;
    }

    [PduMessage]
    public struct Location
    {
        [Field(Position = 3)] public Int16 Height;
        [Field(Position = 1)] public Int32 Lat;
        [Field(Position = 2)] public Int32 Lon;
    }


    [PduMessage]
    public struct PduPerson
    {
        [Field(Position = 3)] public int Age;
        [Field(Position = 6), EnumSerialize] public IntEnum Gender;
        [Field(Position = 4)] public int Id;
        [Field(Position = 2), PaddedString(10, padding : (char) 0)] public string LastName;
        [Field(Position = 1), PaddedString(10, Alignment.Left, (char) 0)] public string Name;
        [Field(Position = 5)] public double UsaShoeSize;
    }
}