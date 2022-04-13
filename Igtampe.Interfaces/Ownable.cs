namespace Igtampe {
    /// <summary>Interface for any object that can be owned by one user</summary>
    public interface Ownable<E> {
        /// <summary>Owner of this object</summary>
        public E? Owner { get; set; }
    }
}
