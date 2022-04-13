namespace Igtampe.Controllers.Requests {

    /// <summary>Request to change a password</summary>
    public class ChangePasswordRequest {

        /// <summary>Current password</summary>
        public string? Current { get; set; }

        /// <summary>New Password</summary>
        public string? New { get; set; }
    }
}
