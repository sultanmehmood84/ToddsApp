using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text.Json;
using ToddsAPI.Models;

namespace ToddsApp.Pages
{  

    public class IndexModel : PageModel
    {   
        public IEnumerable<Sample> Samples { get; set; }       

        public IndexModel()
        {
            
        }              

        public async Task OnGetAsync()
        {
            Samples = await GetDataFromApiAsync();
        }

        public async Task<List<Sample>> GetDataFromApiAsync()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:5190/api/getdata");
            response.EnsureSuccessStatusCode(); // Ensure successful response

            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var content = await response.Content.ReadAsStringAsync();
            var samples = JsonConvert.DeserializeObject<List<Sample>>(content, jsonSettings);

            return samples;
        }
    }
}