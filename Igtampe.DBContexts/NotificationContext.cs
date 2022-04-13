using Igtampe.ChopoImageHandling;
using Igtampe.Notifier;
using Microsoft.EntityFrameworkCore;

namespace Igtampe.DBContexts {

    /// <summary>A context for objects that are, or are derivative forms of <see cref="Notification"/></summary>
    public interface NotificationContext : UserContext {

        /// <summary>Table of all users</summary>
        public DbSet<Notification> Notification {get; set;}
    }
}
