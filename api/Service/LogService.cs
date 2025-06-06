using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using StargateAPI.Controllers;
using StargateAPI.Interface;
using System.Net;
using System.Xml.Linq;

namespace StargateAPI.Service
{
    public class LogService : ILogService
    {
        private readonly IMediator _mediator;
        public LogService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<int> Log(string message)
        {
            try
            {
                var result = await _mediator.Send(new CreateLog()
                {
                    Message = message
                });

                return result.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }
    }
}
