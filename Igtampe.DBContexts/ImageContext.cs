using Igtampe.ChopoImageHandling;
using Microsoft.EntityFrameworkCore;

namespace Igtampe.DBContexts {

    /// <summary>A context for objects that are, or are based on <see cref="ChopoImageHandling.Image"/></summary>
    public interface ImageContext {

        /// <summary>Table of all images</summary>
        public DbSet<Image> Image {get; set;}
    }
}
