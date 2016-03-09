using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Security.Cryptography;

namespace OmniBean.PowerCrypt4
{
    /// <summary>
    /// Class to keep the basic RSA parameters like Keys, and other information.
    /// </summary>
    public class RSAProviderParameters : IDisposable
    {
        private int _ModulusOctets;
        private BigInteger _N;
        private BigInteger _P;
        private BigInteger _Q;
        private BigInteger _DP;
        private BigInteger _DQ;
        private BigInteger _InverseQ;
        private BigInteger _E;
        private BigInteger _D;
        private HashAlgorithm ha = SHA1Managed.Create();
        private int _hLen = 20;
        private bool _Has_CRT_Info = false;
        private bool _Has_PRIVATE_Info = false;
        private bool _Has_PUBLIC_Info = false;

        public enum RSAProviderHashAlgorithm { SHA1, SHA256, SHA512, UNDEFINED };

        public void Dispose()
        {
            ha.Dispose();
        }

        /// <summary>
        /// Computes the hash from the given data.
        /// </summary>
        /// <param name="data">The data to hash.</param>
        /// <returns>Hash of the data.</returns>
        public byte[] ComputeHash(byte[] data)
        {
            return ha.ComputeHash(data);
        }

        /// <summary>
        /// Gets and sets the HashAlgorithm for RSA-OAEP padding.
        /// </summary>
        public RSAProviderHashAlgorithm HashAlgorithm
        {
            get
            {
                RSAProviderHashAlgorithm al = RSAProviderHashAlgorithm.UNDEFINED;
                switch (ha.GetType().ToString())
                {
                    case "SHA1":
                        al = RSAProviderHashAlgorithm.SHA1;
                        break;

                    case "SHA256":
                        al = RSAProviderHashAlgorithm.SHA256;
                        break;

                    case "SHA512":
                        al = RSAProviderHashAlgorithm.SHA512;
                        break;
                }
                return al;
            }

            set
            {
                switch (value)
                {
                    case RSAProviderHashAlgorithm.SHA1:
                        ha = SHA1Managed.Create();
                        _hLen = 20;
                        break;

                    case RSAProviderHashAlgorithm.SHA256:
                        ha = SHA256Managed.Create();
                        _hLen = 32;
                        break;

                    case RSAProviderHashAlgorithm.SHA512:
                        ha = SHA512Managed.Create();
                        _hLen = 64;
                        break;
                }
            }
        }

        public bool HasCRTInfo
        {
            get
            {
                return _Has_CRT_Info;
            }
        }

        public bool Has_PRIVATE_Info
        {
            get
            {
                return _Has_PRIVATE_Info;
            }
        }

        public bool Has_PUBLIC_Info
        {
            get
            {
                return _Has_PUBLIC_Info;
            }
        }

        public int OctetsInModulus 
        { 
            get 
            {
                return _ModulusOctets;
            } 
        }

        public BigInteger N
        {
            get
            {
                return _N;
            }
        }

        public int hLen
        {
            get
            {
                return _hLen;
            }
        }

        public BigInteger P
        {
            get
            {
                return _P;
            }
        }

        public BigInteger Q
        {
            get
            {
                return _Q;
            }
        }

        public BigInteger DP
        {
            get
            {
                return _DP;
            }
        }

        public BigInteger DQ
        {
            get
            {
                return _DQ;
            }
        }

        public BigInteger InverseQ
        {
            get
            {
                return _InverseQ;
            }
        }

        public BigInteger E
        {
            get
            {
                return _E;
            }
        }

        public BigInteger D
        {
            get
            {
                return _D;
            }
        }

        /// <summary>
        /// Initialize the RSA class. It's assumed that both the Public and Extended Private info are there. 
        /// </summary>
        /// <param name="rsaParams">Preallocated RSAParameters containing the required keys.</param>
        /// <param name="ModulusSize">Modulus size in bits</param>
        public RSAProviderParameters(RSAParameters rsaParams, int ModulusSize)
        {
           // rsaParams;
            _ModulusOctets = ModulusSize / 8;
            _E = RSAProviderUtils.OS2IP(rsaParams.Exponent, false);
            _D = RSAProviderUtils.OS2IP(rsaParams.D, false);
            _N = RSAProviderUtils.OS2IP(rsaParams.Modulus, false);
            _P = RSAProviderUtils.OS2IP(rsaParams.P, false);
            _Q = RSAProviderUtils.OS2IP(rsaParams.Q, false);
            _DP = RSAProviderUtils.OS2IP(rsaParams.DP, false);
            _DQ = RSAProviderUtils.OS2IP(rsaParams.DQ, false);
            _InverseQ = RSAProviderUtils.OS2IP(rsaParams.InverseQ, false);
            _Has_CRT_Info = true;
            _Has_PUBLIC_Info = true;
            _Has_PRIVATE_Info = true;
        }

        /// <summary>
        /// Initialize the RSA class. Only the public parameters.
        /// </summary>
        /// <param name="Modulus">Modulus of the RSA key.</param>
        /// <param name="Exponent">Exponent of the RSA key</param>
        /// <param name="ModulusSize">Modulus size in number of bits. Ex: 512, 1024, 2048, 4096 etc.</param>
        public RSAProviderParameters(byte[] Modulus, byte[] Exponent, int ModulusSize)
        {
            // rsaParams;
            _ModulusOctets = ModulusSize / 8;
            _E = RSAProviderUtils.OS2IP(Exponent, false);            
            _N = RSAProviderUtils.OS2IP(Modulus, false);
            _Has_PUBLIC_Info = true;
        }

        /// <summary>
        /// Initialize the RSA class.
        /// </summary>
        /// <param name="Modulus">Modulus of the RSA key.</param>
        /// <param name="Exponent">Exponent of the RSA key</param>
        /// /// <param name="D">Exponent of the RSA key</param>
        /// <param name="ModulusSize">Modulus size in number of bits. Ex: 512, 1024, 2048, 4096 etc.</param>
        public RSAProviderParameters(byte[] Modulus, byte[] Exponent, byte [] D, int ModulusSize)
        {
            // rsaParams;
            _ModulusOctets = ModulusSize / 8;
            _E = RSAProviderUtils.OS2IP(Exponent, false);
            _N = RSAProviderUtils.OS2IP(Modulus, false);
            _D = RSAProviderUtils.OS2IP(D, false);
            _Has_PUBLIC_Info = true;
            _Has_PRIVATE_Info = true;
        }

        /// <summary>
        /// Initialize the RSA class. For CRT.
        /// </summary>
        /// <param name="Modulus">Modulus of the RSA key.</param>
        /// <param name="Exponent">Exponent of the RSA key</param>
        /// /// <param name="D">Exponent of the RSA key</param>
        /// <param name="P">P paramater of RSA Algorithm.</param>
        /// <param name="Q">Q paramater of RSA Algorithm.</param>
        /// <param name="DP">DP paramater of RSA Algorithm.</param>
        /// <param name="DQ">DQ paramater of RSA Algorithm.</param>
        /// <param name="InverseQ">InverseQ paramater of RSA Algorithm.</param>
        /// <param name="ModulusSize">Modulus size in number of bits. Ex: 512, 1024, 2048, 4096 etc.</param>
        public RSAProviderParameters(byte[] Modulus, byte[] Exponent, byte[] D, byte[] P, byte [] Q, byte [] DP, byte [] DQ, byte [] InverseQ, int ModulusSize)
        {
            // rsaParams;
            _ModulusOctets = ModulusSize / 8;
            _E = RSAProviderUtils.OS2IP(Exponent, false);
            _N = RSAProviderUtils.OS2IP(Modulus, false);
            _D = RSAProviderUtils.OS2IP(D, false);           
            _P = RSAProviderUtils.OS2IP(P, false);
            _Q = RSAProviderUtils.OS2IP(Q, false);
            _DP = RSAProviderUtils.OS2IP(DP, false);
            _DQ = RSAProviderUtils.OS2IP(DQ, false);
            _InverseQ = RSAProviderUtils.OS2IP(InverseQ, false);
            _Has_CRT_Info = true;
            _Has_PUBLIC_Info = true;
            _Has_PRIVATE_Info = true;
        }

    }
}
