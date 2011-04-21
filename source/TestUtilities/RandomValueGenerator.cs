using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TestUtilities
{

    public static class RandomValueGenerator
    {

        private static readonly Random _randomGenerator = new Random(DateTime.Now.ToString().GetHashCode());
        private static Random Random
        {
            get
            {
                return (_randomGenerator);
            }
        }

        public static bool GetRandomBoolean()
        {
            return (GetRandomInt(-5, +5) > 0);
        }

        public static DateTime GetRandomDate()
        {
            DateTime minDate = DateTime.MinValue;
            DateTime maxDate = DateTime.MaxValue;
            return GetRandomDate(minDate, maxDate);
        }

        public static DateTime GetRandomDate(DateTime minDate, DateTime maxDate)
        {
            if (maxDate < minDate) maxDate = minDate;
            var timeSpan = maxDate - minDate;

            long range = timeSpan.Days;
            if (maxDate == DateTime.MaxValue && minDate < DateTime.MaxValue.AddDays(-1)) range -= 1;
            if (range <= 0) range = 1;
            int intRange;
            if (range > int.MaxValue)
            {
                intRange = int.MaxValue - 1;
            }
            else
            {
                intRange = (int)range;
            }
            int randomInt = GetRandomInt(1, intRange);
            return minDate.AddDays(randomInt);
        }

        public static decimal GetRandomDecimal()
        {
            return GetRandomInt();
        }
        /*
                public static decimal GetRandomDecimal(decimal minValue, decimal maxValue)
                {
                    if (((minValue < 0M) && (maxValue > 0M)) && ((decimal.MaxValue - maxValue) < (-1M*minValue)))
                    {
                        minValue = 0M;
                    }
                    decimal range = maxValue - minValue;
                    decimal truncatedRange = Math.Floor(range);
                    if (truncatedRange > int.MaxValue)
                    {
                        truncatedRange = int.MaxValue - 1;
                    }
                    if (truncatedRange <= 0M)
                    {
                        truncatedRange = 1M;
                    }
                    int randomAddition = GetRandomInt(1, Convert.ToInt32(truncatedRange));
                    return (minValue + randomAddition);
                }*/

        public static double GetRandomDouble()
        {
            return GetRandomInt();
        }

        public static double GetRandomDouble(double minValue, double maxValue)
        {
            if (((minValue < 0.0) && (maxValue > 0.0)) && ((double.MaxValue - maxValue) < (-1.0 * minValue)))
            {
                minValue = 0.0;
            }
            double range = maxValue - minValue;
            double truncatedRange = Math.Floor(range);
            if (truncatedRange > int.MaxValue)
            {
                truncatedRange = int.MaxValue - 1;
            }
            if (truncatedRange <= 0.0)
            {
                truncatedRange = 1.0;
            }
            int randomAddition = GetRandomInt(1, Convert.ToInt32(truncatedRange));
            return (minValue + randomAddition);
        }

        public static Guid GetRandomGuid()
        {
            return Guid.NewGuid();
        }

        public static int GetRandomInt()
        {
            return GetRandomInt(0x186a0);
        }

        public static int GetRandomInt(int max)
        {
            return Random.Next(-2147483648, max);
        }

        public static int GetRandomInt(int min, int max)
        {
            if (max < min) max = min;
            return Random.Next(min, max);
        }

        public static long GetRandomLong()
        {
            return GetRandomLong(long.MaxValue);
        }

        public static long GetRandomLong(long max)
        {
            return GetRandomLong(long.MinValue, max);
        }

        public static long GetRandomLong(long min, long max)
        {
            if (max < min) max = min;
            long range = max - min;
            if (Overflow(range)) range = int.MaxValue;
            int intRange;
            if (range > int.MaxValue)
            {
                intRange = int.MaxValue;
            }
            else
            {
                intRange = (int)range;
            }
            int randomInt = GetRandomInt(0, intRange);
            return min + randomInt;
        }

        /// <summary>
        /// An overflow has occured i.e. the range is greater than 
        ///  the size of an long.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        private static bool Overflow(long range)
        {
            return range < 0;
        }


        public static object GetRandomLookupListValue(Dictionary<string, string> lookupList)
        {
            if (lookupList.Count == 0)
            {
                return null;
            }
            string[] values = new string[lookupList.Count];
            lookupList.Values.CopyTo(values, 0);
            return ((values.Length == 1) ? values[0] : values[GetRandomInt(0, values.Length - 1)]);
        }

        public static string GetRandomString()
        {
            return ("A" + Guid.NewGuid().ToString().Replace("-", ""));
        }

        public static string GetRandomString(int maxLength)
        {
            string randomString = GetRandomString();
            if (maxLength > randomString.Length)
            {
                maxLength = randomString.Length;
            }
            return randomString.Substring(0, maxLength);
        }

        public static string GetRandomString(int minLength, int maxLength)
        {
            if (maxLength <= 0) maxLength = int.MaxValue;
            if (minLength < 0) minLength = 0;
            string randomString = GetRandomString(maxLength);
            if (randomString.Length < minLength)
            {
                randomString = randomString.PadRight(minLength, 'A');
            }
            return randomString;
        }



        /// <summary>
        /// Returns the absolute minimum for the dataTypes.
        /// This only supports types that have MinValue, MaxValue e.g. single, Double, Decimaal 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetAbsoluteMin<T>() // where T : struct, IComparable<T>
        {
            Type type = typeof(T);
            return (T)GetAbsoluteMin(type);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetAbsoluteMin(Type type)
        {
            if (type == typeof(int)) return int.MinValue;
            if (type == typeof(decimal)) return decimal.MinValue;
            if (type == typeof(double)) return double.MinValue;
            if (type == typeof(Single)) return Single.MinValue;
            if (type == typeof(long)) return long.MinValue;
            if (type == typeof(DateTime)) return DateTime.MinValue;

            return int.MinValue;
        }

        /// <summary>
        /// Returns the absolute minimum for the dataTypes.
        /// This only supports types that have MinValue, MaxValue e.g. single, Double, Decimaal 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetAbsoluteMax<T>() // where T : struct, IComparable<T>
        {
            Type type = typeof(T);
            return (T)GetAbsoluteMax(type);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetAbsoluteMax(Type type)
        {
            if (type == typeof(int)) return int.MaxValue;
            if (type == typeof(decimal)) return decimal.MaxValue;
            if (type == typeof(double)) return double.MaxValue;
            if (type == typeof(Single)) return Single.MaxValue;
            if (type == typeof(long)) return long.MaxValue;
            if (type == typeof(DateTime)) return DateTime.MaxValue;

            return int.MaxValue;
        }
    }
}
