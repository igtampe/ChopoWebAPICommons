namespace Igtampe.IDGenerators {

    /// <summary>Abstract class for any ID Generator</summary>
    public abstract class IDGenerator<E> {

        /// <summary>Generates an ID</summary>
        /// <returns>An ID</returns>
        public abstract E Generate();

    }
}
