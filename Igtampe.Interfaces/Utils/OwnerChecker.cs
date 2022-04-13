namespace Igtampe.Utils {

    /// <summary>Checks ownership of <see cref="Ownable{E}"/> objects</summary>
    public static class OwnerChecker {
    
        /// <summary>Checks if given Owner is the actual owner of the Ownable object</summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="Ownable"></param>
        /// <param name="Owner"></param>
        /// <returns></returns>
        public static bool IsOwner<E>(Ownable<E> Ownable, E Owner) 
            => Ownable.Owner is not null && Ownable.Owner.Equals(Owner);

        /// <summary>Checks if given Owner is in the list of actual owners of the Ownable object</summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="Ownable"></param>
        /// <param name="Owner"></param>
        /// <returns></returns>
        public static bool IsOwner<E>(MultiOwnable<E> Ownable, E Owner) 
            => Ownable.Owners.Contains(Owner);

        /// <summary>Checks if given Owner is the actual primary owner or in the list of other owners of the Ownable object</summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="Ownable"></param>
        /// <param name="Owner"></param>
        /// <returns></returns>
        public static bool IsOwner<E>(PrimaryOwnable<E> Ownable, E Owner) 
            => IsOwner(Ownable as Ownable<E>, Owner) || IsOwner(Ownable as MultiOwnable<E>, Owner);

    }
}
