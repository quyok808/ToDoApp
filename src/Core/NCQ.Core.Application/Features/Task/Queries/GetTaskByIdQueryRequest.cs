using CoreLibs.Wrappers;
using MediatR;
using NCQ.Core.Application.GlobalConfig;
using NCQ.Infrastructure.Repositories.Tasks.Models.GetTaskById;
using NCQ.Infrastructure.Repositories.Tasks.Repository;

namespace NCQ.Core.Application.Features.Task.Queries
{
    public class GetTaskByIdQueryRequest : GetTaskByIdRequestModel, IRequest<Response<GetTaskByIdResponseModel>>
    {
        public class QueryHandler : IRequestHandler<GetTaskByIdQueryRequest, Response<GetTaskByIdResponseModel>>
        {
            private readonly ITaskRepository taskRepository;

            public QueryHandler(ITaskRepository taskRepository)
            {
                this.taskRepository = taskRepository;
            }

            public async Task<Response<GetTaskByIdResponseModel>> Handle(GetTaskByIdQueryRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await taskRepository.GetTaskById(request, cancellationToken).ConfigureAwait(false);
                    return new Response<GetTaskByIdResponseModel>(result);
                }
                catch (Exception ex)
                {
                    return new Response<GetTaskByIdResponseModel>(AppMessageResponse.ExceptionMessageResponse);
                }
            }
        }
    }
}
