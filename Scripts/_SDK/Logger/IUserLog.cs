using System;

namespace Assets._SDK.Logger
{
    public interface IUserLog
    {
        void LogEvent(string key);
        void LogEvent(string name, params LogParameter[] parameters);
        void LogEvent(string name, string parameterName, int parameterValue);
        void LogEvent(string name, string parameterName, string parameterValue);
        void LogEvent(string name, string param1, string value1, string param2, string value2);
        void LogScene(string sceneName);
    }
}