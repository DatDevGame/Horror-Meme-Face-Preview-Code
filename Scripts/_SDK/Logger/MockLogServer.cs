using System.Text;
using UnityEngine;

namespace Assets._SDK.Logger
{
    public class MockLogServer : IUserLog
    {
        public void LogEvent(string key)
        {
            LogMessage(key);
        }

        public void LogEvent(string key, LogParameter[] parameters)
        {
            var _builder = new StringBuilder();
            _builder.Append(key);
            _builder.AppendLine();
            foreach (var param in parameters)
            {
                _builder.Append(param);
                _builder.AppendLine();
            }
            
            LogMessage(_builder.ToString());
        }

        public void LogEvent(string name, string parameterName, int parameterValue)
        {
            LogMessage($"{name} param: {parameterName} {parameterValue}");
        }

        public void LogEvent(string name, string parameterName, string parameterValue)
        {
            LogMessage($"{name} param: {parameterName} {parameterValue}");
        }

        public void LogEvent(string name, string param1, string value1, string param2, string value2)
        {
            LogMessage($"{name} param: ({param1}-{value1}) , ({param2}-{value2})");
        }

        public void LogSelectContent(string content, string paramName, string paramValue)
        {
            LogMessage($"Select content {content} - {paramName} - {paramValue}");
        }

        public void LogSelectContent(string content)
        {
            LogMessage($"Select content {content}");
        }

        public void LogScene(string sceneName)
        {
            LogMessage($"Scene {sceneName}");
        }

        private void LogMessage(string message)
        {
            Debug.Log($"<color=#00FFA3>Mock log: {message}</color>");
        }
    }
}