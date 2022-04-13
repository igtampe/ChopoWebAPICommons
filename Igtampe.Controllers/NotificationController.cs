using Microsoft.AspNetCore.Mvc;
using Igtampe.ChopoSessionManager;
using Microsoft.EntityFrameworkCore;
using Igtampe.DBContexts;
using Igtampe.ChopoAuth;

namespace Igtampe.Controllers {

    /// <summary>Controller that handles User operations</summary>
    [Route("API/Notif")]
    [ApiController]
    public abstract class NotificationController<E> : ErrorResultControllerBase where E : DbContext, UserContext, NotificationContext {

        private readonly E DB;
        private readonly ISessionManager Manager;

        /// <summary>Creates a User Controller</summary>
        /// <param name="Context"></param>
        /// <param name="Manager">Optional session manager</param>
        public NotificationController(E Context, ISessionManager? Manager = null) {
            DB = Context;
            this.Manager = Manager ?? SessionManager.Manager;
        }

        /// <summary>Gets all notifications from the logged in user</summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromHeader] Guid? SessionID) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID ?? Guid.Empty));
            if (S is null) { return InvalidSession(); }

            var Data = await DB.Notification.Where(A => A.Owner != null && A.Owner.Username == S.Username).ToListAsync();
            return Ok(Data);

        }

        /// <summary>Deletes one notification from the logged in user</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteOne([FromHeader] Guid? SessionID, [FromRoute] Guid ID) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID ?? Guid.Empty));
            if (S is null) { return InvalidSession(); }

            DB.Notification.RemoveRange(DB.Notification.Where(A => A.Owner != null && A.Owner.Username == S.Username && A.ID == ID));
            await DB.SaveChangesAsync();
            return Ok();

        }

        /// <summary>Deletes all notifications from the logged in user</summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAll([FromHeader] Guid? SessionID) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID ?? Guid.Empty));
            if (S is null) { return InvalidSession(); }

            DB.Notification.RemoveRange(DB.Notification.Where(A => A.Owner != null && A.Owner.Username == S.Username));
            await DB.SaveChangesAsync();
            return Ok();

        }
    }
}
