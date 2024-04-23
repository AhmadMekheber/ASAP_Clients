using ASAP_Clients.Dto;
using ASAP_Clients.Entities;
using ASAP_Clients.Exceptions;
using ASAP_Clients.Manager.IManager;
using Microsoft.AspNetCore.Mvc;

namespace ASAP_Clients.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientManager _clientManager;

        public ClientController(IClientManager clientManager)
        {
            _clientManager = clientManager;
        }

        [HttpGet]
        public async Task<ActionResult<ClientDto>> GetAll() 
        {
            return Ok(await _clientManager.GetAll());
        }

        [HttpGet("{clientID}", Name = "GetByID")]
        public async Task<ActionResult<ClientDto>> GetByID(long clientID) 
        {
            ClientDto? clientDto = await _clientManager.GetByID(clientID);

            if (clientDto == null)
            {
                return NotFound();
            }

            return Ok(clientDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] UpdateClientDto updateClientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try 
            {
                ClientDto clientDto = await _clientManager.CreateClient(updateClientDto);

                return CreatedAtRoute("GetByID", new { clientID = clientDto.ID}, clientDto);
            }
            catch (DuplicateEmailException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                //Should Log Exception and the stacktrace important info.

                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPut("{clientID}")]
        public async Task<IActionResult> UpdateClient(long clientID, [FromBody] UpdateClientDto updateClientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _clientManager.UpdateClient(clientID, updateClientDto);

                return NoContent();
            }
            catch (DuplicateEmailException ex)
            {
                return Conflict(ex.Message);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                //Should Log Exception and the stacktrace important info.

                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpDelete("{clientID}")]
        public async Task<IActionResult> DeleteClient(long clientID)
        {
            try
            {
                await _clientManager.DeleteClient(clientID);

                return NoContent();
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                //Should Log Exception and the stacktrace important info.
                
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}