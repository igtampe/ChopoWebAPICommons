namespace Igtampe {

    /// <summary>Abstract class for any object that has an ID</summary>
    public abstract class Identifiable<E> {

        /// <summary>ID of this object</summary>
        public virtual E? ID { get; set; }

        /// <summary>Verifies if this object is equal to the given object</summary>
        /// <param name="obj"></param>
        /// <returns> True if the object is Identifiable and its ID matches this one's</returns>
        public override bool Equals(object? obj)
            => obj is not null &&
            obj.GetType() == GetType() &&
            obj is Identifiable<E> I &&
            ID as dynamic == I.ID as dynamic;

        /// <summary>Generates a hashcode for this identifiable</summary>
        /// <returns>The hashcode of the ID</returns>
        public override int GetHashCode() => ID is null ? base.GetHashCode() : ID.GetHashCode();

        /// <summary>Turns this identifiable into a string</summary>
        /// <returns>The ID of the identifiable tostring</returns>
        public override string ToString() => ID?.ToString() ?? "Unknown Identifiable";
    }
}
