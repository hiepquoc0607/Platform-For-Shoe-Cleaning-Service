using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using TP4SCS.Library.Models.Request.OpenAI;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class OpenAIService : IOpenAIService
    {
        private readonly IConfiguration _configuration;
        private const string OpenAIEndpoint = "https://api.openai.com/v1/chat/completions";
        private const string Model = "gpt-4o";

        public OpenAIService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> ValidateFeedbackContentAsync(HttpClient httpClient, string content)
        {
            var prompt = $"Please use all knowledge about Vietnamese of your to analyze if the following feedback is appropriate. Respond with 'Valid' if the content is appropriate, and 'Invalid' if it is inappropriate:\n\n\"{content}\"";

            var completionRequest = new ChatCompletionRequest
            {
                Model = Model,
                MaxTokens = 1000,
                Messages = new List<Message>
                {
                    new Message
                    {
                        Role = "user",
                        Content = prompt
                    }
                }
            };

            using var httpReq = new HttpRequestMessage(HttpMethod.Post, OpenAIEndpoint);
            httpReq.Headers.Add("Authorization", $"Bearer {_configuration["OpenAIKey"]}");

            string requestString = JsonSerializer.Serialize(completionRequest);
            httpReq.Content = new StringContent(requestString, Encoding.UTF8, "application/json");

            using HttpResponseMessage httpResponse = await httpClient.SendAsync(httpReq);

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var completionResponse = JsonSerializer.Deserialize<ChatCompletionResponse>(responseContent);

            var analysisResult = completionResponse?.Choices?.FirstOrDefault()?.Message?.Content?.Trim() ?? string.Empty;

            return string.Equals(analysisResult, "Valid", StringComparison.OrdinalIgnoreCase);
        }
    }
}
