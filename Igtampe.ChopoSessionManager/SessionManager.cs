namespace Igtampe.ChopoSessionManager {

    /// <summary>In memory singleton session manager. </summary>
    public class SessionManager : ISessionManager {

        /// <summary>Internal Singleton Session Manager object</summary>
        private static SessionManager? SingletonSM;

        /// <summary>Gets the static, singleton session manager</summary>
        public static SessionManager Manager => SingletonSM is null ? SingletonSM = new SessionManager() : SingletonSM;

        /// <summary>Collection of all sessions in this manager</summary>
        private readonly ICollection<Session> Sessions;

        /// <summary>Amount of sessions in the collection (including those that are expired)</summary>
        public int Count => Sessions.Count;

        /// <summary>Internal constructor to create a session manager</summary>
        private SessionManager() => Sessions = new HashSet<Session>();

        /// <summary>Logs specified user in to a new session.</summary>
        /// <param name="UserID">ID of the user to sign in</param>
        /// <returns>GUID of the added session</returns>
        public Guid LogIn(string UserID) {
            Session S;
            do { S = new(UserID); } 
            while (Sessions.Contains(S)); //Ideally this will only run once

            //Add the session
            Sessions.Add(S);

            //Return the Session
            return S.ID;
        }

        /// <summary>Returns a session with sepcified ID. <br/>
        /// If the Session is expired, it returns null, and removes the session from the collection.<br/>
        /// Otherwise, it extends the session before returning it.</summary>
        /// <param name="ID">ID of the session to find</param>
        /// <returns>Returns a session if one exists, if not NULL</returns>
        public Session? FindSession(Guid? ID) {
            if (ID == null) { return null; }
            Session? S = Sessions.FirstOrDefault(S => S.ID == ID);
            if (S is null) { return null; }
            if (S.Expired) { Sessions.Remove(S); return null; }
            S.ExtendSession();
            return S;
        }

        /// <summary>Extends a session with given UID</summary>
        /// <returns>True if a session was found and it was able to be extended. False otherwise</returns>
        public bool ExtendSession(Guid ID) {
            Session? S = FindSession(ID);
            if (S == null) { return false; }
            S.ExtendSession();
            return true;
        }

        /// <summary>Removes a session with specified ID</summary>
        /// <param name="ID"></param>
        /// <returns>Returns true if the session was found and was removed, false otherwise</returns>
        public bool LogOut(Guid ID) {
            Session? S = FindSession(ID);
            return S is not null && Sessions.Remove(S);
        }

        /// <summary>Removes all sessions for the specified user</summary>
        /// <param name="Username"></param>
        /// <returns>Number of sessions logged out of</returns>
        public int LogOutAll(string Username) {
            ICollection<Session> Ss = Sessions.Where(S => S.Username == Username).ToList();
            foreach (Session S in Ss) { Sessions.Remove(S);}
            return Ss.Count;
        }

        /// <summary>Removes all expired sessions from the collection of active sessions</summary>
        /// <returns>Amount of removed sessions</returns>
        public int RemoveExpiredSessions() {
            ICollection<Session> ExpiredSessions = Sessions.Where(S => S.Expired).ToHashSet();
            int RemovedCount = 0;
            foreach (Session S in ExpiredSessions) { if (Sessions.Remove(S)) RemovedCount++; }
            return RemovedCount;
        }

        /// <summary>Routine to periodically remove all expired sessions from the collection</summary>
        public static void SessionRemoverThread(int Seconds) {
            while (true) {
                Thread.Sleep(Seconds * 1000);
                if (Manager.Sessions.Count == 0) { continue; }
                Console.WriteLine($"Removed {Manager.RemoveExpiredSessions()} expired session(s)");
            }
        }
    }
}
