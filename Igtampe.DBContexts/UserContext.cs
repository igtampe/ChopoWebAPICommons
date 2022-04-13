using Igtampe.ChopoAuth;
using Microsoft.EntityFrameworkCore;

namespace Igtampe.DBContexts {

    /// <summary>Context for objects that are, or are derived from <see cref="ChopoAuth.User"/></summary>
    public interface UserContext {

        /// <summary>Table of all users</summary>
        public DbSet<User> User {get; set;}
    }
}
