using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Threading;

namespace ButtonNETMF
{
    class TemperatureMeter
    {
        static I2CDevice.Configuration config;
        static I2CDevice device;

        public TemperatureMeter()
        {
            config = new I2CDevice.Configuration(79, 100);
            device = new I2CDevice(config);
        }

        public double ReadValue()
        {
            double temp;
            byte[] inBuffer = new byte[2];
            I2CDevice.I2CReadTransaction readTransaction = I2CDevice.CreateReadTransaction(inBuffer);

            //execute both transactions
            I2CDevice.I2CTransaction[] transactions =
                new I2CDevice.I2CTransaction[] { readTransaction };

            int transferred = device.Execute(transactions,
                                             100 //timeout in ms
                                             );
            // The value is now converted
            temp = (float)(inBuffer[0] << 1) / 2;

            if ((inBuffer[1] >> 7) != 0)
                temp += (float)0.5;

            if ((inBuffer[0] >> 7) != 0)
                temp = -temp;

            Thread.Sleep(1000);
            return temp;
        }
    }
}
