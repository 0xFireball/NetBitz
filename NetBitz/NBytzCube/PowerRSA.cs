using System;
using System.Text;
using System.Security.Cryptography;

namespace OmniBean.PowerCrypt4
{
    public class PowerRSA
    {
        int KeySize;
        RSAProvider rsaProvider;
        RSACryptoServiceProvider csp;

        /// <summary>
        /// Disposes the cryptographic service provider and keeps it from persisting in the CSP Container.
        /// </summary>
        public void Dispose()
        {
            csp.PersistKeyInCsp = false;
        }

        public enum PHashAlgorithm
        {
            SHA1 = 0,
            SHA256 = 1,
            SHA512 = 2
        }

        public string PublicKey
        {
            get
            {
                return csp.ToXmlString(false);
            }
        }

        public string PrivateKey
        {
            get
            {
                return csp.ToXmlString(true);
            }
        }

        /// <summary>
        /// Initializes the RSA Provider with either only a public key or a public/private key pair.
        /// </summary>
        /// <param name="rsaKeyInfo">The XML string to initialize the RSA Provider with.</param>
        /// <param name="keySize">The length of the key.</param>
        /// <returns>PowerRSA object initialized with XML string.</returns>
        public PowerRSA(string rsaKeyInfo, int keySize)
        {
            this.KeySize = keySize;
            int keyLength = keySize;
            csp = new RSACryptoServiceProvider(keyLength);
            csp.FromXmlString(rsaKeyInfo);
            rsaProvider = new RSAProvider(rsaKeyInfo, keyLength);
            rsaProvider.RSAProviderHashAlgorithm = RSAProviderParameters.RSAProviderHashAlgorithm.SHA256;
        }

        /// <summary>
        /// Initializes the RSA Provider with either only a public key or a public/private key pair.
        /// </summary>
        /// <param name="rsaKeyInfo">The XML string to initialize the RSA Provider with.</param>
        /// <param name="keySize">The length of the key.</param>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        /// <returns>PowerRSA object initialized with XML string.</returns>
        public PowerRSA(string rsaKeyInfo, int keySize, PHashAlgorithm hashAlgorithm)
        {
            this.KeySize = keySize;
            int keyLength = keySize;
            csp = new RSACryptoServiceProvider(keyLength);
            csp.FromXmlString(rsaKeyInfo);
            rsaProvider = new RSAProvider(rsaKeyInfo, keyLength);
            switch ((int)hashAlgorithm)
            {
                case 0:
                    rsaProvider.RSAProviderHashAlgorithm = RSAProviderParameters.RSAProviderHashAlgorithm.SHA1;
                    break;
                case 1:
                    rsaProvider.RSAProviderHashAlgorithm = RSAProviderParameters.RSAProviderHashAlgorithm.SHA256;
                    break;
                case 2:
                    rsaProvider.RSAProviderHashAlgorithm = RSAProviderParameters.RSAProviderHashAlgorithm.SHA512;
                    break;
            }
        }

        private void InitRSA(int keySize, PHashAlgorithm hashAlgorithm)
        {
            this.KeySize = keySize;
            int keyLength = keySize;
            csp = new RSACryptoServiceProvider(keyLength);
            string rsaKeyInfo = csp.ToXmlString(true);//.Replace("><", ">\r\n<");
            rsaProvider = new RSAProvider(rsaKeyInfo, keyLength);
            switch ((int)hashAlgorithm)
            {
                case 0:
                    rsaProvider.RSAProviderHashAlgorithm = RSAProviderParameters.RSAProviderHashAlgorithm.SHA1;
                    break;
                case 1:
                    rsaProvider.RSAProviderHashAlgorithm = RSAProviderParameters.RSAProviderHashAlgorithm.SHA256;
                    break;
                case 2:
                    rsaProvider.RSAProviderHashAlgorithm = RSAProviderParameters.RSAProviderHashAlgorithm.SHA512;
                    break;
            }
        }
        /// <summary>
        /// Initializes the RSA Provider.
        /// </summary>
        /// <param name="keySize">The length of the key.</param>
        /// <returns>PowerRSA object.</returns>
        public PowerRSA(int keySize)
        {
            PHashAlgorithm ha = PHashAlgorithm.SHA256;
            InitRSA(keySize, ha);
        }
        /// <summary>
        /// Initializes the RSA Provider.
        /// </summary>
        /// <param name="keySize">The length of the key.</param>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        /// <returns>PowerRSA object.</returns>
        public PowerRSA(int keySize, PHashAlgorithm hashAlgorithm)
        {
            InitRSA(keySize, hashAlgorithm);
        }

        public string EncryptStringWithPublicKey(string plainText)
        {
            byte[] CTX = rsaProvider.Encrypt(Encoding.UTF8.GetBytes(plainText), false, true);
            string CipherText = Convert.ToBase64String(CTX);
            return CipherText;
        }
        public string EncryptStringWithPrivateKey(string plainText)
        {
            byte[] CTX = rsaProvider.Encrypt(Encoding.UTF8.GetBytes(plainText), true, true);
            string CipherText = Convert.ToBase64String(CTX);
            return CipherText;
        }
        public string DecryptStringWithPrivateKey(string cipherText)
        {
            byte[] CTX = Convert.FromBase64String(cipherText);
            byte[] PTX = rsaProvider.Decrypt(CTX, true, true);
            string DecryptedString = Encoding.UTF8.GetString(PTX);
            return DecryptedString;
        }
        public string DecryptStringWithPublicKey(string cipherText)
        {
            byte[] CTX = Convert.FromBase64String(cipherText);
            byte[] PTX = rsaProvider.Decrypt(CTX, false, true);
            string DecryptedString = Encoding.UTF8.GetString(PTX);
            return DecryptedString;
        }
    }
}