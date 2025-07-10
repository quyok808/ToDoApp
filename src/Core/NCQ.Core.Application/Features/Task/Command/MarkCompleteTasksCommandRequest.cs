using CoreLibs.Wrappers;
using MediatR;
using NCQ.Core.Application.GlobalConfig;
using NCQ.Infrastructure.Repositories.Tasks.Models.MarkCompleteTasks;
using NCQ.Infrastructure.Repositories.Tasks.Repository;

namespace NCQ.Core.Application.Features.Task.Command
{
    public class MarkCompleteTasksCommandRequest : MarkCompletedTasksRequestModel, IRequest<Response<MarkCompletedTasksResponseModel>>
    {
        public class Handler : IRequestHandler<MarkCompleteTasksCommandRequest, Response<MarkCompletedTasksResponseModel>>
        {
            private readonly ITaskRepository taskRepository;

            public Handler(ITaskRepository taskRepository)
            {
                this.taskRepository = taskRepository;
            }

            public async Task<Response<MarkCompletedTasksResponseModel>> Handle(MarkCompleteTasksCommandRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await taskRepository.MarkCompletedTasksAsync(request, cancellationToken);
                    return new Response<MarkCompletedTasksResponseModel>(result);
                }
                catch (Exception ex)
                {
                    return new Response<MarkCompletedTasksResponseModel>(AppMessageResponse.ExceptionMessageResponse);
                }
            }
        }
    }
}
