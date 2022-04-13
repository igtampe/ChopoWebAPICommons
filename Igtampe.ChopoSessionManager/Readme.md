# Chopo Session Manager
The Chopo Session Manager is a super simple active session manager.

## Session
An Individual session of the CSM

|Property|Description|
|-|-|
|ID|GUID of this session
|ExpirationDate|Date and Time at which this session will expire|
|Username|Username this session is tied to|
|Expired|Property shortcut to if the current time is beyond the ExpirationDate|

Sessions can be extended with `ExtendSession()` with a specified timespan, or a default configured timespan `ExtendTime`

## SessionManager
The actual session manager. This one is a singleton, accessed by static property `Manager`. This isn't required, and there's an interface `ISessionManager` for flexibility.

|Property|Description|
|-|-|
|Count|Number of sessions tracked (including expired ones!)

|Method|Description|
|-|-|
|Login()|Generates a session for a user with specified username, and adds it to the collection of tracked sessions. Returns the ID of the session.|
|FindSession()|Finds a session with given ID, extends it, then returns it. If it's not found, it returns null. If it's found but is expired, it is removed from the list of sessions, and returns null.|
|ExtendSession()|Extends a session with given ID|
|LogOut()|Logs out session with given ID. Returns true if successful|
|LogOutAll()|Logs out all sessions with given username|
|RemoveExpiredSessions()|Removes all sessions from the collection of active sessions that are expired|
