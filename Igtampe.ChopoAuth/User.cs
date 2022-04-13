using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Igtampe.ChopoAuth {
    /// <summary>Holds the basics of a ChopoAuth User.</summary>
    public class User : Depictable {

        private static readonly Hashbrown.Hashbrown hashbrown = new("AUTH_SALT");

        /// <summary>Username of this user</summary>
        [Key]
        public string Username { get; set; } = "";

        /// <summary>Hashed password of this user. Remember! If you're trying to update the password, use <see cref="UpdatePass(string)"/></summary>
        [JsonIgnore]
        public string Password { get; set; } = "";

        /// <summary>Image URL to this User's profile picture</summary>
        public string ImageURL { get; set; } = "";

        /// <summary>Whether or not this user is an administrator</summary>
        public bool IsAdmin { get; set; } = false;

        //Additional roles can be set by derivatives of this de-esta cosa

        /// <summary>Updates the password of this user</summary>
        /// <param name="NewPass"></param>
        public void UpdatePass(string NewPass) => Password = hashbrown.Hash(NewPass);

        /// <summary>Checks if the given password is a match to this user's password</summary>
        /// <param name="Attempt"></param>
        /// <returns></returns>
        public bool CheckPass(string Attempt) => hashbrown.Hash(Attempt) == Password;

        /// <summary>Checks if this User is equal to another user</summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj) => obj is User U && Username == U.Username;

        /// <summary>Gets a hashcode for this User. Delegates to <see cref="Username"/></summary>
        /// <returns></returns>
        public override int GetHashCode() => Username.GetHashCode();

        /// <summary>Returns a string representation of this user</summary>
        /// <returns></returns>
        public override string ToString() => $"User {Username}";

    }
}