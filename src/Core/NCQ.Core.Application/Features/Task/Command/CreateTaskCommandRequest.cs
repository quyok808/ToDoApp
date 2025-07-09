using CoreLibs.Wrappers;
using MediatR;
using NCQ.Infrastructure.Repositories.Tasks.Models.CreateTask;
using NCQ.Infrastructure.Repositories.Tasks.Repository;

namespace NCQ.Core.Application.Features.Task.Command
{
    public class CreateTaskCommandRequest : CreateTaskRequestModel, IRequest<Response<CreateTaskResponseModel>>
    {
        public class Handler : IRequestHandler<CreateTaskCommandRequest, Response<CreateTaskResponseModel>>
        {
            private readonly ITaskRepository taskRepository;

            public Handler(ITaskRepository taskRepository)
            {
                this.taskRepository = taskRepository;
            }

            public async Task<Response<CreateTaskResponseModel>> Handle(CreateTaskCommandRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await taskRepository.CreateTask(request, cancellationToken);
                    return new Response<CreateTaskResponseModel>(result);
                }
                catch (Exception ex)
                {
                    return new Response<CreateTaskResponseModel>();
                }
            }
        }
    }
}
