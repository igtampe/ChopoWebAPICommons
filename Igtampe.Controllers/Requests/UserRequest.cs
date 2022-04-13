namespace Igtampe.Controllers.Requests {

    /// <summary>Request to log in or register</summary>
    public class UserRequest {

        /// <summary>Username of the user</summary>
        public string Username { get; set; } = "";

        /// <summary>Password of the user</summary>
        public string Password { get; set; } = "";

    }
}
