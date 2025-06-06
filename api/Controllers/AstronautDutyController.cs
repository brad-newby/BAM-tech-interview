using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using StargateAPI.Interface;
using System.Net;

namespace StargateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AstronautDutyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private ILogService _logService;
        public AstronautDutyController(IMediator mediator, ILogService logService)
        {
            _mediator = mediator;
            _logService = logService;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAstronautDutiesByName(string name)
        {
            try
            {
                var result = await _mediator.Send(new GetAstronautDutiesByName()
                {
                    Name = name.ToLower()
                });

                await _logService.Log($"Getting {name} duty history...");
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
        public async Task<IActionResult> CreateAstronautDuty([FromBody] CreateAstronautDuty request)
        {
            var result = await _mediator.Send(request);
            await _logService.Log($"Creating new duty entry for {request.Name}...");
            return this.GetResponse(result);           
        }
    }
}