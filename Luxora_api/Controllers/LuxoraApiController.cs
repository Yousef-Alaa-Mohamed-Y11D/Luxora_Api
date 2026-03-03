using Luxora_api.Data;
using Luxora_api.Logging;
using Luxora_api.Models;
using Luxora_api.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Luxora_api.Controllers
{
    [Route("api/LuxoraApi")]
    //[ApiController]
    public class LuxoraApiController : ControllerBase
    {
        private readonly ILogging _logger;
        public LuxoraApiController(ILogging logging)
        {
            _logger = logging;
        }
        [HttpGet]
        public ActionResult GetVillas()
        {
            _logger.log("Getting all Villas",LogType.Info);
            IEnumerable<VillaDTO> Villas =  VillaStore.villaList;
            return Ok(Villas); // status code : 200
        }
        [HttpGet("{id:int}" , Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)    
            {
                _logger.log ("Get villa Error with Id" + id ,LogType.Error);
                return BadRequest(); // status code : 400
            }
           
            VillaDTO? Villa = VillaStore.villaList.FindLast(x => x.Id == id);
            if (Villa == null)
            {
                _logger.log("Villa is Null", LogType.Warning);
                return NotFound(); // status code : 404
            }
                return Ok(Villa); // 200
            
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //CustomError
            if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already Exists!");
                return BadRequest(ModelState);
            }
            
            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            if (villaDTO.Id > 0)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDTO);
            return CreatedAtRoute("GetVilla",new {id = villaDTO.Id}, villaDTO);

        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}" , Name = "DeleteVilla")]       
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            VillaStore.villaList.Remove(villa);
            return NoContent();

        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            villa.Name = villaDTO.Name;
            villa.Occupancy = villaDTO.Occupancy;
            villa.Sqft = villaDTO.Sqft;
            return NoContent();

        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if(patchDTO ==null || id ==0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id ==id);
            if(villa == null)
            {
                return NotFound();
            }
            patchDTO.ApplyTo(villa,ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

        }


    }
}

