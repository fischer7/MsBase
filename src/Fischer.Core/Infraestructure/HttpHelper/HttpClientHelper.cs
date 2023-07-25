using Newtonsoft.Json;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace Fischer.Core.Infraestructure.HttpHelper;
public sealed class HttpClientHelper
{

    /// <summary>
    /// Crie uma mensagem GET com conteúdo (body) em Json.
    /// </summary>
    /// <param name="baseUri">Url Base</param>
    /// <param name="relativeUri">Url Relativa</param>
    /// <param name="content">Conteúdo a ser serializado</param>
    /// <returns>Retorna uma mensagem para execução do método: var response = await _httpClient.SendAsync(requestMsg)</returns>
    public static HttpRequestMessage GetWithJsonContent(Uri baseUri, object content, string relativeUri = "")
    {
        return new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = string.IsNullOrEmpty(relativeUri) ? baseUri : new Uri(baseUri, relativeUri),
            Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, MediaTypeNames.Application.Json /* or "application/json" in older versions */),
        };
    }

    /// <summary>
    /// Post frequentemente usado na aplicação.
    /// </summary>
    /// <param name="httpClient">Com o base URL preenchido.</param>
    /// <param name="endpoint">URL Parcial</param>
    /// <param name="request">Seu objeto</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<HttpResponseMessage> PostWithJsonBody(HttpClient httpClient, string endpoint,
        object request, CancellationToken cancellationToken)
    {
        var requisicao = JsonConvert.SerializeObject(request);
        var stringContent = new StringContent(requisicao, Encoding.UTF8, MediaTypeNames.Application.Json);
        return await httpClient.PostAsync(endpoint, stringContent, cancellationToken);
    }

    public static async Task<HttpResponseMessage> PostWithJsonToGzip(HttpClient client, string endpoint, object request, CancellationToken cancellationToken)
    {
        var streamContent = await ObterStreamContent(request);
        return await client.PostAsync(endpoint, streamContent, cancellationToken);
    }

    private static async Task<StreamContent> ObterStreamContent(object conteudo)
    {
        var conteudoSerializado = JsonConvert.SerializeObject(conteudo);
        var conteudoBytes = Encoding.UTF8.GetBytes(conteudoSerializado);
        var memoryStream = new MemoryStream();

        await using (var gzip = new GZipStream(memoryStream, CompressionMode.Compress, true))
        {
            gzip.Write(conteudoBytes, default, conteudoBytes.Length);
        }

        memoryStream.Position = default;
        var streamContent = new StreamContent(memoryStream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/gzip");
        streamContent.Headers.ContentEncoding.Add("gzip");

        return streamContent;
    }
}