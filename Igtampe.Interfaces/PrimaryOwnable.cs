namespace Igtampe {
    /// <summary>Interface for any object that can be owned by more than one user, but has one primary owner</summary>
    public interface PrimaryOwnable<E> : MultiOwnable<E>, Ownable<E> {
        /// <summary>Owner of this object</summary>
        public E? PrimaryOwner => Owner;
    }
}
