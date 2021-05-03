using System;
using System.Numerics;
using System.Collections.Generic;

namespace RSA
{
  class Keypair
  {
    //numbeic values for keypair generation
    public BigInteger r = 0;       // p * q
    private BigInteger phi_r = 0;  // (p - 1) * (q - 1)
    public BigInteger e = 0;       //1 < e < phi_r, gcd(phi_r, e) = 1
    public BigInteger d = 0;       // (e * d) mod phi_r = 1

    //generates keypair for given primes p and q

    public Keypair(){}
    public Keypair(BigInteger p, BigInteger q, BigInteger exponent, BigInteger r, BigInteger d) {
      this.r = r;
      if (IsPrime(p) && IsPrime(q)){
        this.r = calc_r(p, q);
        phi_r = calc_phi_r(p, q);
      }
      e = exponent;
      if (BigInteger.Compare(exponent, BigInteger.Zero)==0){
        e = calc_e(phi_r);
      }
      if (BigInteger.Compare(d,BigInteger.Zero)==0){
        this.d = calc_d(phi_r, e);
      }else{
        this.d = d;
      }
      
    }

    //test if testNum is prime and return bool value according to result
    public bool IsPrime(BigInteger testNum) {
      if (testNum.CompareTo(2) < 0) {
        return false;
      }
      for (BigInteger i = 2; BigInteger.Compare(i, testNum) < 0; i++) {
        if (BigInteger.Compare(BigInteger.Remainder(testNum, i), BigInteger.Zero) == 0) {
          return false;
        }
      }
      return true;
    }

    //calculates r = p * q ...
    public BigInteger calc_r(BigInteger p, BigInteger q) {
      return BigInteger.Multiply(p, q);
    }

    //calculates phi_r = (p-1)*(q-1)
    public BigInteger calc_phi_r(BigInteger p, BigInteger q) {
      BigInteger buf1, buf2;
      buf1 = BigInteger.Subtract(p, BigInteger.One);
      buf2 = BigInteger.Subtract(q, BigInteger.One);
      return BigInteger.Multiply(buf1, buf2);
    }

    //calculates exponent
    public BigInteger calc_e(BigInteger phi_r) {
  //    Random rand = new Random();
      BigInteger i = 2;
      List<BigInteger> e_candidates = new List<BigInteger>();
      //e < phi_r
      while (BigInteger.Compare(i, phi_r) < 0) {
        
        //e and phi_r should be mutually prime
        while (BigInteger.Compare(BigInteger.GreatestCommonDivisor(phi_r, i), 1) != 0) {
          i = BigInteger.Add(i, BigInteger.One);
        }

        //check if number found is ok 
        if (BigInteger.Compare(i, phi_r) < 0) {
          e_candidates.Add(i);
        }

        i = BigInteger.Add(i, BigInteger.One);
      }
      if (e_candidates.Count == 0) {
        return BigInteger.Zero;
      }
      //just pick random candidate
     // int val = rand.Next(e_candidates.Count - 1);
      return e_candidates[e_candidates.Count -1];
    }

    //calculates d using euclidian algorhythm
    public BigInteger calc_d(BigInteger phi_r, BigInteger e) {
      BigInteger d0, d1, d2; 
      BigInteger x0, x1, x2;
      BigInteger y0, y1, y2, q;
      d0 = phi_r;
      d1 = e;
      x0 = y1 = BigInteger.One;
      y0 = x1 = BigInteger.Zero;
      while (BigInteger.Compare(d1, BigInteger.One) > 0) {
        q = BigInteger.DivRem(d0, d1, out d2);
        x2 = BigInteger.Subtract(x0, BigInteger.Multiply(q, x1));
        y2 = BigInteger.Subtract(y0, BigInteger.Multiply(q, y1));
        d0 = d1; d1 = d2;
        x0 = x1; x1 = x2;
        y0 = y1; y1 = y2;
      }

      if (BigInteger.Compare(y1, BigInteger.Zero) < 0) {
        y1 = BigInteger.Add(y1,phi_r);
      }
      return y1;
    }
  }
}