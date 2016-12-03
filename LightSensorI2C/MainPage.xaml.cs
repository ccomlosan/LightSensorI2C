using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Devices.I2c;
using Windows.Devices.Enumeration;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LightSensorI2C {
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page {
    private I2cDevice bh1750fviSensor_;
    private const int BH1750FVI_ADDR = 0x0;
    public MainPage() {
      this.InitializeComponent();
    }

    private async void initializeI2c() {
      //get a query, selector string for all the I2c controllers on the system
      string i2cDeviceSelector = I2cDevice.GetDeviceSelector();
      //find the I2c bus controller device with the selector string
      var devices = await DeviceInformation.FindAllAsync(i2cDeviceSelector).AsTask();
      //create the settings and specify the device address
      var settings = new I2cConnectionSettings(BH1750FVI_ADDR);
      bh1750fviSensor_ = await I2cDevice.FromIdAsync(devices[0].Id, settings);

    }
  }
}
