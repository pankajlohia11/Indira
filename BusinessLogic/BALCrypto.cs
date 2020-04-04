using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Threading.Tasks;
using BusinessEntity.EntityModels;
using DataAccess;

namespace BusinessLogic
{
    public class BALCrypto
    {
        /// <summary>
        /// Use to encrypt a given password
        /// </summary>
        /// <param name="Source">The original password before encrypting</param>
        /// <param name="Key">The key used to encrypt</param>
        /// <returns>An encrypted String will be returned</returns>
        /// <example>
        /// This example will take the password "HelpMe" and return the encrypted string "YXjoo8/q6CsFF4NMqzUgVw=="
        /// <code>
        /// private void HelpMe()
        /// {
        ///   MessageBox.Show(Encrypting("HelpMe", "12345"));
        /// }
        /// </code></example>
        public string Encrypting(string Source, string Key)
        {
            SymmetricAlgorithm EncodeMethod = new RijndaelManaged();
            if (Source == null || Key == null || Source.Length == 0 || Key.Length == 0)
            {
                return null;
            }

            if (EncodeMethod == null)
            {
                return "Under Construction";
            }

            long lLen;
            int nRead, nReadTotal;
            byte[] buf = new byte[3];
            byte[] srcData;
            byte[] encData;
            System.IO.MemoryStream sin;
            System.IO.MemoryStream sout;
            CryptoStream encStream;

            srcData = System.Text.ASCIIEncoding.ASCII.GetBytes(Source);
            sin = new MemoryStream();
            sin.Write(srcData, 0, srcData.Length);
            sin.Position = 0;
            sout = new MemoryStream();

            EncodeMethod.Key = getValidKey(Key);
            EncodeMethod.IV = getValidIV(Key, EncodeMethod.IV.Length);

            encStream = new CryptoStream(sout,
                EncodeMethod.CreateEncryptor(),
                CryptoStreamMode.Write);
            lLen = sin.Length;
            nReadTotal = 0;
            while (nReadTotal < lLen)
            {
                nRead = sin.Read(buf, 0, buf.Length);
                encStream.Write(buf, 0, nRead);
                nReadTotal += nRead;
            }
            encStream.Close();

            encData = sout.ToArray();
            return System.Convert.ToBase64String(encData);
        }
        /// <summary>
        /// Use to decrypt a given password
        /// </summary>
        /// <param name="Source">The encrypted password</param>
        /// <param name="Key">The key used to decrypt</param>
        /// <returns>Decrypted String will be returned</returns>
        /// <example>
        /// This example will take the encrypted password "YXjoo8/q6CsFF4NMqzUgVw==" and return the string "HelpMe"
        /// <code>
        /// private void HelpMe()
        /// {
        ///   MessageBox.Show(Decrypting("YXjoo8/q6CsFF4NMqzUgVw==", "12345"));
        /// }
        /// </code></example>		
        public string Decrypting(string Source, string Key)
        {
            SymmetricAlgorithm EncodeMethod = new RijndaelManaged();
            if (Source == null || Key == null || Source.Length == 0 || Key.Length == 0)
            {
                return null;
            }

            if (EncodeMethod == null)
            {
                return "Under Construction";
            }
            long lLen;
            int nRead, nReadTotal;
            byte[] buf = new byte[3];
            byte[] decData;
            byte[] encData;
            System.IO.MemoryStream sin;
            System.IO.MemoryStream sout;
            CryptoStream decStream;

            encData = System.Convert.FromBase64String(Source);
            sin = new MemoryStream(encData);
            sout = new MemoryStream();

            EncodeMethod.Key = getValidKey(Key);
            EncodeMethod.IV = getValidIV(Key, EncodeMethod.IV.Length);

            decStream = new CryptoStream(sin,
                EncodeMethod.CreateDecryptor(),
                CryptoStreamMode.Read);

            lLen = sin.Length;
            nReadTotal = 0;
            while (nReadTotal < lLen)
            {
                nRead = decStream.Read(buf, 0, buf.Length);
                if (0 == nRead)
                {
                    break;
                }
                sout.Write(buf, 0, nRead);
                nReadTotal += nRead;
            }

            decStream.Close();

            decData = sout.ToArray();

            System.Text.ASCIIEncoding ascEnc = new System.Text.ASCIIEncoding();
            return ascEnc.GetString(decData);

        }
        /// <summary>
        /// Used to get the Valid key for encoding. This method is called from "Encrypting" and "Decrypting" methods.
        /// </summary>
        /// <param name="Key">The key used to encrypt the password</param>
        /// <returns>It returns the Valid encoding key</returns>
        private byte[] getValidKey(string Key)
        {
            SymmetricAlgorithm EncodeMethod = new RijndaelManaged();
            string sTemp;
            if (EncodeMethod.LegalKeySizes.Length > 0)
            {
                int lessSize = 0, moreSize = EncodeMethod.LegalKeySizes[0].MinSize;
                // key sizes are in bits

                while (Key.Length * 8 > moreSize &&
                    EncodeMethod.LegalKeySizes[0].SkipSize > 0 &&
                    moreSize < EncodeMethod.LegalKeySizes[0].MaxSize)
                {
                    lessSize = moreSize;
                    moreSize += EncodeMethod.LegalKeySizes[0].SkipSize;
                }

                if (Key.Length * 8 > moreSize)
                {
                    sTemp = Key.Substring(0, (moreSize / 8));
                }
                else
                {
                    sTemp = Key.PadRight(moreSize / 8, ' ');
                }
            }
            else
            {
                sTemp = Key;
            }
            // convert the secret key to byte array
            return System.Text.ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        /// <summary>
        /// Used to get the Valid IV key for encoding. This method is called from "Encrypting" and "Decrypting" methods.
        /// </summary>
        /// <param name="InitVector">The key used to encrypt the password</param>
        /// <param name="ValidLength">The length used in the encoding</param>
        /// <returns>It returns the Valid IV key</returns>
        private byte[] getValidIV(String InitVector, int ValidLength)
        {
            //SymmetricAlgorithm EncodeMethod;
            if (InitVector.Length > ValidLength)
            {
                return System.Text.ASCIIEncoding.ASCII.GetBytes(InitVector.Substring(0, ValidLength));
            }
            else
            {
                return System.Text.ASCIIEncoding.ASCII.GetBytes(InitVector.PadRight(ValidLength, ' '));
            }
        }
    }
}
