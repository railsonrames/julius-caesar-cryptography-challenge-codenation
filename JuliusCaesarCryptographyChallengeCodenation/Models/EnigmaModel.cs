using System.Text.Json.Serialization;

namespace JuliusCaesarCryptographyChallengeCodenation.Models
{
    public class EnigmaModel
    {
        [JsonPropertyName("numero_casas")]
        public int NumeroCasas { get; set; }
        public string Token { get; set; }
        public string Cifrado { get; set; }
        public string Decifrado { get; set; }
        [JsonPropertyName("resumo_criptografico")]
        public string ResumoCriptografico { get; set; }
    }
}
