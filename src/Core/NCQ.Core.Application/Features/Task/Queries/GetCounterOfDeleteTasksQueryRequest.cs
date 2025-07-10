using CoreLibs.Wrappers;
using MediatR;
using NCQ.Core.Application.GlobalConfig;
using NCQ.Infrastructure.Repositories.Tasks.Models.GetCounterDeleteTaskTemporary;
using NCQ.Infrastructure.Repositories.Tasks.Repository;

namespace NCQ.Core.Application.Features.Task.Queries
{
    public class GetCounterOfDeleteTasksQueryRequest : IRequest<Response<GetCounterDeleteTaskTemporaryResponseModel>>
    {
        public class QueryHandler : IRequestHandler<GetCounterOfDeleteTasksQueryRequest, Response<GetCounterDeleteTaskTemporaryResponseModel>>
        {
            private readonly ITaskRepository taskRepository;

            public QueryHandler(ITaskRepository taskRepository)
            {
                this.taskRepository = taskRepository;
            }

            public async Task<Response<GetCounterDeleteTaskTemporaryResponseModel>> Handle(GetCounterOfDeleteTasksQueryRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await taskRepository.GetCounterDeleteTaskTemporaryAsync(cancellationToken);
                    return new Response<GetCounterDeleteTaskTemporaryResponseModel>(result);
                }
                catch (Exception ex)
                {
                    return new Response<GetCounterDeleteTaskTemporaryResponseModel>(AppMessageResponse.ExceptionMessageResponse);
                }
            }
        }
    }
}
