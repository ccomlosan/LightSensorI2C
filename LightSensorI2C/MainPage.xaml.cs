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
using System.Threading.Tasks;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LightSensorI2C {
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page {

    enum SensorCommand { PowerDown = 0x0, PowerOn = 0x1, Reset = 0x7, OneTimeHMode = 0x20};

    private I2cDevice bh1750fviSensor_;
    private const int BH1750FVI_ADDR = 0x0;

    private DispatcherTimer timer_;

    public MainPage() {
      this.InitializeComponent();
      if (bh1750fviSensor_ != null) {
        initializeTimer();
      }
    }

    private void initializeTimer() {
      timer_ = new DispatcherTimer();
      timer_.Interval = TimeSpan.FromSeconds(1);
      timer_.Tick += timerTick;

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

    private void timerTick(object args, object sender) {
      byte[] cmd = new byte[1];
      byte[] data = new byte[2];
      cmd[0] = (byte)SensorCommand.PowerOn;
      bh1750fviSensor_.Write(cmd);
      cmd[0] = (byte)SensorCommand.OneTimeHMode;
      bh1750fviSensor_.WriteRead(cmd, data);

      //Task.Delay(120).Wait();
      //bh1750fviSensor_.Read(data);

    }

    private void buttonExit_Click(object sender, RoutedEventArgs e) {
      Application.Current.Exit();
    }
  }
}
