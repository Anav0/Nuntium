using System;

namespace Nuntium.Core
{
    public static class DateHelper
    {
        private static Random mRnd = new Random();

        //TODO: Make it BETTER!
        public static string ConvertToReadableFormat(double seconds)
        {
            if (seconds < 60)
                return "few secondes ago";

            var min = seconds / 60;

            if (min < 24)
                return $"{Math.Round(min)} min ago";

            var hours = min / 60;

            if (hours < 24)
                return $"{Math.Round(hours)} hours ago";

            var days = hours / 24;

            if (days < 365)
                return $"{Math.Round(days)} days ago";

            var years = days / 365;

            return $"{Math.Round(years)} years ago";
        }

        /// <summary>
        /// Generates random <see cref="DateTime"/> between 1970 and today
        /// </summary>
        /// <returns>Generated <see cref="DateTime"/></returns>
        public static DateTime RandomDate()
        {
            DateTime start = new DateTime(1970, 1, 1);
            int range = (DateTime.Today - start).Days;
            start = start.AddMinutes(mRnd.Next(0, 1440));
            return start.AddDays(mRnd.Next(range));
        }

        /// <summary>
        /// Generates random <see cref="DateTime"/> between given <paramref name="start"/> and today
        /// </summary>
        /// <param name="start">Data to start from</param>
        /// <returns>Generated <see cref="DateTime"/></returns>
        public static DateTime RandomDate(DateTime start)
        {
            int range = (DateTime.Today - start).Days;
            start = start.AddMinutes(mRnd.Next(0, 1440));
            return start.AddDays(mRnd.Next(range));
        }
    }
}
