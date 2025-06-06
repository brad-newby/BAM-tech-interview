using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Commands
{
    public class CreateLog : IRequest<CreateLogResult>
    {
        public required string Message { get; set; } = string.Empty;
    }

    public class CreateLogHandler : IRequestHandler<CreateLog, CreateLogResult>
    {
        private readonly StargateContext _context;

        public CreateLogHandler(StargateContext context)
        {
            _context = context;
        }
        public async Task<CreateLogResult> Handle(CreateLog request, CancellationToken cancellationToken)
        {

            var newLog = new Logs()
            {
                Message = request.Message,
                MessageDate = DateTime.UtcNow,
            };

                await _context.Logs.AddAsync(newLog);

                await _context.SaveChangesAsync();

                return new CreateLogResult()
                {
                    Id = newLog.Id
                };
          
        }
    }

    public class CreateLogResult : BaseResponse
    {
        public int Id { get; set; }
    }
}
