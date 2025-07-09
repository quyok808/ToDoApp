using CoreLibs.Wrappers;
using MediatR;
using NCQ.Core.Application.GlobalConfig;
using NCQ.Infrastructure.Repositories.Tasks.Models.DeleteTask;
using NCQ.Infrastructure.Repositories.Tasks.Repository;

namespace NCQ.Core.Application.Features.Task.Command
{
    public class DeleteManyTasksCommandRequest : DeleteManyTasksRequestModel, IRequest<Response<DeleteTaskResponseModel>>
    {
        public class Handler : IRequestHandler<DeleteManyTasksCommandRequest, Response<DeleteTaskResponseModel>>
        {
            private readonly ITaskRepository taskRepository;

            public Handler(ITaskRepository taskRepository)
            {
                this.taskRepository = taskRepository;
            }

            public async Task<Response<DeleteTaskResponseModel>> Handle(DeleteManyTasksCommandRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await taskRepository.DeleteManyTasksAsync(request, cancellationToken).ConfigureAwait(false);
                    return new Response<DeleteTaskResponseModel>(result);
                }
                catch (Exception ex)
                {  
                    return new Response<DeleteTaskResponseModel>(AppMessageResponse.ExceptionMessageResponse);
                }
            }
        }
    }
}
