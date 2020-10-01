using System;

namespace CommonLibrary
{
    public class ConfigurationFileManager
    {
        public static T ReadValue<T>(String ParameterName, T ReplacementValue)
        {
            try
            {
                T res = (T)Convert.ChangeType(Environment.GetEnvironmentVariable(ParameterName), typeof(T));
                return res == null ? ReplacementValue : res;
            }
            catch
            {
                return ReplacementValue;
            }
        }
    }
}
