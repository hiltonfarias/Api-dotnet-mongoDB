using ApiDotNet.Data.Collections;
using ApiDotNet.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
namespace ApiDotNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectedController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infected> _infectedCollection;

        public InfectedController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectedCollection = _mongoDB.mongoDatabase.GetCollection<Infected>(typeof(Infected).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SaveInfected([FromBody] InfectedDTO infectedDTO)
        {
            var infected = new Infected(
                infectedDTO.BirthDate, 
                infectedDTO.Gender, 
                infectedDTO.Latitude, 
                infectedDTO.Longitude
            );
            _infectedCollection.InsertOne(infected);
            return StatusCode(201, "Infected successfully added");
        }

        [HttpGet]
        public ActionResult GetInfected()
        {
            var infected = _infectedCollection.Find(Builders<Infected>.Filter.Empty).ToList();
            return Ok(infected);
        }

        [HttpPut]
        public ActionResult UpdateInfected([FromBody] InfectedDTO infectedDTO)
        {
            var infected = new Infected(
                infectedDTO.BirthDate, 
                infectedDTO.Gender, 
                infectedDTO.Latitude, 
                infectedDTO.Longitude
            );
            _infectedCollection.UpdateOne(
                Builders<Infected>.Filter.Where(_=> _.BirthDate == infectedDTO.BirthDate),
                Builders<Infected>.Update.Set("gender", infectedDTO.Gender)
            );
            
            return Ok("Successfully updated");
        }

        [HttpDelete]
        public ActionResult DeleteInfected([FromBody] InfectedDTO infectedDTO)
        {
            _infectedCollection.DeleteOne(
                Builders<Infected>.Filter.Where(_=> _.BirthDate == infectedDTO.BirthDate)
            );
            
            return Ok("Successfully deleted");
        }
    }
}