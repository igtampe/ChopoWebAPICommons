using Microsoft.AspNetCore.Mvc;
using Igtampe.ChopoSessionManager;
using Microsoft.EntityFrameworkCore;
using Igtampe.DBContexts;
using Igtampe.ChopoImageHandling;
using Igtampe.ChopoAuth;

namespace Igtampe.Controllers {

    /// <summary>Controller that handles User operations</summary>
    [Route("API/Images")]
    [ApiController]
    public abstract class ImageController<E> : ErrorResultControllerBase where E : DbContext, ImageContext, UserContext {

        private readonly E DB;
        private readonly ISessionManager Manager;

        /// <summary>Creates a User Controller</summary>
        /// <param name="Context"></param>
        /// <param name="Manager">Optional session manager</param>
        public ImageController(E Context, ISessionManager? Manager = null) {
            DB = Context;
            this.Manager = Manager ?? SessionManager.Manager;
        }

        /// <summary>Gets an image from the DB</summary>
        /// <param name="ID">ID of the image to retrieve</param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetImage(Guid ID) {
            Image? I = await DB.Image.FindAsync(ID);
            return I is null || I.Data is null || I.Type is null 
                ? NotFoundItem("Image", ID)
                : File(I.Data, I.Type);
        }

        /// <summary>Gets an image from the DB</summary>
        /// <param name="ID">ID of the image to retrieve</param>
        /// <returns></returns>
        [HttpGet("{ID}/Info")]
        public async Task<IActionResult> GetImageInfo(Guid ID) {
            Image? I = await DB.Image.FindAsync(ID);
            return I is null || I.Data is null || I.Type is null
                ? NotFoundItem("Image", ID)
                : Ok(I);
        }

        /// <summary>Checks the roles of a given u</summary>
        /// <param name="U">User to check the roles of</param>
        /// <returns></returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        //This method is left async in case the user needs to find the roles elswhere.
        protected async virtual Task<bool> CheckUploadRoles(User U) => true;
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        /// <summary>Uploads an Image to the DB.</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <returns></returns>
        // POST api/Images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromHeader] Guid SessionID) {

            //Check the session:
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            //Find the user
            User? U = await DB.User.FirstOrDefaultAsync(O=> O.Username == S.Username);
            if(U is null) { return InvalidSession(); }

            if (!await CheckUploadRoles(U)) { return Unauthorized(ErrorResult.ForbiddenRoles()); }

            string? ContentType = Request.ContentType;
            int MaxSize = 1024 * 1024 * 1;

            if (ContentType != "image/png" && ContentType != "image/jpeg" && ContentType != "image/gif") { return BadRequest(ErrorResult.BadRequest("File must be PNG, JPG, or GIF")); }
            if (Request.ContentLength > MaxSize) { return BadRequest("File must be less than 1mb in size"); }

            Image I = new() { Type = ContentType };

            using (var memoryStream = new MemoryStream()) {
                await Request.Body.CopyToAsync(memoryStream);
                I.Data = memoryStream.ToArray();
                if (I.Data.Length > MaxSize) { return BadRequest("File must be less than 1mb in size"); }
            }

            DB.Image.Add(I);
            await DB.SaveChangesAsync();

            return Ok(I);

        }
    }
}
