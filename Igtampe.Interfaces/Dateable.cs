namespace Igtampe {
    
    /// <summary>Any item that has a date of creation, and a date it was last updated</summary>
    public interface Updateable : Dateable{

        /// <summary>Date of this item's creation</summary>
        public DateTime DateUpdated { get; set; }
    }
}
