using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataAccess;
using Newtonsoft.Json;

namespace WebApplication1
{
    public class DataProcessing
    {
        /// <summary>
        /// Gets a list of all Characters from the API
        /// </summary>
        /// <returns>List of Characters</returns>
        public async Task<List<Character>> GetCharactersAsync()
        {
            var client = new HttpClient();
            var request = await client.GetStringAsync("https://rickandmortyapi.com/api/character");
            Wrapper<Character> characters = JsonConvert.DeserializeObject<Wrapper<Character>>(request);
            List<Character> result = new List<Character>();
            result.AddRange(characters.results);
            while(characters.info.next != null)
            {
                request = await client.GetStringAsync(characters.info.next);
                Wrapper<Character> nextCharacters = JsonConvert.DeserializeObject<Wrapper<Character>>(request);
                result.AddRange(nextCharacters.results);
                characters.info.next = nextCharacters.info.next;
            }
            return result;
        }

        /// <summary>
        /// Gets info about a character using a uri to communicate with API
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>A Character</returns>
        public async Task<Character> GetCharacterAsync(string uri)
        {
            var client = new HttpClient();
            var request = await client.GetStringAsync(uri);
            Character character = JsonConvert.DeserializeObject<Character>(request);
            return character;
        }

        /// <summary>
        /// Gets a list of Characters using given list of uris from the API
        /// </summary>
        /// <param name="uris"></param>
        /// <returns>A List of Characters</returns>
        public async Task<List<Character>> GetCharactersListAsync(List<string> uris)
        {
            var client = new HttpClient();
            List<Character> characters = new List<Character>();            
            for(int i = 0; i < uris.Count; i++)
            {
               characters.Add(await GetCharacterAsync(uris[i]));
            }
            return characters;
        }

        /// <summary>
        /// Gets info about all Episodes from the API
        /// </summary>
        /// <returns>A List of Episodes</returns>
        public async Task<List<Episode>> GetEpisodesAsync()
        {
            var client = new HttpClient();
            var request = await client.GetStringAsync("https://rickandmortyapi.com/api/episode");
            Wrapper<Episode> episodes = JsonConvert.DeserializeObject<Wrapper<Episode>>(request);
            List<Episode> result = new List<Episode>();            
            result.AddRange(episodes.results);
            while (episodes.info.next != null)
            {
                request = await client.GetStringAsync(episodes.info.next);
                Wrapper<Episode> nextEpisodes = JsonConvert.DeserializeObject<Wrapper<Episode>>(request);
                result.AddRange(nextEpisodes.results);
                episodes.info.next = nextEpisodes.info.next;
            }
                return episodes.results;
        }

        /// <summary>
        /// Gets a Character and makes it into a Person
        /// </summary>
        /// <param name="name"></param>
        /// <returns>A Person</returns>
        public async Task<Person> GetPersonAsync(string name)
        {
            List<Character> characters = await GetCharactersAsync();
            Character character = characters.Where(x => x.name == name).FirstOrDefault();
            if (character == null)
                return null;
            if (character.origin.name == "unknown")
                return new Person(character, null);
            var client = new HttpClient();
            var request = await client.GetStringAsync(character.origin.url);
            Place place = JsonConvert.DeserializeObject<Place>(request);
            PersonOrigin personOrigin = new PersonOrigin(place);
            Person person = new Person(character, personOrigin);
            return person;
        }

        public DataProcessing() { }
    }

    //This class is made for storing the info that comes when querying the target API
    public class Info
    {
        public int count { get; set; }
        public int pages { get; set; }
        public string next { get; set; }
        public object prev { get; set; }
    }

    //A wrapper class that is needed to deserialize API response
    public class Wrapper<T>
    {
        public Info info { get; set; }
        public List<T> results { get; set; }
    }
}
