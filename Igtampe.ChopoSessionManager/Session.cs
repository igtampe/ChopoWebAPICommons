using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Igtampe.ChopoSessionManager {

    /// <summary>Holds information on a user session. <br/>
    /// Although The CSM stores things in memory in a hash set, this class is ready to be used to store sessions in a DB 
    /// with EF Core if you're into that sort of thing</summary>
    public class Session {

        /// <summary>Timespan a session will be extended by. By default, this is 12 hours</summary>
        public static TimeSpan ExtendTime { get; set; } = TimeSpan.FromHours(12);

        /// <summary>ID of this session</summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; }

        /// <summary>Time at which this session will no longer be valid</summary>
        public DateTime ExpirationDate { get; private set; } = DateTime.MinValue;

        /// <summary>Alias for <see cref="Username"/></summary>
        [Obsolete("Use 'Username' instead!")]
        [NotMapped]
        [JsonIgnore]
        public string UserID => Username;

        /// <summary>User tied to this Session</summary>
        public string Username { get; }

        /// <summary>Whether or not this session is expired.</summary>
        [NotMapped]
        public bool Expired => DateTime.UtcNow > ExpirationDate;

        /// <summary>Creates a session for a user with given username</summary>
        /// <param name="Username"></param>
        public Session(string Username) {
            this.Username= Username;
            ID = Guid.NewGuid();
            ExtendSession();
        }

        /// <summary>Extends a session by either default configured timespan (<see cref="ExtendTime"/>) or by a specified one</summary>
        /// <param name="Span">Optional specified timespan to extend this session by</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void ExtendSession(TimeSpan? Span = null) {
            if (ExpirationDate != DateTime.MinValue && Expired) { throw new InvalidOperationException("Session is already expired"); }
            ExpirationDate = DateTime.UtcNow.Add(Span ?? ExtendTime);
        }

        /// <summary>String representation of this session</summary>
        /// <returns></returns>
        public override string ToString() => $"Session {ID} for {Username} (Expires {ExpirationDate})";

        /// <summary>Compares this Session to another object</summary>
        /// <param name="obj"></param>
        /// <returns>True if and only if object is a session and the ID matches</returns>
        public override bool Equals(object? obj) => obj is Session session && ID.Equals(session.ID);

        /// <summary>Gets hashcode for this session</summary>
        /// <returns><see cref="ID"/>'s hashcode</returns>
        public override int GetHashCode() => HashCode.Combine(ID);
    }
}
