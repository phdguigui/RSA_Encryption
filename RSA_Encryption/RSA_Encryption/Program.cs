using RSA_Encryption;
using System.Numerics;

Console.WriteLine("RSA - Criptografia Assimétrica\n");

BigInteger p, q;

// Escolha do método de seleção dos primos
while (true)
{
    Console.WriteLine("Escolha uma opção:");
    Console.WriteLine("1 - Gerar valores de p e q aleatoriamente (grandes)");
    Console.WriteLine("2 - Digitar valores de p e q manualmente");
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
Console.Write("Digite a mensagem (ex: abc): ");
string mensagem = Console.ReadLine();

// Conversão da mensagem em ASCII
List<int> asciiOriginal = new List<int>();
foreach (char c in mensagem)
    asciiOriginal.Add((int)c);

Console.WriteLine("\nMensagem ASCII: " + string.Join(" ", asciiOriginal));

// Gerar chaves pública e privada
BigInteger n = p * q;
BigInteger phi = (p - 1) * (q - 1);

// Escolher automaticamente e (coprimo de phi)
BigInteger e = Helpers.FindCoprime(phi);
BigInteger d = Helpers.ModInverse(e, phi);

Console.WriteLine($"\nChave Pública: (n = {n}, e = {e})");
Console.WriteLine($"Chave Privada: (n = {n}, d = {d})");

// Criptografar mensagem
List<BigInteger> criptografada = new List<BigInteger>();
foreach (int ascii in asciiOriginal)
    criptografada.Add(BigInteger.ModPow(ascii, e, n));

Console.WriteLine("\nMensagem Criptografada: " + string.Join(" ", criptografada));

// Descriptografar mensagem
List<int> descriptografada = new List<int>();
foreach (BigInteger c in criptografada)
    descriptografada.Add((int)BigInteger.ModPow(c, d, n));

Console.WriteLine("\nMensagem Descriptografada (ASCII): " + string.Join(" ", descriptografada));

// Converter ASCII para string
string mensagemFinal = "";
foreach (int ascii in descriptografada)
    mensagemFinal += (char)ascii;

Console.WriteLine("Mensagem Final: " + mensagemFinal);