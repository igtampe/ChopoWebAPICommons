namespace Igtampe {
    /// <summary>Interface for any object that can be owned by more than one user</summary>
    public interface MultiOwnable<E> {
        /// <summary>Owner of this object</summary>
        public List<E> Owners { get; set; }
    }
}
