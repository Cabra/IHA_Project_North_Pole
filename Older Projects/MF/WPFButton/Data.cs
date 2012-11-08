using System;
using Microsoft.SPOT;

namespace ButtonNETMF
{
    public class Time
    {
        public int hour;
        public int minute;

        public Time()
        {
            hour = 0;
            minute = 0;
        }
    }

    class Data
    {
        TemperatureMeter TempMeter;
        int updateTime=300000;
        public bool fahrenheit = false;
        int weatherType = 0;
        public int humidity;
        public Time sunrise=new Time();
        public Time sunset=new Time();
        public Time current = new Time();
        public string WOEID = "12718362";
        public string location = "";

        public Data()
        {
            TempMeter = new TemperatureMeter();
        }

        public void setUpdateTime(int n)
        {
            updateTime = n * 60 * 1000;
        }

        public int getUpdateTime()
        {
            return (updateTime/1000)/60;
        }

        public int ReadValue()
        {
            double value = TempMeter.ReadValue();
            if (fahrenheit)
                return (int)(value * 1.8 + 32);
            else
                return (int)value;
        }

        public int getWeatherType()
        {
            return weatherType;
        }

        public void setWeatherType(int n)
        {
            //SOS
            if (n == 0 || n == 1 || n == 2 || n == 3)
            {
                weatherType = 0;
                return;
            }
            //Sun & Night
            if (n >= 31 && n <= 36 || n>=23&&n<=25)
                if (current.hour <= sunset.hour && current.hour >= sunrise.hour && current.minute <= sunset.minute && current.minute>=sunrise.minute)
                {
                    weatherType = 1;
                    return;
                }
                else
                {
                    weatherType = 3;
                    return;
                }
            //Cloudy Day && Cloudy Night
            if ( n == 30 || n == 22 || n == 20 || n==29 )
                if (current.hour <= sunset.hour && current.hour >= sunrise.hour && current.minute <= sunset.minute && current.minute >= sunrise.minute)
                {
                    weatherType = 2;
                    return;
                }
                else
                {
                    weatherType = 4;
                    return;
                }
            //Storm
            if (n == 47 || n == 45 || n == 38 || n == 39 || n==37 || n == 4 )
            {
                weatherType = 5;
                return;
            }
            //Small Rain
            if ( n == 40 )
            {
                weatherType = 6;
                return;
            }
            //Rain
            if (n == 11 || n == 12 || n == 40 )
            {
                weatherType = 7;
                return;
            }
            //Little Snow
            if (n == 5 || n == 6 || n == 7 || n == 8 || n == 9 || n == 10 || n == 13 || n == 14 || n == 17 || n == 18 || n == 42)
            {
                weatherType = 8;
                return;
            }
            //Cloudy
            if (n == 19 || n == 21 || n == 26 || n == 27 || n == 28 || n == 44 )
            {
                weatherType = 9;
                return;
            }
            //Snow
            if (n == 15 || n == 16 || n == 41 || n == 43 || n == 46 )
            {
                weatherType = 10;
                return;
            }

        }
    }
}
