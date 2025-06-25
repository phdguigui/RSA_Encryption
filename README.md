# RSA Encryption

This project implements the [RSA encryption algorithm](https://en.wikipedia.org/wiki/RSA_(cryptosystem)), a foundational public-key cryptosystem in cryptography. RSA’s security is based on the mathematical difficulty of factoring large composites and uses concepts from number theory, modular arithmetic, and Euler's theorem.

This implementation, developed in C# for the Information Security course at UDESC, demonstrates all core RSA steps: key generation, encryption, and decryption. The code provides both automatic (random) and manual (user input) generation of large prime numbers, ASCII-based message handling, and all essential number-theoretical routines.

---

## Introduction

RSA (Rivest–Shamir–Adleman) is a public-key cryptosystem for secure data transmission. It supports both encryption and digital signatures. Its security relies on the practical difficulty of factoring the product of two large prime numbers.

---

## Mathematical Background

All formulas in this section are written in LaTeX for clarity.

### Key Generation

Compute the modulus:

$$
n = p \times q
$$

Compute Euler’s totient:

$$
\varphi(n) = (p-1)(q-1)
$$

Choose public exponent:

$$
1 < e < \varphi(n),\quad \gcd(e, \varphi(n)) = 1
$$

Compute private exponent (modular inverse of \( e \)):

$$
d \equiv e^{-1} \pmod{\varphi(n)}
$$

Which means:

$$
d \cdot e \equiv 1 \pmod{\varphi(n)}
$$

Public key:

$$
(n, e)
$$

Private key:

$$
(n, d)
$$

---

### Encryption

Given message:

$$
0 \leq m < n
$$

Encryption:

$$
c \equiv m^e \pmod{n}
$$

Where:
- \( c \): ciphertext
- \( m \): plaintext message

---

### Decryption

Given ciphertext:

$$
c
$$

Decryption:

$$
m \equiv c^d \pmod{n}
$$

---

### Digital Signatures

To sign a message:

$$
s \equiv m^d \pmod{n}
$$

To verify a signature:

$$
m \stackrel{?}{=} s^e \pmod{n}
$$

---

## How It Works

RSA is based on the principle that, while it is easy to multiply large numbers, it is computationally hard to factor their product. The correctness of RSA encryption and decryption is guaranteed by Euler’s theorem:

$$
(m^e)^d \equiv m^{ed} \equiv m \pmod{n}
$$

when

$$
ed \equiv 1 \pmod{\varphi(n)}
$$

---

## Usage Example

Suppose:

$$
p = 61
$$

$$
q = 53
$$

$$
n = 3233
$$

$$
\varphi(n) = 3120
$$

$$
e = 17
$$

$$
d = 2753
$$

Encrypt:

$$
m = 123
$$

$$
c = 123^{17} \bmod 3233 = 855
$$

Decrypt:

$$
m = 855^{2753} \bmod 3233 = 123
$$

Given:

$$
\varphi(n) = 3120
$$

---

## Project Structure and Main Logic

- **Program.cs**: User interface for prime selection (random/manual), message input, ASCII encoding, encryption, and decryption.
- **Helpers.cs**: All number-theoretical helpers for prime generation, primality testing (Miller-Rabin), coprime finding, modular inverse, and random number generation.

#### Main algorithm steps

```csharp
// 1. Escolher p e q (grandes primos)
// 2. Calcular n = p * q e phi = (p - 1) * (q - 1)
// 3. Encontrar e (coprimo de phi) e d (inverso modular de e)
// 4. Converter mensagem para ASCII
// 5. Criptografar: c = m^e mod n
// 6. Descriptografar: m = c^d mod n
```
---

> **Disclaimer:** This implementation was developed for educational purposes for the Information Security course at UDESC.
