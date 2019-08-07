using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurLibrary.Util.Common
{
    public class DateUtil
    {
        private static int[] construcDays(int Year)
        {
           int[] Days = new int[12];
            for (int i = 1; i <= 12; i++)
            {
                if (i <= 7)
                {
                    if (i == 2)
                    {
                        Days[i - 1] = 28 + (Year % 4 == 0 ? 1 : 0);
                        continue;
                    }
                    Days[i - 1] = 30 + (i % 2 == 0 ? 0 : 1);

                }
                else
                {
                    Days[i - 1] = 30 + (i % 2 == 0 ? 1 : 0);
                }
            }
            return Days;
        }

        public static DateTime Now()
        {
           
            return DateTime.Now;
        }

        public static DateTime PlusDay(DateTime date, int day)
        {
          
            int[] Days = new int[12];
            int DD = date.Day;
            int MM = date.Month;
            int YY = date.Year;
            int Hour = date.Hour;
            int Min = date.Minute;
            int Sec = date.Second;
            int Ms = date.Millisecond;
            
            Days = construcDays(YY);
           
            for (int i = 1; i <= day; i++)
            {
                int MaxDay = Days[MM - 1];
                if (DD + 1 > MaxDay)
                {
                    DD = 1;
                    if (MM + 1 > 12)
                    {
                        MM = 1;
                        YY++;
                        Days= construcDays(YY);
                    }
                    else
                    {
                        MM++;
                    }
                }
                else
                {
                    DD++;
                }
            }
            DateTime result = DateTime.Parse(DD + "/" + MM + "/" + YY);
            return result;
        }


    }
}