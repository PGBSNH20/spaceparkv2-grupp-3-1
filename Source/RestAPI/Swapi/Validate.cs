using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace RestAPI.Swapi
{
    public class Validate
    {
        public static async Task<bool> Person(string input)
        {
            var client = new RestClient("https://swapi.dev/api/");
            var request = new RestRequest($"people/?search={input}", DataFormat.Json); // Use the swAPI search function with the user input string.
            var peopleResponse = await client.GetAsync<PeopleResponse>(request); // Add the results from the search to the PeopleResponse List
            var person = peopleResponse.Results.Find(p => p.Name.ToLower() == input.ToLower());
            return person != null;
        }   
        public static async Task<Starship> Starship(string input)
        {
            var client = new RestClient("https://swapi.dev/api/");
            var request = new RestRequest($"starships/?search={input}", DataFormat.Json); // Use the swAPI search function with the user input string.
            var starshipResponse = await client.GetAsync<StarshipResponse>(request); // Add the results from the search to the PeopleResponse List
            var starship = starshipResponse.Results.Find(p => p.Name.ToLower() == input.ToLower());
            return starship;
        }
    }
}
