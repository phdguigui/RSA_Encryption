using RSA_Encryption;
using System.Numerics;

Console.WriteLine("RSA - Criptografia Assimétrica\n");

BigInteger p, q;

// Escolha do método de seleção dos primos
while (true)
{
    Console.WriteLine("Escolha uma opção:");
    Console.WriteLine("1 - Gerar valores de p e q aleatoriamente");
    Console.WriteLine("2 - Digitar valores de p e q manualmente\nEx: p = 174166265209585933744872919681332961507 | q = 257108794072914633043712785291406516647\n");
    Console.Write("Opção: ");
    string opcao = Console.ReadLine();
    if (opcao == "1")
    {
        Console.Write("Digite o tamanho dos primos em bits (ex: 128 ou 256): ");
        int bits = int.Parse(Console.ReadLine());
        var primos = Helpers.GerarDoisPrimosGrandes(bits);
        p = primos.Item1;
        q = primos.Item2;
        Console.WriteLine($"Primos gerados:\np = {p}\nq = {q}");
        break;
    }
    else if (opcao == "2")
    {
        while (true)
        {
            Console.Write("Digite o valor do primo p: ");
            if (BigInteger.TryParse(Console.ReadLine(), out p) && Helpers.IsProvablePrime(p, 16))
                break;
            Console.WriteLine("Valor inválido! p deve ser um número primo grande.");
        }

        while (true)
        {
            Console.Write("Digite o valor do primo q (diferente de p): ");
            if (BigInteger.TryParse(Console.ReadLine(), out q) && Helpers.IsProvablePrime(q, 16) && q != p)
                break;
            Console.WriteLine("Valor inválido! q deve ser um número primo grande e diferente de p.");
        }
        break;
    }
    else
    {
        Console.WriteLine("Opção inválida. Tente novamente.\n");
    }
}

// Entrada da mensagem
Console.Write("Digite a mensagem: ");
string mensagem = Console.ReadLine();

// Conversão da mensagem em ASCII
List<int> asciiOriginal = new List<int>();
foreach (char c in mensagem)
    asciiOriginal.Add((int)c);

Console.WriteLine("\nMensagem ASCII: " + string.Join(" ", asciiOriginal));

// Gerar chaves pública e privada
BigInteger n = p * q;
// φ(n) = (p - 1) * (q - 1)
BigInteger phi = (p - 1) * (q - 1);

// Escolher e, sendo um número coprimo com φ(n) e 1 < e < φ(n)
BigInteger e = Helpers.FindCoprime(phi);
// d ≡ e^(-1) (mod φ(n))
// Isso é d * e ≡ 1 (mod φ(n))
// Ou seja, um número d tal que (d * e) % φ(n) = 1
BigInteger d = Helpers.ModInverse(e, phi);

Console.WriteLine($"\nChave Pública: (n = {n}, e = {e})");
Console.WriteLine($"Chave Privada: (n = {n}, d = {d})");

// Criptografar mensagem
List<BigInteger> criptografada = new List<BigInteger>();
foreach (int ascii in asciiOriginal)
    // Criptografia RSA: c ≡ m^e (mod n)
    // Ou seja, c = ascii^e mod n
    criptografada.Add(BigInteger.ModPow(ascii, e, n));

Console.WriteLine("\nMensagem Criptografada: " + string.Join(" ", criptografada));

// Descriptografar mensagem
List<int> descriptografada = new List<int>();
foreach (BigInteger c in criptografada)
    // Descriptografia RSA: m ≡ c^d (mod n)
    // Ou seja, m = c^d mod n
    descriptografada.Add((int)BigInteger.ModPow(c, d, n));

Console.WriteLine("\nMensagem Descriptografada (ASCII): " + string.Join(" ", descriptografada));

// Converter ASCII para string
string mensagemFinal = "";
foreach (int ascii in descriptografada)
    mensagemFinal += (char)ascii;

Console.WriteLine("Mensagem Final: " + mensagemFinal);