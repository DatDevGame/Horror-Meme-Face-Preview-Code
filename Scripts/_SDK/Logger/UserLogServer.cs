#define FIREBASE_LOG
using System;
using System.Collections.Generic;

namespace Assets._SDK.Logger
{
    public static class UserLogServer
    {
        [Flags] 
        private enum ServerType
        {
            Debug = 1,
            Firebase = 1 << 1
        };

        public static IUserLog LogServer => _logServer.Value;
        
        private static readonly Lazy<IUserLog> _logServer =
            new Lazy<IUserLog>(ServerFactory);
#if FIREBASE_LOG
        private static ServerType _serverType = ServerType.Firebase;
#else
        private static ServerType _serverType = ServerType.Debug;
#endif

        private static readonly List<IUserLog> _serverList = new List<IUserLog>();

        private static IUserLog ServerFactory()
        {
            if ((_serverType & ServerType.Debug) != 0)
                _serverList.Add(new MockLogServer());
            if ((_serverType & ServerType.Firebase) != 0)
                _serverList.Add(new FirebaseLogServer());
            return new MultiLogServer(_serverList);
        }
    }
}