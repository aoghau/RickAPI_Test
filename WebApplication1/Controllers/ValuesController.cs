using Microsoft.AspNetCore.Mvc;
using DataAccess;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        
        public List<Character> Characters { get; set; }
        public List<Episode> Episodes { get; set; }        
        
        /// <summary>
        /// GET method that returns info about a Character
        /// </summary>
        /// <param name="person"></param>
        /// <returns>If such a Character exists, returns a Person based on the Character, otherwise - 404</returns>
        [HttpGet("{person}")]
        public async Task<IActionResult> Person(string person)
        {
            DataProcessing processing= new DataProcessing();
            if (await processing.GetPersonAsync(person) == null)
                return StatusCode(404);
            return new JsonResult(await processing.GetPersonAsync(person));
        }

        /// <summary>
        /// POST method that checks whether a Character was participating in an Episode
        /// </summary>
        /// <param name="personName"></param>
        /// <param name="episodeName"></param>
        /// <returns>true if a Character was in an Episode, false if they were not, and 404 if such a Character or Episode is not found</returns>
        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> CheckPerson(string personName, string episodeName)
        {
            DataProcessing processing = new DataProcessing();
            Episodes = await processing.GetEpisodesAsync();
            Characters = await processing.GetCharactersAsync();                       
            Character person = Characters.Where(x => x.name == personName).FirstOrDefault();
            Episode episode = Episodes.Where(x => x.name == episodeName).FirstOrDefault();            
            if (person != null && episode != null)
            {
                List<Character> episodeCharacters = await processing.GetCharactersListAsync(episode.characters);
                if (episodeCharacters.Where(x => x.name == personName).FirstOrDefault() != null)
                {
                    return new ObjectResult("true");
                }
                return new ObjectResult("false");
            }
            return StatusCode(404);
        }
        

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }        

    }    
}
