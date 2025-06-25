using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace RSA_Encryption
{
    public class Helpers
    {
        /// <summary>
        /// Gera dois números primos distintos e grandes aleatórios (por exemplo, 128 bits).
        /// </summary>
        /// <param name="bitLength">Tamanho do primo em bits.</param>
        /// <returns>Tupla contendo dois primos grandes distintos.</returns>
        public static Tuple<BigInteger, BigInteger> GerarDoisPrimosGrandes(int bitLength)
        {
            BigInteger p = GerarPrimoGrande(bitLength);
            BigInteger q;
            do
            {
                q = GerarPrimoGrande(bitLength);
            } while (q == p);
            return Tuple.Create(p, q);
        }

        /// <summary>
        /// Gera um primo grande aleatório com o número de bits especificado.
        /// </summary>
        /// <param name="bitLength">Tamanho em bits.</param>
        /// <returns>Primo grande.</returns>
        public static BigInteger GerarPrimoGrande(int bitLength)
        {
            BigInteger numero;
            do
            {
                numero = GerarNumeroAleatorio(bitLength);
            } while (!IsProvablePrime(numero, 16)); // 16 rodadas de Miller-Rabin
            return numero;
        }

        /// <summary>
        /// Gera um número aleatório positivo com o número de bits especificado.
        /// </summary>
        public static BigInteger GerarNumeroAleatorio(int bitLength)
        {
            int byteLength = (bitLength + 7) / 8;
            byte[] bytes = new byte[byteLength];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            bytes[bytes.Length - 1] &= (byte)(0xFF >> (8 * byteLength - bitLength)); // Garante o número de bits
            bytes[0] |= 1; // Garante ímpar (melhor chance de ser primo)
            return new BigInteger(bytes, isUnsigned: true, isBigEndian: true);
        }

        /// <summary>
        /// Testa se o número é primo usando Miller-Rabin.
        /// </summary>
        public static bool IsProvablePrime(BigInteger n, int k)
        {
            if (n < 2) return false;
            if (n == 2 || n == 3) return true;
            if (n % 2 == 0) return false;

            // Escreve n-1 como 2^s * d
            BigInteger d = n - 1;
            int s = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[n.ToByteArray().LongLength];
                for (int i = 0; i < k; i++)
                {
                    BigInteger a;
                    do
                    {
                        rng.GetBytes(bytes);
                        a = new BigInteger(bytes, isUnsigned: true, isBigEndian: true);
                    } while (a < 2 || a >= n - 2);

                    BigInteger x = BigInteger.ModPow(a, d, n);
                    if (x == 1 || x == n - 1)
                        continue;
                    bool continueOuter = false;
                    for (int r = 1; r < s; r++)
                    {
                        x = BigInteger.ModPow(x, 2, n);
                        if (x == 1) return false;
                        if (x == n - 1)
                        {
                            continueOuter = true;
                            break;
                        }
                    }
                    if (continueOuter)
                        continue;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Função para encontrar e coprimo de phi
        /// </summary>
        public static BigInteger FindCoprime(BigInteger phi)
        {
            BigInteger e = 65537; // valor padrão seguro e comum
            if (GCD(e, phi) == 1)
                return e;
            // Se não funcionar, busca outro
            for (e = 3; e < phi; e += 2)
            {
                if (GCD(e, phi) == 1)
                    return e;
            }
            throw new Exception("Não foi possível encontrar um coprimo adequado para phi.");
        }

        /// <summary>
        /// Calcula o máximo divisor comum (MDC) entre dois números.
        /// </summary>
        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            while (b != 0)
            {
                BigInteger temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        /// <summary>
        /// Calcula o inverso multiplicativo de 'a' módulo 'm'.
        /// </summary>
        public static BigInteger ModInverse(BigInteger a, BigInteger m)
        {
            BigInteger m0 = m, t, q;
            BigInteger x0 = 0, x1 = 1;
            if (m == 1) return 0;
            while (a > 1)
            {
                q = a / m;
                t = m;
                m = a % m;
                a = t;
                t = x0;
                x0 = x1 - q * x0;
                x1 = t;
            }
            if (x1 < 0)
                x1 += m0;
            return x1;
        }
    }
}