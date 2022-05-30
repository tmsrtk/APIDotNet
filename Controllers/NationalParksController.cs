using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using APIDotNet.Entities;
using APIDotNet.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace APIDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(404)]
    public class NationalParksController : Controller
    {
        private readonly ILogger<NationalParksController> _logger;

        private INationalParkRepository _npRepository;
        private readonly IMapper _mapper;

        public NationalParksController(ILogger<NationalParksController> logger, INationalParkRepository repo, IMapper mapper)
        {
            _logger = logger;
            _npRepository = repo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetNationalParks()
        {
            var nationalParks = _npRepository.GetNationalParks();
            var nationalParkDto = new List<NationalParkDto>();
            foreach (var nationalPark in nationalParks)
            {
                nationalParkDto.Add(_mapper.Map<NationalParkDto>(nationalPark));
            }
            return Ok(nationalParkDto);
        }

        [HttpGet("{id:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(400)]
        public IActionResult GetNationalPark(int id)
        {
            var nationalPark = _npRepository.GetNationalPark(id);
            var nationalParkDto = new NationalParkDto();
            if (nationalPark == null)
            {
                return NotFound();
            }
            else
            {
                nationalParkDto = _mapper.Map<NationalParkDto>(nationalPark);
                return Ok(nationalParkDto);
            }
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_npRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park already exists");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nationalPark = new NationalPark();
            nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
            bool success = _npRepository.CreateNationalPark(nationalPark);
            if (!success)
            {
                ModelState.AddModelError("", $"Something went wrong when adding national park {nationalParkDto.Name}");
                return StatusCode(500, ModelState);

            }
            return CreatedAtRoute("GetNationalPark", new { id = nationalPark.Id }, nationalPark);
        }

        [HttpPatch("{Id:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalParkDto.Id != nationalParkId)
            {
                return BadRequest(ModelState);
            }
            var nationalPark = new NationalPark();
            nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_npRepository.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong when upddating national park {nationalParkDto.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int id)
        {
            if (!_npRepository.NationalParkExists(id))
            {
                return NotFound();
            }
            var nationalPark = _npRepository.GetNationalPark(id);
            if (!_npRepository.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting national park {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}