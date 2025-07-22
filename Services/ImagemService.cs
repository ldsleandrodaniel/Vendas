namespace Pedidos.Services
{
    public class ImagemService
    {
        private const string PastaProdutos = "images/produtos";
        private const string ImagemPadrao = "images/sem-imagem.jpg";

        public string ObterCaminhoImagem(string caminhoOuUrl)
        {
            // Caso não tenha imagem definida
            if (string.IsNullOrEmpty(caminhoOuUrl))
                return $"/{ImagemPadrao}";

            // Se for URL da internet (http ou https)
            if (Uri.TryCreate(caminhoOuUrl, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                return caminhoOuUrl;
            }

            // Verifica se já está no formato correto
            if (caminhoOuUrl.StartsWith($"/{PastaProdutos}/"))
                return caminhoOuUrl;

            // Remove barras ou caminhos relativos
            var nomeArquivo = caminhoOuUrl.TrimStart('/', '\\');

            return $"/{PastaProdutos}/{nomeArquivo}";
        }
    }
}
