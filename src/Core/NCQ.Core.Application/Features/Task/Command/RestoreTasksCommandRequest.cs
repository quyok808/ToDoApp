using CoreLibs.Wrappers;
using MediatR;
using NCQ.Infrastructure.Repositories.Tasks.Models.RestoreTask;
using NCQ.Infrastructure.Repositories.Tasks.Repository;

namespace NCQ.Core.Application.Features.Task.Command
{
    public class RestoreTasksCommandRequest : RestoreTasksRequestModel, IRequest<Response<RestoreTasksResponseModel>>
    {
        public class Handler : IRequestHandler<RestoreTasksCommandRequest, Response<RestoreTasksResponseModel>>
        {
            private readonly ITaskRepository taskRepository;

            public Handler(ITaskRepository taskRepository)
            {
                this.taskRepository = taskRepository;
            }

            public async Task<Response<RestoreTasksResponseModel>> Handle(RestoreTasksCommandRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await taskRepository.RestoreTaskAsync(request, cancellationToken).ConfigureAwait(false);
                    return new Response<RestoreTasksResponseModel>(result);
                }
                catch (Exception ex)
                {
                    return new Response<RestoreTasksResponseModel>();
                }
            }
        }
    }
}
