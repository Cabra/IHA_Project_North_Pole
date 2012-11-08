using System;
using Microsoft.SPOT;

namespace ButtonNETMF
{
    class DayData
    {
        public double temperature;
        public double humidity;

        public DayData(string t, string h)
        {
            if (t != "null" && t != "NaN")
                temperature = double.Parse(t);
            else
                temperature = -666;
            if (h != "null" && h != "NaN")
                humidity = double.Parse(h);
            else
                humidity = -666;
        }

        public override string ToString()
        {
            return "Temperature:" + temperature + "\tHumidity:" + humidity;
        }
    }
}
