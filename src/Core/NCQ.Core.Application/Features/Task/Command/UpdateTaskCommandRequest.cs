using CoreLibs.Wrappers;
using MediatR;
using NCQ.Infrastructure.Repositories.Tasks.Models.UpdateTask;
using NCQ.Infrastructure.Repositories.Tasks.Repository;

namespace NCQ.Core.Application.Features.Task.Command
{
    public class UpdateTaskCommandRequest : UpdateTaskRequestModel, IRequest<Response<UpdateTaskResponseModel>>
    {
        public class Handler : IRequestHandler<UpdateTaskCommandRequest, Response<UpdateTaskResponseModel>>
        {
            private readonly ITaskRepository taskRepository;

            public Handler(ITaskRepository taskRepository)
            {
                this.taskRepository = taskRepository;
            }

            public async Task<Response<UpdateTaskResponseModel>> Handle(UpdateTaskCommandRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await taskRepository.UpdateTask(request.ID, request, cancellationToken).ConfigureAwait(false);
                    return new Response<UpdateTaskResponseModel>(result);
                }
                catch (Exception ex)
                {
                    return new Response<UpdateTaskResponseModel>();
                }
            }
        }
    }
}
