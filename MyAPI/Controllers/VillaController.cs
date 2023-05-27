using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAPI.Data;
using MyAPI.Logging;
using MyAPI.Models;
using MyAPI.Models.Dto;

namespace MyAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //the APIController allows to process the validations placed in Models
    public class VillaController : ControllerBase
    {

        //using logger to log in console or file
        //private readonly ILogger<VillaController> _logger;

        //public VillaController(ILogger<VillaController> logger)
        //{
        //    _logger = logger;
        //}
        //private readonly ILogging _logger;
        //public VillaController(ILogging logger)
        //{
        //    _logger = logger;
        //}

        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas() 
        {
            //_logger.LogInformation("Getting all villas"); // this will log in console
            //_logger.Log("Getting all villas","");
            //return Ok(VillaStore.villalist);
            return Ok(_db.Villas);
        }


        [HttpGet("Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status404NotFound), Type= typeof(VillaDto)]
        public ActionResult GetVilla(int id)
        {
            //var villa = VillaStore.villalist.FirstOrDefault(n => n.Id == id);
            var villa= _db.Villas.ToList().FirstOrDefault(n=> n.Id==id);
            if(villa == null)
            {
                //_logger.Log("getting error -"+id, "error");
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public ActionResult<VillaDto> SetVilla([FromBody]VillaDto villa)
        {
            //if(!ModelState.IsValid) //this can be used to validate if we are not uising APIController
            //{
            //    return BadRequest();
            //}
            if(_db.Villas.FirstOrDefault(n=>n.Name.ToLower() == villa.Name.ToLower())!= null)
            {
                ModelState.AddModelError("Name", "Villa already exists");
                return BadRequest(ModelState);
            }
            if (villa == null)
            {
                return BadRequest(villa);
            }
            if (villa.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Villa model = new()
            {
                Name = villa.Name,
                Id = villa.Id,
                Description = villa.Description,
                Rate = villa.Rate,
                ImageUrl = villa.ImageUrl,
                PinCode = villa.PinCode

            };
            _db.Villas.Add(model);
            _db.SaveChanges();
            //return Ok(villa);
            return CreatedAtRoute(GetVilla(villa.Id),villa);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult Delete(int id)
        {
            var villa= _db.Villas.FirstOrDefault(n=>n.Id==id);
            if(villa != null)
            {
                _db.Villas.Remove(villa);
                _db.SaveChanges();
                return NoContent();
            }
            else
            {
                ModelState.AddModelError("", "Villa not found");
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDto villadto) 
        {
            if( id==null || id != villadto.Id)
            {
                return BadRequest(ModelState);
            }
            //var villa = _db.Villas.FirstOrDefault(n => n.Id == id);
            //villa.Name= villadto.Name;
            //villa.Description= villadto.Description;

            Villa model = new()
            {
                Name = villadto.Name,
                Id = villadto.Id,
                Description = villadto.Description,
                Rate = villadto.Rate,
                ImageUrl = villadto.ImageUrl,
                PinCode = villadto.PinCode

            };
            _db.Villas.Update(model);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult EditVilla(int id, JsonPatchDocument<VillaDto> patchvilladto)
        {
            if (id == null || id == 0)
            {
                return BadRequest(ModelState);
            }
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(n => n.Id == id);
            if (villa == null)
            {
                return BadRequest(ModelState);
            }
            VillaDto villaDto = new()
            {
                Name = villa.Name,
                Id = villa.Id,
                Description = villa.Description,
                Rate = villa.Rate,
                ImageUrl = villa.ImageUrl,
                PinCode = villa.PinCode

            };


            patchvilladto.ApplyTo(villaDto, ModelState);
            Villa model = new()
            {
                Name = villaDto.Name,
                Id = villaDto.Id,
                Description = villaDto.Description,
                Rate = villaDto.Rate,
                ImageUrl = villaDto.ImageUrl,
                PinCode = villaDto.PinCode

            };
            _db.Villas.Update(model);
            _db.SaveChanges();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }

}
