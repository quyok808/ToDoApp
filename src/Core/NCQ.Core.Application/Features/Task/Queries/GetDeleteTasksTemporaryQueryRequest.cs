using MediatR;
using NCQ.Infrastructure.Repositories.Tasks.Models.GetAllTasks;
using NCQ.Infrastructure.Repositories.Tasks.Repository;
using CoreLibs.Wrappers;

namespace NCQ.Core.Application.Features.Task.Queries
{
    public class GetDeleteTasksTemporaryQueryRequest : IRequest<Response<IEnumerable<GetAllTaskResponseModel>>>
    {
        public class QueryHandler : IRequestHandler<GetDeleteTasksTemporaryQueryRequest, Response<IEnumerable<GetAllTaskResponseModel>>>
        {
            private readonly ITaskRepository taskRepository;

            public QueryHandler(ITaskRepository taskRepository)
            {
                this.taskRepository = taskRepository;
            }

            public async Task<Response<IEnumerable<GetAllTaskResponseModel>>> Handle(GetDeleteTasksTemporaryQueryRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await taskRepository.GetAllDeleteTasksTemporaryAsync(cancellationToken).ConfigureAwait(false);
                    return new Response<IEnumerable<GetAllTaskResponseModel>>(result);
                }
                catch (Exception ex)
                {
                    return new Response<IEnumerable<GetAllTaskResponseModel>> { Succeeded = false, Message = "Đã có lỗi xảy ra", Errors = ex, Data = new List<GetAllTaskResponseModel>() };
                }
            }
        }
    }
}
