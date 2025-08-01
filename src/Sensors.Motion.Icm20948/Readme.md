# Meadow.Foundation.Sensors.Motion.Icm20948

**Bosch Icm20948 I2C 9 axis absolute orientation sensor**

The **Icm20948** library is included in the **Meadow.Foundation.Sensors.Motion.Icm20948** nuget package and is designed for the [Wilderness Labs](www.wildernesslabs.co) Meadow .NET IoT platform.

This driver is part of the [Meadow.Foundation](https://developer.wildernesslabs.co/Meadow/Meadow.Foundation/) peripherals library, an open-source repository of drivers and libraries that streamline and simplify adding hardware to your C# .NET Meadow IoT applications.

For more information on developing for Meadow, visit [developer.wildernesslabs.co](http://developer.wildernesslabs.co/).

To view all Wilderness Labs open-source projects, including samples, visit [github.com/wildernesslabs](https://github.com/wildernesslabs/).

## Installation

You can install the library from within Visual studio using the the NuGet Package Manager or from the command line using the .NET CLI:

`dotnet add package Meadow.Foundation.Sensors.Motion.Icm20948`
## Usage

```csharp
Icm20948 sensor;

public override Task Initialize()
{
    Resolver.Log.Info("Initialize...");

    // create the sensor driver
    sensor = new Icm20948(Device.CreateI2cBus());

    // classical .NET events can also be used:
    sensor.Updated += (sender, result) =>
    {
        Resolver.Log.Info($"Accel: [X:{result.New.Acceleration3D?.X.MetersPerSecondSquared:N2}," +
            $"Y:{result.New.Acceleration3D?.Y.MetersPerSecondSquared:N2}," +
            $"Z:{result.New.Acceleration3D?.Z.MetersPerSecondSquared:N2} (m/s^2)]");

        Resolver.Log.Info($"Gyro: [X:{result.New.AngularVelocity3D?.X.DegreesPerSecond:N2}," +
            $"Y:{result.New.AngularVelocity3D?.Y.DegreesPerSecond:N2}," +
            $"Z:{result.New.AngularVelocity3D?.Z.DegreesPerSecond:N2} (degrees/s)]");

        Resolver.Log.Info($"Compass: [X:{result.New.MagneticField3D?.X.Tesla:N2}," +
            $"Y:{result.New.MagneticField3D?.Y.Tesla:N2}," +
            $"Z:{result.New.MagneticField3D?.Z.Tesla:N2} (Tesla)]");

        Resolver.Log.Info($"Gravity: [X:{result.New.GravityVector?.X.MetersPerSecondSquared:N2}," +
            $"Y:{result.New.GravityVector?.Y.MetersPerSecondSquared:N2}," +
            $"Z:{result.New.GravityVector?.Z.MetersPerSecondSquared:N2} (meters/s^2)]");

        Resolver.Log.Info($"Quaternion orientation: [X:{result.New.QuaternionOrientation?.X:N2}," +
            $"Y:{result.New.QuaternionOrientation?.Y:N2}," +
            $"Z:{result.New.QuaternionOrientation?.Z:N2}]");

        Resolver.Log.Info($"Euler orientation: [heading: {result.New.EulerOrientation?.Heading:N2}," +
            $"Roll: {result.New.EulerOrientation?.Roll:N2}," +
            $"Pitch: {result.New.EulerOrientation?.Pitch:N2}]");

        Resolver.Log.Info($"Linear Accel: [X:{result.New.LinearAcceleration?.X.MetersPerSecondSquared:N2}," +
            $"Y:{result.New.LinearAcceleration?.Y.MetersPerSecondSquared:N2}," +
            $"Z:{result.New.LinearAcceleration?.Z.MetersPerSecondSquared:N2} (meters/s^2)]");

        Resolver.Log.Info($"Temp: {result.New.Temperature?.Celsius:N2}C");
    };

    // Example that uses an IObservable subscription to only be notified when the filter is satisfied
    var consumer = Icm20948.CreateObserver(
        handler: result => Resolver.Log.Info($"Observer: [x] changed by threshold; new [x]: X:{result.New.Acceleration3D?.X.MetersPerSecondSquared:N2}, old: X:{result.Old?.Acceleration3D?.X.MetersPerSecondSquared:N2}"),
        // only notify if there's a greater than 1 micro tesla on the Y axis

        filter: result =>
        {
            if (result.Old is { } old)
            {
                return ((result.New.Acceleration3D - old.Acceleration3D)?.Y > new Acceleration(1, AU.MetersPerSecondSquared));
            }
            return false;
        });
    sensor.Subscribe(consumer);

    return Task.CompletedTask;
}

public async override Task Run()
{
    await ReadConditions();

    sensor.StartUpdating(TimeSpan.FromMilliseconds(500));
}

protected async Task ReadConditions()
{
    var result = await sensor.Read();
    Resolver.Log.Info("Initial Readings:");
    Resolver.Log.Info($"Accel: [X:{result.Acceleration3D?.X.MetersPerSecondSquared:N2}," +
        $"Y:{result.Acceleration3D?.Y.MetersPerSecondSquared:N2}," +
        $"Z:{result.Acceleration3D?.Z.MetersPerSecondSquared:N2} (m/s^2)]");

    Resolver.Log.Info($"Gyro: [X:{result.AngularVelocity3D?.X.DegreesPerSecond:N2}," +
        $"Y:{result.AngularVelocity3D?.Y.DegreesPerSecond:N2}," +
        $"Z:{result.AngularVelocity3D?.Z.DegreesPerSecond:N2} (degrees/s)]");

    Resolver.Log.Info($"Compass: [X:{result.MagneticField3D?.X.Tesla:N2}," +
        $"Y:{result.MagneticField3D?.Y.Tesla:N2}," +
        $"Z:{result.MagneticField3D?.Z.Tesla:N2} (Tesla)]");

    Resolver.Log.Info($"Gravity: [X:{result.GravityVector?.X.MetersPerSecondSquared:N2}," +
        $"Y:{result.GravityVector?.Y.MetersPerSecondSquared:N2}," +
        $"Z:{result.GravityVector?.Z.MetersPerSecondSquared:N2} (meters/s^2)]");

    Resolver.Log.Info($"Quaternion orientation: [X:{result.QuaternionOrientation?.X:N2}," +
        $"Y:{result.QuaternionOrientation?.Y:N2}," +
        $"Z:{result.QuaternionOrientation?.Z:N2}]");

    Resolver.Log.Info($"Euler orientation: [heading: {result.EulerOrientation?.Heading:N2}," +
        $"Roll: {result.EulerOrientation?.Roll:N2}," +
        $"Pitch: {result.EulerOrientation?.Pitch:N2}]");

    Resolver.Log.Info($"Linear Accel: [X:{result.LinearAcceleration?.X.MetersPerSecondSquared:N2}," +
        $"Y:{result.LinearAcceleration?.Y.MetersPerSecondSquared:N2}," +
        $"Z:{result.LinearAcceleration?.Z.MetersPerSecondSquared:N2} (meters/s^2)]");

    Resolver.Log.Info($"Temp: {result.Temperature?.Celsius:N2}C");
}
        
```
## How to Contribute

- **Found a bug?** [Report an issue](https://github.com/WildernessLabs/Meadow_Issues/issues)
- Have a **feature idea or driver request?** [Open a new feature request](https://github.com/WildernessLabs/Meadow_Issues/issues)
- Want to **contribute code?** Fork the [Meadow.Foundation](https://github.com/WildernessLabs/Meadow.Foundation) repository and submit a pull request against the `develop` branch


## Need Help?

If you have questions or need assistance, please join the Wilderness Labs [community on Slack](http://slackinvite.wildernesslabs.co/).
## About Meadow

Meadow is a complete, IoT platform with defense-grade security that runs full .NET applications on embeddable microcontrollers and Linux single-board computers including Raspberry Pi and NVIDIA Jetson.

### Build

Use the full .NET platform and tooling such as Visual Studio and plug-and-play hardware drivers to painlessly build IoT solutions.

### Connect

Utilize native support for WiFi, Ethernet, and Cellular connectivity to send sensor data to the Cloud and remotely control your peripherals.

### Deploy

Instantly deploy and manage your fleet in the cloud for OtA, health-monitoring, logs, command + control, and enterprise backend integrations.


