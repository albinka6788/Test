using BHIC.Common.XmlHelper;
using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

using BHIC.Common.CommonUtilities;

namespace BHIC.Common
{
    public static class Encryption
    {
        #region Private Variables

        private const int iterations = 10000;
        private const int keySize = 256;
        private const int blockSize = 128;
        private const int saltSize = 16;
        private const int byteSize = 8;
        private const int BUFFER_SIZE = 128 * 1024;
        private const string ReplaceChars = "~|~";
        private const string ReplaceSlash = "|!~";

        /// <summary>
        /// Tag to make sure this file is readable/decryptable by this class
        /// </summary>
        private const ulong FC_TAG = 0xFC010203040506CF;

        #endregion

        #region Public Methods

        /// <summary>
        /// This method will encrypt the target bytes with the password bytes provided
        /// </summary>
        /// <param name="bytesToBeEncrypted"></param>
        /// <param name="passwordBytes"></param>
        /// <returns name="encryptedBytes"></returns>
        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes, byte[] saltValue = null, bool saveSaltKey = true)
        {
            byte[] encryptedBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = keySize;
                    AES.BlockSize = blockSize;
                    byte[] saltBytes = (saltValue == null ? GetRandomBytes() : saltValue);
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, iterations);
                    AES.Key = key.GetBytes(AES.KeySize / byteSize);
                    AES.IV = key.GetBytes(AES.BlockSize / byteSize);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    if (saveSaltKey)
                    {
                        encryptedBytes = Combine(saltBytes, ms.ToArray());
                    }
                    else
                    {
                        encryptedBytes = ms.ToArray();
                    }
                }
            }

            return encryptedBytes;
        }

        /// <summary>
        /// This method will decrypt the target bytes with the password bytes provided
        /// </summary>
        /// <param name="bytesToBeDecrypted"></param>
        /// <param name="passwordBytes"></param>
        /// <returns name="decryptedBytes"></returns>
        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes, byte[] saltValue = null)
        {
            byte[] decryptedBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = keySize;
                    AES.BlockSize = blockSize;
                    byte[] saltBytes = new byte[saltSize];
                    byte[] decryptBytes = new byte[(bytesToBeDecrypted.Length - saltSize)];

                    if (saltValue == null)
                    {
                        Array.Copy(bytesToBeDecrypted, saltBytes, saltSize);
                    }
                    else
                    {
                        saltBytes = saltValue;
                    }
                    Array.Copy(bytesToBeDecrypted, saltSize, decryptBytes, 0, decryptBytes.Length);

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, iterations);
                    AES.Key = key.GetBytes(AES.KeySize / byteSize);
                    AES.IV = key.GetBytes(AES.BlockSize / byteSize);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(decryptBytes, 0, decryptBytes.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        /// <summary>
        /// This method will encrypt the input string with password provided
        /// </summary>
        /// <param name="input"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncryptText(string input, string password = null)
        {
            password = (password == null ? ConfigCommonKeyReader.DefaultPassword : password);
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(bytesEncrypted).Replace("+", ReplaceChars).Replace("/", ReplaceSlash);
        }

        /// <summary>
        /// This method will decrypt the input string with password provided
        /// </summary>
        /// <param name="input"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string DecryptText(string input, string password = null)
        {
            return Encoding.UTF8.GetString(DecryptTextByte(input, password));
        }

        /// <summary>
        /// This method will decrypt the input string with password provided in Secure String format
        /// </summary>
        /// <param name="input"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static SecureString DecryptTextSecure(string input, string password = null)
        {
            return UtilityFunctions.ConvertToSecureString(Encoding.UTF8.GetString(DecryptTextByte(input, password)));
        }

        /// <summary>
        /// This method will encrypt the input string with password provided using salt bytes
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string EncryptWithStaticKey(string text, string pwd = null)
        {
            pwd = (pwd == null ? ConfigCommonKeyReader.DefaultPassword : pwd);
            byte[] originalBytes = Encoding.UTF8.GetBytes(text);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pwd);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            // Generating salt bytes
            byte[] saltBytes = Encoding.UTF8.GetBytes((!string.IsNullOrEmpty(ConfigCommonKeyReader.DefaultSecureKey) ? ConfigCommonKeyReader.DefaultSecureKey : pwd));

            byte[] bytesEncrypted = AES_Encrypt(originalBytes, passwordBytes, saltBytes, false);

            return Convert.ToBase64String(bytesEncrypted).Replace("+", ReplaceChars).Replace("/", ReplaceSlash);
        }

        /// <summary>
        /// This method will decrypt the input string with password provided using salt bytes
        /// </summary>
        /// <param name="decryptedText"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private static string Decrypt(string decryptedText, string pwd = null)
        {
            pwd = (pwd == null ? ConfigCommonKeyReader.DefaultPassword : pwd);
            byte[] bytesToBeDecrypted = Convert.FromBase64String(decryptedText);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pwd);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] decryptedBytes = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            // Removing salt bytes, retrieving original bytes
            byte[] originalBytes = new byte[decryptedBytes.Length - saltSize];
            for (int i = saltSize; i < decryptedBytes.Length; i++)
            {
                originalBytes[i - saltSize] = decryptedBytes[i];
            }

            return Encoding.UTF8.GetString(originalBytes);
        }

        /// <summary>
        /// This method will encrypt file and copy it at output path provided with salt and password
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        /// <param name="password"></param>
        public static void EncryptFile(string inputPath, string outputPath, string password = null)
        {
            password = (password == null ? ConfigCommonKeyReader.DefaultPassword : password);
            var input = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
            var output = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write);
            //EncryptFileStream(input, output, password);
            input.EncryptFile(output, password);
        }

        /// <summary>
        /// Encrypt Stream before saving to specified location
        /// </summary>
        /// <param name="inFile"></param>
        /// <param name="outFile"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static void EncryptFile(this Stream inFile, FileStream outFile, string password)
        {
            try
            {
                inFile.Position = 0;
                long lSize = inFile.Length; // the size of the input file for storing
                int size = (int)lSize;  // the size of the input file for progress
                byte[] bytes = new byte[BUFFER_SIZE]; // the buffer
                int read = -1; // the amount of bytes read from the input file
                int value = 0; // the amount overall read from the input file for progress

                // generate IV and Salt
                byte[] IV = GetRandomBytes();
                byte[] salt = GetRandomBytes();

                // create the crypting object
                //SymmetricAlgorithm sma = CreateAES(password, salt);
                RijndaelManaged sma = CreateAES(password, salt);
                sma.IV = IV;

                // write the IV and salt to the beginning of the file
                outFile.Write(IV, 0, IV.Length);
                outFile.Write(salt, 0, salt.Length);

                // create the hashing and crypto streams
                HashAlgorithm hasher = SHA512.Create();
                using (CryptoStream cout = new CryptoStream(outFile, sma.CreateEncryptor(), CryptoStreamMode.Write),
                          chash = new CryptoStream(Stream.Null, hasher, CryptoStreamMode.Write))
                {
                    // write the size of the file to the output file
                    BinaryWriter bw = new BinaryWriter(cout);
                    bw.Write(lSize);

                    // write the file cryptor tag to the file
                    bw.Write(FC_TAG);

                    // read and the write the bytes to the crypto stream in BUFFER_SIZEd chunks
                    while ((read = inFile.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        cout.Write(bytes, 0, read);
                        chash.Write(bytes, 0, read);
                        value += read;
                    }
                    // flush and close the hashing object
                    chash.Flush();
                    chash.Close();

                    // read the hash
                    byte[] hash = hasher.Hash;

                    // write the hash to the end of the file
                    cout.Write(hash, 0, hash.Length);

                    // flush and close the cryptostream
                    cout.Flush();
                    cout.Close();
                }
                if (outFile.CanSeek && outFile.Length > 0 && outFile.Position != 0)
                {
                    outFile.Position = 0;
                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// This method will decrypt file and copy it at output path provided with salt and password
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        /// <param name="password"></param>
        public static void DecryptFile(string inputPath, string outputPath, string password = null)
        {
            password = (password == null ? ConfigCommonKeyReader.DefaultPassword : password);
            var input = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
            var output = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write);
            //DecryptFileStream(input, output, password);
            input.DecryptFile(output, password);
        }

        /// <summary>
        /// Decrypt Stream before saving to specified location
        /// </summary>
        /// <param name="inFile"></param>
        /// <param name="outFile"></param>
        /// <param name="password"></param>
        public static void DecryptFile(this Stream inFile, Stream outFile, string password)
        {
            try
            {
                inFile.Position = 0;
                int size = (int)inFile.Length; // the size of the file for progress notification
                byte[] bytes = new byte[BUFFER_SIZE]; // byte buffer
                int read = -1; // the amount of bytes read from the stream
                int value = 0;
                int outValue = 0; // the amount of bytes written out

                // read off the IV and Salt
                byte[] IV = new byte[saltSize];
                inFile.Read(IV, 0, saltSize);
                byte[] salt = new byte[saltSize];
                inFile.Read(salt, 0, saltSize);

                // create the crypting stream
                //SymmetricAlgorithm sma = CreateAES(password, salt);
                RijndaelManaged sma = CreateAES(password, salt);
                sma.IV = IV;

                value = 32; // the value for the progress
                long lSize = -1; // the size stored in the input stream

                // create the hashing object, so that we can verify the file
                HashAlgorithm hasher = SHA512.Create();

                // create the cryptostreams that will process the file
                using (CryptoStream cin = new CryptoStream(inFile, sma.CreateDecryptor(), CryptoStreamMode.Read),
                          chash = new CryptoStream(Stream.Null, hasher, CryptoStreamMode.Write))
                {
                    // read size from file
                    BinaryReader br = new BinaryReader(cin);
                    lSize = br.ReadInt64();
                    ulong tag = br.ReadUInt64();

                    if (FC_TAG != tag)
                    {
                        throw new Exception("Either File is Corrupted or not a valid file!");
                    }

                    //determine number of reads to process on the file
                    long numReads = lSize / BUFFER_SIZE;

                    // determine what is left of the file, after numReads
                    long slack = (long)lSize % BUFFER_SIZE;

                    // read the buffer_sized chunks
                    for (int i = 0; i < numReads; ++i)
                    {
                        read = cin.Read(bytes, 0, bytes.Length);
                        outFile.Write(bytes, 0, read);
                        chash.Write(bytes, 0, read);
                        value += read;
                        outValue += read;
                    }

                    // now read the slack
                    if (slack > 0)
                    {
                        read = cin.Read(bytes, 0, (int)slack);
                        outFile.Write(bytes, 0, read);
                        chash.Write(bytes, 0, read);
                        value += read;
                        outValue += read;
                    }
                    // flush and close the hashing stream
                    chash.Flush();
                    chash.Close();

                    // read the current hash value
                    byte[] curHash = hasher.Hash;

                    // get and compare the current and old hash values
                    byte[] oldHash = new byte[hasher.HashSize / 8];
                    read = cin.Read(oldHash, 0, oldHash.Length);
                    if ((oldHash.Length != read) || (!CheckByteArrays(oldHash, curHash)))
                        throw new Exception("Either File is Corrupted or not a valid file!");
                }

                // make sure the written and stored size are equal
                if (outValue != lSize)
                    throw new Exception("File Sizes don't match, either File is Corrupted or not a valid file!");

                if (outFile.CanSeek && outFile.Length > 0 && outFile.Position != 0)
                {
                    outFile.Position = 0;
                }

            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// This method will copy the contents of an Input File to the output file stream
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        private static void CopyStream(Stream input, Stream output)
        {
            //using (output)
            {
                using (input)
                {
                    byte[] buffer = new byte[BUFFER_SIZE];
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        output.Write(buffer, 0, read);
                    }
                }
            }
        }

        /// <summary>
        /// This method will generate Random Bytes for use as salt value
        /// </summary>
        /// <returns></returns>
        private static byte[] GetRandomBytes()
        {
            byte[] ba = new byte[saltSize];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }

        /// <summary>
        /// Combining multiple bytes
        /// </summary>
        /// <param name="arrays"></param>
        /// <returns></returns>
        public static byte[] Combine(params byte[][] arrays)
        {
            byte[] ret = new byte[arrays.Sum(x => x.Length)];
            int offset = 0;
            foreach (byte[] data in arrays)
            {
                Buffer.BlockCopy(data, 0, ret, offset, data.Length);
                offset += data.Length;
            }
            return ret;
        }

        /// <summary>
        /// Creates a AES SymmetricAlgorithm for use in EncryptFile and DecryptFile
        /// </summary>
        /// <param name="password">the string to use as the password</param>
        /// <param name="salt">the salt to use with the password</param>
        /// <returns>A SymmetricAlgorithm for encrypting/decrypting with Rijndael</returns>
        //private static SymmetricAlgorithm CreateAES(string password, byte[] salt)
        private static RijndaelManaged CreateAES(string password, byte[] salt)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, salt, "SHA512", iterations);

            //SymmetricAlgorithm sma = Rijndael.Create();
            RijndaelManaged sma = new RijndaelManaged();
            sma.KeySize = keySize;
            sma.Key = pdb.GetBytes(32);
            sma.Padding = PaddingMode.PKCS7;
            return sma;
        }

        /// <summary>
        /// Checks to see if two byte array are equal
        /// </summary>
        /// <param name="b1">the first byte array</param>
        /// <param name="b2">the second byte array</param>
        /// <returns>true if b1.Length == b2.Length and each byte in b1 is
        /// equal to the corresponding byte in b2</returns>
        private static bool CheckByteArrays(byte[] b1, byte[] b2)
        {
            if (b1.Length == b2.Length)
            {
                for (int i = 0; i < b1.Length; ++i)
                {
                    if (b1[i] != b2[i])
                        return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Decrypt text and return bytes.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static byte[] DecryptTextByte(string input, string password = null)
        {
            password = (password == null ? ConfigCommonKeyReader.DefaultPassword : password);
            input = input.Replace(ReplaceChars, "+").Replace(ReplaceSlash, "/");
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            return AES_Decrypt(bytesToBeDecrypted, passwordBytes);
        }
        #endregion
    }
}
