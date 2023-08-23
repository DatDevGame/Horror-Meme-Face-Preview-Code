using System.Collections.Generic;
using Assets._SDK.Game;
using Firebase.Analytics;
using Assets._SDK.Analytics;

namespace Assets._SDK.Logger
{

    public class FirebaseLogServer : IUserLog
    {
        private bool IsConnected => FirebaseService.Instance.IsConnected.Value;

        public void LogEvent(string name)
        {
            if (IsConnected)
            {
                FirebaseAnalytics.LogEvent(name);
            }
        }

        public void LogEvent(string name, params LogParameter[] parameters)
        {
            var firebaseParameters = new Parameter[parameters.Length];
            for (int i = 0; i < firebaseParameters.Length; i++)
            {
                firebaseParameters[i] = parameters[i].GetFirebaseParameter();
            }

            if (IsConnected)
            {
                FirebaseAnalytics.LogEvent(name, firebaseParameters);
            }
            
        }

        public void LogEvent(string name, string parameterName, int parameterValue)
        {
            if (IsConnected)
            {
                FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
            }
            
        }

        public void LogEvent(string name, string parameterName, string parameterValue)
        {
            if (IsConnected)
            {
                FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
            }
            
        }

        public void LogEvent(string name, string param1, string value1, string param2, string value2)
        {
            if (IsConnected)
            {
                FirebaseAnalytics.LogEvent(name, new Parameter(param1, value1), new Parameter(param2, value2));
            }
            
        }

        public void LogScene(string sceneName)
        {
            // NOTE: Dont need yet
        }
    }
}