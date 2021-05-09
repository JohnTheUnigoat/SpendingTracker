using System;

namespace Core.Const
{
    public static class ReportPeriods
    {
        public const string CurrentDay = "curr_day";
        public const string CurrentWeek = "curr_week";
        public const string CurrentMonth = "curr_month";
        public const string CurrentYear = "curr_year";
        public const string AllTime = "all_time";

        public static string GetText(string reportPeriod)
        {
            switch (reportPeriod)
            {
                case CurrentDay: return "Current day";
                case CurrentWeek: return "Current week";
                case CurrentMonth: return "Current month";
                case CurrentYear: return "Current year";
                case AllTime: return "All time";
                default: throw new ArgumentException("reportPeriod should be one of the ReportPeriods constants.");
            }
        }
    }
}


