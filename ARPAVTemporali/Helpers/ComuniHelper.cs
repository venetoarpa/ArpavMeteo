using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using ARPAVTemporali.Models;
using Newtonsoft.Json;

namespace ARPAVTemporali.Helpers
{
    public class ComuniHelper
    {
        public ComuniHelper()
        {
        }

        public static async Task<List<Comune>> Fetch()
        {
            List<Comune> comuni = new List<Comune>();

            string URL = $"{Variables.ApplicationServerURL}/app/comuni";
            UriBuilder baseUri = new UriBuilder(URL);

            var _client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, baseUri.Uri);

            try
            {

                var response = await _client.SendAsync(request); //assicurarsi di abilitare il permesso a usare internet in android->options->android application->required permissions

                response.EnsureSuccessStatusCode();
                HttpContent httpContent = response.Content;
                string json = await httpContent.ReadAsStringAsync();
                comuni = JsonConvert.DeserializeObject<List<Comune>>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return comuni;
        }
    }
}
