using Firebase.Analytics;

namespace Assets._SDK.Logger
{
    public class LogParameter
    {
        public enum LogType
        {
            Long,
            String,
            Double
        }

        public string  ParameterName { get; }
        public string  StringValue   { get; }
        public long    LongValue     { get; }
        public double  DoubleValue   { get; }
        public LogType Type          { get; }

        public LogParameter(string parameterName, string parameterValue)
        {
            ParameterName = parameterName;
            StringValue = parameterValue;
            Type = LogType.String;
        }

        public LogParameter(string parameterName, long parameterValue)
        {
            ParameterName = parameterName;
            LongValue = parameterValue;
            Type = LogType.Long;
        }

        public LogParameter(string parameterName, double parameterValue)
        {
            ParameterName = parameterName;
            DoubleValue = parameterValue;
            Type = LogType.Double;
        }

        public Parameter GetFirebaseParameter()
        {
            switch (Type)
            {
                case LogType.Long:
                    return new Parameter(ParameterName, LongValue);
                case LogType.String:
                    return new Parameter(ParameterName, StringValue);
                case LogType.Double:
                    return new Parameter(ParameterName, DoubleValue);
            }

            return null;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case LogType.Long:
                    return $"{ParameterName} {Type} {LongValue}";
                case LogType.String:
                    return $"{ParameterName} {Type} {StringValue}";
                case LogType.Double:
                    return $"{ParameterName} {Type} {DoubleValue}";
                default:
                    return $"{ParameterName} {Type}";
            }
            
        }
    }
}