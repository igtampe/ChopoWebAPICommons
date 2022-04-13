using Igtampe.IDGenerators;

namespace Igtampe {

    /// <summary>Abstract class for any object which has an ID that needs to be *generated* manually</summary>
    /// <typeparam name="E"></typeparam>
    public abstract class ManuallyGeneratableIdentifiable<E> : Identifiable<E> {

        /// <summary>Generator for this object's ID</summary>
        /// <returns>A potential ID for this object</returns>
        public abstract IDGenerator<E> IDGenerator { get; }
    }
}
