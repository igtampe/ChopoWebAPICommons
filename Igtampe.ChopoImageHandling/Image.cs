using System.Text.Json.Serialization;

namespace Igtampe.ChopoImageHandling {

    /// <summary>Image stored on the database</summary>
    public class Image : AutomaticallyGeneratableIdentifiable {

        /// <summary>Data of this image</summary>
        [JsonIgnore]
        public byte[]? Data { get; set; }

        /// <summary>MIME Type of this image (image/png)</summary>
        public string Type { get; set; } = "";

        /// <summary>Gives basic information of this Image</summary>
        /// <returns></returns>
        public override string ToString() => $"Image \'{ID}\' ({Type})";
    }
}
