using Microsoft.AspNetCore.Mvc;
using Igtampe.ChopoSessionManager;
using Microsoft.EntityFrameworkCore;
using Igtampe.DBContexts;
using Igtampe.ChopoAuth;
using Igtampe.Controllers.Requests;

namespace Igtampe.Controllers {

    /// <summary>Controller that handles User operations</summary>
    [Route("API/Users")]
    [ApiController]
    public class UserController<E> : ErrorResultControllerBase where E : DbContext, UserContext{

        private readonly E DB;
        private readonly ISessionManager Manager;

        /// <summary>Creates a User Controller</summary>
        /// <param name="Context"></param>
        /// <param name="Manager">Session Manager used by this controller. If null, it'll use the default singleton one from <see cref="SessionManager.Manager"/> </param>
        public UserController(E Context, ISessionManager? Manager = null) {
            DB = Context; 
            this.Manager = Manager ?? SessionManager.Manager;
        }

        #region Gets
        /// <summary>Gets a directory of all users</summary>
        /// <param name="Query">Search query to search in IDs and </param>
        /// <param name="Take"></param>
        /// <param name="Skip"></param>
        /// <returns></returns>
        [HttpGet("Dir")]
        public async Task<IActionResult> Directory([FromQuery] string? Query, [FromQuery] int? Take, [FromQuery] int? Skip) {
            IQueryable<User> Set = DB.User;
            if (!string.IsNullOrWhiteSpace(Query)) { Set = Set.Where(U => U.Username != null && U.Username.Contains(Query)); }
            Set = Set.Skip(Skip ?? 0).Take(Take ?? 20);

            return Ok(await Set.ToListAsync());
        }

        /// <summary>Gets username of the currently logged in session</summary>
        /// <param name="SessionID">ID of the session</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCurrentLoggedIn([FromHeader] Guid? SessionID) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID ?? Guid.Empty));
            if (S is null) { return InvalidSession(); }

            //Get the user
            return await GetUser(S.Username);
        }

        /// <summary>Gets a given user</summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetUser(string ID) {
            //Get the user
            User? U = await DB.User.FirstOrDefaultAsync(U => U.Username == ID);
            return U is null ? NotFound("User was not found") : Ok(U);
        }

        #endregion

        #region Puts

        /// <summary>Handles user password changes</summary>
        /// <param name="Request">Request with their current and new password</param>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <returns></returns>
        // PUT api/Users
        [HttpPut]
        public async Task<IActionResult> Update([FromHeader] Guid? SessionID, [FromBody] ChangePasswordRequest Request) {

            //Ensure nothing is null
            if (Request.New is null || Request.Current is null) { return BadRequest("Cannot have empty passwords"); }

            //Check the session:
            Session? S = await Task.Run(() => Manager.FindSession(SessionID ?? Guid.Empty));
            if (S is null) { return InvalidSession(); }

            //Check the password
            User? U = await DB.User.FirstOrDefaultAsync(U => U.Username == S.Username);
            if (U is null || !U.CheckPass(Request.Current)) { return BadRequest("Incorrect current password"); }

            U.UpdatePass(Request.New);
            DB.Update(U);
            await DB.SaveChangesAsync();

            return Ok(U);

        }

        /// <summary>Request to reset the password of a user</summary>
        /// <param name="SessionID">SessionID of an administrator who wishes to change the password of another user</param>
        /// <param name="ID">ID of the user to change the password of</param>
        /// <param name="Request">Request to change</param>
        /// <returns></returns>
        [HttpPut("{ID}/Reset")]
        public async Task<IActionResult> ResetPassword([FromHeader] Guid? SessionID, [FromRoute] string ID, [FromBody] ChangePasswordRequest Request) {
            //Ensure nothing is null
            if (Request.New is null) { return BadRequest("Cannot have empty password"); }

            //Check the session:
            Session? S = await Task.Run(() => Manager.FindSession(SessionID ?? Guid.Empty));
            if (S is null) { return InvalidSession(); }

            //Get Users
            User? Executor = await DB.User.FirstOrDefaultAsync(U => U.Username == S.Username);
            if (Executor is null) { return InvalidSession(); }
            if (!Executor.IsAdmin) { return ForbiddenRoles("Admin"); }

            User? U = await DB.User.FirstOrDefaultAsync(U => U.Username == ID);
            if (U is null) { return NotFoundItem("User",ID); }

            U.UpdatePass(Request.New);
            DB.Update(U);
            await DB.SaveChangesAsync();

            return Ok(U);

        }

        /// <summary>Updates the image of the user with this session</summary>
        /// <param name="SessionID"></param>
        /// <param name="ImageURL"></param>
        /// <returns></returns>
        [HttpPut("image")]
        public async Task<IActionResult> UpdateImage([FromHeader] Guid? SessionID, [FromBody] string ImageURL) {
            //Check the session:
            Session? S = await Task.Run(() => Manager.FindSession(SessionID ?? Guid.Empty));
            if (S is null) { return InvalidSession(); }

            //Check the password
            User? U = await DB.User.FirstOrDefaultAsync(U => U.Username == S.Username);
            if (U is null) { throw new InvalidOperationException("Somehow we're here and we're not supposed to be here"); }

            U.ImageURL = ImageURL;
            DB.Update(U);
            await DB.SaveChangesAsync();

            return Ok(U);
        }

        #endregion

        #region Posts

        // POST api/Users
        /// <summary>Handles logging in to Clothespin</summary>
        /// <param name="Request">Request with a User and Password attempt to log in</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LogIn(UserRequest Request) {
            if (Request.Username is null || Request.Password is null) { return BadRequest("User or Password was empty"); }

            //Check the user on the DB instead of the user de-esta cosa
            var Login = await DB.User.FirstOrDefaultAsync(U => U.Username == Request.Username);
            if (Login is null || !Login.CheckPass(Request.Password)) { return BadRequest("User or Password was incorrect"); }

            //Generate a session
            return Ok(new { SessionID = Manager.LogIn(Request.Username) });

        }

        /// <summary>Handles user registration</summary>
        /// <param name="Request">User and password combination to create</param>
        /// <returns></returns>
        // POST api/Users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRequest Request) {
            if (Request.Username is null || Request.Password is null) { return BadRequest("User or Password was empty"); }

            User NewUser = new() { Username = Request.Username };
            NewUser.UpdatePass(NewUser.Password);

            if (!await DB.User.AnyAsync()) {
                //This is the first account and *MUST* be an admin
                NewUser.IsAdmin = true;
            } else if (DB.User.Any(U => U.Username == Request.Username)) { 
                //This check doesn't need to run if there isn't any users so we can put it as an else if
                return BadRequest("Username already in use"); 
            }

            DB.User.Add(NewUser);
            await DB.SaveChangesAsync();

            return Ok(NewUser);

        }

        /// <summary>Handles user logout</summary>
        /// <param name="SessionID">Session to log out of</param>
        /// <returns></returns>
        // POST api/Users/out
        [HttpPost("out")]
        public async Task<IActionResult> LogOut([FromHeader] Guid SessionID) 
            => Ok(await Task.Run(() => Manager.LogOut(SessionID)));

        /// <summary>Handles user logout of *all* sessions</summary>
        /// <param name="SessionID">Session that wants to log out of all tied sessions</param>
        /// <returns></returns>
        // POST api/Users/outall
        [HttpPost("outall")]
        public async Task<IActionResult> LogOutAll([FromHeader] Guid SessionID) {
            //Check the session:
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            return S is null 
                ? InvalidSession() 
                : Ok(await Task.Run(() => Manager.LogOutAll(S.Username)));
        }

        #endregion

    }
}
