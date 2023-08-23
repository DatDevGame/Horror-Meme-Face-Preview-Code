using System.Collections.Generic;

namespace Assets._SDK.Logger
{
    public class MultiLogServer : IUserLog
    {
        private readonly List<IUserLog> _serverList;

        public MultiLogServer(List<IUserLog> serverList)
        {
            _serverList = serverList;
        }

        public void LogEvent(string key)
        {
            foreach (var userLog in _serverList)
            {
                userLog.LogEvent(key);
            }
        }

        public void LogEvent(string key, LogParameter[] parameterName)
        {
            foreach (var userLog in _serverList)
            {
                userLog.LogEvent(key, parameterName);
            }
        }

        public void LogEvent(string name, string parameterName, int parameterValue)
        {
            foreach (var userLog in _serverList)
            {
                userLog.LogEvent(name, parameterName, parameterValue);
            }
        }

        public void LogEvent(string name, string parameterName, string parameterValue)
        {
            foreach (var userLog in _serverList)
            {
                userLog.LogEvent(name, parameterName, parameterValue);
            }
        }

        public void LogEvent(string name, string param1, string value1, string param2, string value2)
        {
            foreach (var userLog in _serverList)
            {
                userLog.LogEvent(name, param1, value1, param2, value2);
            }
        }

        public void LogScene(string sceneName)
        {
            foreach (var userLog in _serverList)
            {
                userLog.LogScene(sceneName);
            }
        }
    }
}