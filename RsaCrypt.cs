using System;
using System.Text;
using System.Numerics;

namespace RSA
{
  static class RSA_crypt
  {
		public static String Encrypt(string encString, Keypair keys) {
      byte[] encBytes = Encoding.UTF8.GetBytes(encString);
			string resString = "";
			for(int i = 0; i < encBytes.Length; i++) { 
				resString += BigInteger.ModPow(encBytes[i], keys.e, keys.r).ToString() + "|";
			}
      return resString;
    }

    public static String Decrypt(string encString, Keypair keys)
    {
      string[] encSequence = encString.TrimEnd('|').Split('|');
      byte[] encBytes = new byte[encSequence.Length];
      for (int i = 0; i < encBytes.Length; i++) {
        encBytes[i] = (byte)BigInteger.ModPow(BigInteger.Parse(encSequence[i]), keys.d, keys.r);
      }
      return Encoding.UTF8.GetString(encBytes);
    }
  }
}