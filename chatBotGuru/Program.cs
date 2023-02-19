using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenAIDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Enter a prompt: ");
            string prompt = Console.ReadLine();

            string generatedText = await GenerateTextAsync(prompt);
            if (generatedText != null)
            {
                Console.WriteLine("Generated text: " + generatedText);
            }
            Console.ReadLine();
        }

        static async Task<string> GenerateTextAsync(string prompt)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.openai.com/v1/engines/davinci-codex/completions"),
                Headers =
                {
                    { "Authorization", "Bearer YOUR_API_KEY" },
                },
                Content = new StringContent(
                    JsonConvert.SerializeObject(
                        new
                        {
                            prompt,
                            max_tokens = 60,
                            n = 1,
                            stop = "\n"
                        }),
                    System.Text.Encoding.UTF8,
                    "application/json")
            };

            try
            {
                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(responseContent);
                string generatedText = data.choices[0].text;

                return generatedText;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
    }
}
