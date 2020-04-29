using JuliusCaesarCryptographyChallengeCodenation.Models;
using JuliusCaesarCryptographyChallengeCodenation.Tools;
using System.Text;

namespace JuliusCaesarCryptographyChallengeCodenation.Services
{
    public class MessageDecrypter
    {
        public EnigmaModel Decripter(EnigmaModel enigma)
        {
            var enigmaArray = enigma.Cifrado.ToLower().ToCharArray();

            int variacao = (-1) * enigma.NumeroCasas;
            StringBuilder result = new StringBuilder();

            // 97 ~> a até z <~ 122
            foreach (var character in enigmaArray)
            {
                int ascii = (int)character;

                if (ascii >= 97 && ascii <= 122) // caracteres que me interessam
                {
                    // b ~> 98 + (-3) = 95 ~> true
                    if (ascii + variacao < 97) // variacao negativa
                    {
                        // teremos (95 - 97) + 123 = 121
                        var caracterNegativo = ((ascii + variacao) - 97) + 123;
                        //Console.Write((char)caracterNegativo); // temos o y
                        result.Append((char)caracterNegativo);
                    }
                    // y ~> 121 + (+3) = 124 ~> true
                    else if (ascii + variacao > 122) // variação positiva
                    {
                        // teremos (124 - 122) + 96 = 98
                        var caracterPositivo = ((ascii + variacao) - 122) + 96;
                        //Console.Write((char)caracterPositivo); // temos o b
                        result.Append((char)caracterPositivo);

                    }
                    else // variação não viola negativamente e nem positivamente
                    {
                        //Console.Write((char)(ascii + variacao));
                        result.Append((char)(ascii + variacao));

                    }
                }
                else // caracteres que não me intreressam a conversão
                {
                    //Console.Write((char)ascii);
                    result.Append((char)ascii);
                }
            }

            var sha1 = new Sha1Generator();

            var enigmaDecrypted = result.ToString();
            var resumoCriptografico = sha1.Hash(enigmaDecrypted);

            return new EnigmaModel()
            {
                Cifrado = enigma.Cifrado,
                Decifrado = enigmaDecrypted,
                NumeroCasas = enigma.NumeroCasas,
                Token = enigma.Token,
                ResumoCriptografico = resumoCriptografico
            };
        }
    }
}
