using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Igtampe.Hashbrown {

    /// <summary>The Hashbrown Hasher</summary>
    public class Hashbrown {

        private readonly string Salt;

        //private string Pepper; //Unlike most hashbrowns, we will not put pepper on this one, sadly.

        /// <summary>Whips up a Hashbrown. Yum.<br/><br/>
        /// Will attempt to load the salt for this hashbrown from the provided environment variable. If it is not set or is empty, 
        /// it will fallback and attempt to read a text file with the name of the environment variablewith the salt. 
        /// If that also fails, it will mine out some salt (generate a salt) and save it to the text file</summary>
        /// <param name="SaltEnvVar">The environment variable that contains this Hashbrown's salt</param>
        public Hashbrown(string? SaltEnvVar = null) {
            var EnvVar = SaltEnvVar ?? "HASHBROWN_SALT";
            Salt = Environment.GetEnvironmentVariable(EnvVar) ?? "";

            if (string.IsNullOrWhiteSpace(Salt)) {
                if (File.Exists($"{EnvVar}.txt")) { Salt = File.ReadAllText($"{EnvVar}.txt"); } else {
                    Salt = MineSalt();
                    Console.WriteLine($"Salt could not be found, so we mined one: {Salt}");
                    Console.WriteLine($"It was written to {EnvVar}.txt");
                    Console.WriteLine("Do not lose it! Any text that was hashed using this key will be lost if you do!");
                    File.WriteAllText($"{EnvVar}.txt", Salt);
                }
            }
        }

        /// <summary>Hashes a string value using the salt that's on this hashbrown</summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string Hash(string Value)
            => Convert.ToBase64String(KeyDerivation.Pbkdf2(
              password: Value, salt: Convert.FromBase64String(Salt),
              prf: KeyDerivationPrf.HMACSHA256,
              iterationCount: 100000,
              numBytesRequested: 256 / 8));

        /// <summary>We'll mine some salt for this hashbrown. It'll be 128 bits</summary>
        /// <returns>A Salt string that can be used with this hashbrown to hash text</returns>
        public static string MineSalt() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8));

    }
}
