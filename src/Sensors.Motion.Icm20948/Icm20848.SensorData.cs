using Meadow.Foundation.Spatial;
using Meadow.Units;
using System;
using System.ComponentModel;

namespace Meadow.Foundation.Sensors.Motion
{
    public partial class Icm20948
    {
        /// <summary>
        /// A structure to hold all data from IMU.
        /// </summary>
        [Serializable]
        [ImmutableObject(true)]
        public struct SensorData : IEquatable<SensorData>
        {
            /// <summary>
            /// 
            /// </summary>
            public Acceleration3D Acceleration3D { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public AngularVelocity3D AngularVelocity3D { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public MagneticField3D MagneticField3D { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Quaternion QuaternionOrientation { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public EulerAngles EulerOrientation { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Units.Temperature Temperature { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool Equals(SensorData other)
            {
                return Acceleration3D == other.Acceleration3D &&
                    AngularVelocity3D == other.AngularVelocity3D &&
                    MagneticField3D  == other.MagneticField3D &&
                    QuaternionOrientation.W == other.QuaternionOrientation.W &&
                    QuaternionOrientation.X == other.QuaternionOrientation.X &&
                    QuaternionOrientation.Y == other.QuaternionOrientation.Y &&
                    QuaternionOrientation.Z == other.QuaternionOrientation.Z &&
                    EulerOrientation.Heading == other.EulerOrientation.Heading &&
                    EulerOrientation.Roll == other.EulerOrientation.Roll &&
                    EulerOrientation.Pitch == other.EulerOrientation.Pitch &&
                    Temperature == other.Temperature
                    ;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            public static bool operator ==(SensorData left, SensorData right)
            {
                return object.Equals(left, right);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            public static bool operator !=(SensorData left, SensorData right)
            {
                return !object.Equals(left, right);
            }
        }
    }
}