using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace testclient
{
    class Program
    {
        static Filter GetPredicate(){
            var e1 = new Filter(){kind=0,att="Name",value="Daniel Craig"};
            var e2 = new Filter(){kind=0,att="Gender",value="Male"};
            return new Filter(){kind = 1, a1 = e1, a2 = e2};
        }

        static async Task GetActors(){
            var client = new HttpClient {BaseAddress = new Uri("http://localhost:5000")};
            var jsonParse = JsonConvert.SerializeObject(GetPredicate(), new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.None});
            var jsonString = new StringContent(jsonParse, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/actor/GetActorFiltered", jsonString);
            if(response.IsSuccessStatusCode){
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
            else{
                Console.WriteLine(response.StatusCode);
            }
        }


        static void Main(string[] args)
        {
            GetActors().Wait();
        }
    }
}
