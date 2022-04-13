using System.ComponentModel.DataAnnotations.Schema;

namespace Igtampe {

    /// <summary>Abstract class for any object that has a GUID ID</summary>
    public abstract class AutomaticallyGeneratableIdentifiable : Identifiable<Guid> {

        /// <summary>ID of this object</summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid ID { get; set; }
    }
}
