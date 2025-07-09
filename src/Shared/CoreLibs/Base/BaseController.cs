using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoreLibs.Base
{
    [ApiController]
    [Produces("application/json", new string[] { })]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator mediator;

        protected BaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}
