namespace Igtampe {
    
    /// <summary>Any item that has a date of creation</summary>
    public interface Dateable {

        /// <summary>Date of this item's creation</summary>
        public DateTime DateCreated { get; set; }
    }
}
