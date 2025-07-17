using Meadow;
using Meadow.Foundation.Sensors.Motion;

namespace OrchardCore.Blazor.Meadow.Services
{
    public class SensorService : IDisposable
    {
        public static int? Decimals = null;
        private Icm20948? _sensor;

        public string AngleValue { get; private set; } = "-";
        public string AccelerationValue { get; private set; } = "-";
        public string GyroscopeValue { get; private set; } = "-";
        public string MagneticValue { get; private set; } = "-";
        public string TemperatureValue { get; private set; } = "-";

        public event Action<IChangeResult<Icm20948.SensorData>>? StateChanged;

        public SensorService()
        {
            Task.Run(InitializeAsync);
        }
        public async Task InitializeAsync()
        {
            _sensor = Resolver.Services.Get<Icm20948>();
            if (_sensor == null)
            {
                AngleValue = AccelerationValue = GyroscopeValue = MagneticValue = TemperatureValue = "not found";
                return;
            }
            _sensor.Updated += SensorUpdated;
            _sensor.StartUpdating(TimeSpan.FromSeconds(2));
        }

        public static bool AreEqual(double? a, double? b, int? decimals = null)
        {
            if (a == null && b == null)
                return true;
            if (a == null || b == null)
                return false;
            return AreEqual(a.Value, b.Value, decimals);
        }

        public static bool AreEqual(double a, double b, int? decimals = null)
        {
            var digits = decimals ?? Decimals;
            return Round(a, digits) == Round(b, digits);
        }
        public static double Round(double a, int? decimals = null)
        {
            var digits = decimals ?? Decimals;
            if (digits == null)
                return Math.Round(a);
            return Math.Round(a, digits.Value);
        }
        private void SensorUpdated(object? sender, IChangeResult<Icm20948.SensorData> e)
        {
            AngleValue = $"Roll: {Round(e.New.EulerOrientation.Roll.Degrees)}     Pitch: {Round(e.New.EulerOrientation.Pitch.Degrees)}     Yaw: {Round(e.New.EulerOrientation.Heading.Degrees)}";
            AccelerationValue = $"X: {Round(e.New.Acceleration3D.X.Gravity)}     Y: {Round(e.New.Acceleration3D.Y.Gravity)}     Z: {Round(e.New.Acceleration3D.Z.Gravity)}";
            MagneticValue = $"X: {Round(e.New.MagneticField3D.X.Tesla)}     Y: {Round(e.New.MagneticField3D.Y.Tesla)}     Z: {Round(e.New.MagneticField3D.Z.Tesla)}";
            GyroscopeValue = $"X: {Round(e.New.AngularVelocity3D.X.RadiansPerSecond)}     Y: {Round(e.New.AngularVelocity3D.Y.RadiansPerSecond)}     Z: {Round(e.New.AngularVelocity3D.Z.RadiansPerSecond)}";
            TemperatureValue = $"{Round(e.New.Temperature.Celsius)}°C";

            //if (e.Old == null || e.Old.Value != e.New)
            StateChanged?.Invoke(e);
        }

        public void Dispose()
        {
            if (_sensor != null)
            {
                _sensor.StopUpdating();
                _sensor.Updated -= SensorUpdated;
//                _sensor.Dispose();
            }
        }
    }
}