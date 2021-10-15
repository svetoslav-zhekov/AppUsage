using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUsage
{
    class AppUsageTime
    {
        public int days { get; set; }
        public int hours { get; set; }
        public int minutes { get; set; }
        public int seconds { get; set; }

        public AppUsageTime(int days, int hours, int minutes, int seconds)
        {
            addDays(days);
            addHours(hours);
            addMinutes(minutes);
            addSeconds(seconds);
        }

        /// Main Functions
        public void addSeconds(int seconds)
        {
            if (seconds < 0)
            {
                throw new ArgumentException($"Invalid amount of seconds! Seconds cant be a negative amount!");
            }
            else if (seconds >= 60)
            {
                convertToMinutes(seconds);
            }
            else
            {
                this.seconds += seconds;

                if (this.seconds >= 60)
                {
                    int temp = this.seconds;
                    this.seconds = 0;
                    addSeconds(temp);
                }
            }
        }

        public void addMinutes(int minutes)
        {
            if (minutes < 0)
            {
                throw new ArgumentException($"Invalid amount of minutes! Minutes cant be a negative amount!");
            }
            else if (minutes >= 60)
            {
                convertToHours(minutes);
            }
            else
            {
                this.minutes += minutes;

                if (this.minutes >= 60)
                {
                    int temp = this.minutes;
                    this.minutes = 0;
                    addMinutes(temp);
                }
            }
        }

        public void addHours(int hours)
        {
            if (hours < 0)
            {
                throw new ArgumentException($"Invalid amount of hours! Hours cant be a negative amount!");
            }
            else if (hours >= 24)
            {
                convertToDays(hours);
            }
            else
            {
                this.hours += hours;

                if (this.hours >= 24)
                {
                    int temp = this.hours;
                    this.hours = 0;
                    addHours(temp);
                }
            }
        }

        public void addDays(int days)
        {
            if (days < 0)
            {
                throw new ArgumentException($"Invalid amount of days! Days cant be a negative amount");
            }
            else
            {
                this.days += days;
            }
        }

        ///Secondary Functions
        private void convertToMinutes(int seconds)
        {
            int minutes = (seconds - seconds % 60) / 60;
            addMinutes(minutes);
            addSeconds(seconds - (minutes * 60));
        }

        private void convertToHours(int minutes)
        {
            int hours = (minutes - minutes % 60) / 60;
            addHours(hours);
            addMinutes(minutes - (hours * 60));
        }

        private void convertToDays(int hours)
        {
            int days = (hours - hours % 24) / 24;
            addDays(days);
            addHours(hours - (days * 24));
        }

    }
}
