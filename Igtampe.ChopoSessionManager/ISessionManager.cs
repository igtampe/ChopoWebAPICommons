namespace Igtampe.ChopoSessionManager {
    /// <summary>Interface for an implementation of any other type of session manager</summary>
    public interface ISessionManager {

        /// <summary>Amount of sessions in the collection (including those that are expired)</summary>
        public int Count { get; }

        /// <summary>Logs specified user in to a new session.</summary>
        /// <param name="UserID">ID of the user to sign in</param>
        /// <returns>GUID of the added session</returns>
        public Guid LogIn(string UserID);

        /// <summary>Returns a session with sepcified ID. <br/>
        /// If the Session is expired, it returns null, and removes the session from the collection.<br/>
        /// Otherwise, it extends the session before returning it.</summary>
        /// <param name="ID">ID of the session to find</param>
        /// <returns>Returns a session if one exists, if not NULL</returns>
        public Session? FindSession(Guid? ID);

        /// <summary>Extends a session with given UID</summary>
        /// <returns>True if a session was found and it was able to be extended. False otherwise</returns>
        public bool ExtendSession(Guid ID);

        /// <summary>Removes a session with specified ID</summary>
        /// <param name="ID"></param>
        /// <returns>Returns true if the session was found and was removed, false otherwise</returns>
        public bool LogOut(Guid ID);

        /// <summary>Removes all sessions for the specified user</summary>
        /// <param name="Username"></param>
        /// <returns>Number of sessions logged out of</returns>
        public int LogOutAll(string Username);

        /// <summary>Removes all expired sessions from the collection of active sessions</summary>
        /// <returns>Amount of removed sessions</returns>
        public int RemoveExpiredSessions();

    }
}
