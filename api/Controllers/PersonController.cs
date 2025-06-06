using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using StargateAPI.Interface;
using StargateAPI.Service;
using System.Net;

namespace StargateAPI.Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;
        private ILogService _logService;
        public PersonController(IMediator mediator, ILogService logService)
        {
            _mediator = mediator;
            _logService = logService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPeople()
        {
            try
            {
                var result = await _mediator.Send(new GetPeople()
                {

                });
                await _logService.Log("Getting list of all people...");
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
                await _logService.Log(ex.ToString());
                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetPersonByName(string name)
        {
            try
            {
                var result = await _mediator.Send(new GetPersonByName()
                {
                    Name = name
                });

                await _logService.Log($"Fetching info for {name}...");
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                await _logService.Log(ex.ToString());
                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> CreatePerson([FromBody] CreatePerson person)
        {
            try
            {
                var result = await _mediator.Send(new CreatePerson()
                {
                    Name = person.Name
                });
                await _logService.Log($"Creating record for {person.Name}...");
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                await _logService.Log(ex.ToString());
                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }

        }
    }
}