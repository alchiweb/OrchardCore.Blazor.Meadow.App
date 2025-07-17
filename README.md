# Meadow Orchard CMS with ASP.NET Aspire

## This is a Proof of Concept (POC)
 This Blazor app is a POC for using together:
 ![Sensor data in Charts (with Ant Blazor)](assets/sensor_charts.gif)

 - Meadow framework with Jetson Orin Nano
 - a new Meadow driver for the ICM20948 sensors (included on IMX219-83 dual cameras kit), converted from a C linux driver
 - Orchard Core: an open-source .net Foundation project, that can be used as a CMS or a framework
 - Blazor (server mode) with Orchard Core
 - Ant Blazor components (Ant Blazor Pro + Ant Blazor Charts))
 - Aspire with Orchard Core (-> docker)

 ![Aspire + Orchard Core admin + Orchard Core content in Blazor](assets/aspire_orchard_core.gif)

 ## Test without hardware
 Note: if you haven't the hardware, it's possible to test with fake data, even if you are on Windows.
 
 In the following file:
 ```
 src\OrchardCore.Blazor.Meadow\Pages\Sensor.razor
 ```
 Uncomment the line (after the comment):
 ```csharp
        // Simulate data for testing purposes
        Task.Run(async () => await AddFakeData());
 ```
 ## Thanks
 Thanks to:
 - https://github.com/nubiquest-blogs/orchard-core-with-aspire for the tutorial for Aspire with Orchard Core
 - https://docs.orchardcore.net/en/main/guides/create-blazor-cms/ for Blazor with Orchard Core
 - https://github.com/mtmal/ICM20948-linux-i2c for the ICM20948 linux driver (in C language)
