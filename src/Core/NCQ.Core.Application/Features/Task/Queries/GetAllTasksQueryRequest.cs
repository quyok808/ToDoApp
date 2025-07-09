using CoreLibs.Wrappers;
using MediatR;
using NCQ.Infrastructure.Repositories.Tasks.Models.GetAllTasks;
using NCQ.Infrastructure.Repositories.Tasks.Repository;

namespace NCQ.Core.Application.Features.Task.Queries
{
    public class GetAllTasksQueryRequest : IRequest<Response<IEnumerable<GetAllTaskResponseModel>>>
    {
        public class QueryHandler : IRequestHandler<GetAllTasksQueryRequest, Response<IEnumerable<GetAllTaskResponseModel>>>
        {
            private readonly ITaskRepository taskRepository;

            public QueryHandler(ITaskRepository taskRepository)
            {
                this.taskRepository = taskRepository;
            }

            public async Task<Response<IEnumerable<GetAllTaskResponseModel>>> Handle(GetAllTasksQueryRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await taskRepository.GetAllTasks(cancellationToken).ConfigureAwait(false);
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
