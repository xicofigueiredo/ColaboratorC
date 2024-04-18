using Microsoft.AspNetCore.Mvc;

using Application.Services;
using Application.DTO;
using Domain.Factory;
using RabbitMQ.Client;
using Newtonsoft.Json;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboratorController : ControllerBase
    {   
        private readonly ColaboratorService _colaboratorService;
        private readonly ColaboratorPublisher _colaboratorPublisher;

        List<string> _errorMessages = new List<string>();

        public ColaboratorController(ColaboratorService colaboratorService, ColaboratorPublisher colaboratorPublisher)
        {
            _colaboratorService = colaboratorService;
            _colaboratorPublisher = colaboratorPublisher;
        }

        // PUT: api/Colaborator/a@bc
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{email}")]
        public async Task<IActionResult> PutColaborator(string email, ColaboratorDTO colaboratorDTO)
        {
            if (email != colaboratorDTO.Email)
            {
                return BadRequest();
            }

            bool wasUpdated = await _colaboratorService.Update(email, colaboratorDTO, _errorMessages);

            if (!wasUpdated /* && _errorMessages.Any() */)
            {
                return BadRequest(_errorMessages);
            }

            return Ok();
        }

        // POST: api/Colaborator
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ColaboratorDTO>> PostColaborator(ColaboratorDTO colaboratorDTO)
        {
            ColaboratorDTO colaboratorResultDTO = await _colaboratorService.Add(colaboratorDTO, _errorMessages);

            if(colaboratorResultDTO != null){
                
                string objectAsString = JsonConvert.SerializeObject(colaboratorResultDTO);
                _colaboratorPublisher.PublishMessage(objectAsString);
                

                return Ok(colaboratorResultDTO);
            }else{
                return BadRequest(_errorMessages);
            }
        }

        // // DELETE: api/Colaborator/5
        // [HttpDelete("{email}")]
        // public async Task<IActionResult> DeleteColaborator(string email)
        // {
        //     var colaborator = await _context.Colaboradores.FindAsync(email);
        //     if (colaborator == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Colaboradores.Remove(colaborator);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

    }
}
