using NCQ.Infrastructure.Repositories.Tasks.Models.CreateTask;
using NCQ.Infrastructure.Repositories.Tasks.Models.DeleteTask;
using NCQ.Infrastructure.Repositories.Tasks.Models.GetAllTasks;
using NCQ.Infrastructure.Repositories.Tasks.Models.GetCounterDeleteTaskTemporary;
using NCQ.Infrastructure.Repositories.Tasks.Models.GetTaskById;
using NCQ.Infrastructure.Repositories.Tasks.Models.RestoreTask;
using NCQ.Infrastructure.Repositories.Tasks.Models.UpdateTask;

namespace NCQ.Infrastructure.Repositories.Tasks.Repository
{
    public interface ITaskRepository
    {
        Task<IEnumerable<GetAllTaskResponseModel>> GetAllTasks(CancellationToken cancellationToken = default);
        Task<IEnumerable<GetAllTaskResponseModel>> GetAllDeleteTasksTemporaryAsync(CancellationToken cancellationToken = default);
        Task<GetTaskByIdResponseModel> GetTaskById(GetTaskByIdRequestModel request, CancellationToken cancellationToken = default);
        Task<CreateTaskResponseModel> CreateTask(CreateTaskRequestModel request, CancellationToken cancellationToken = default);
        Task<UpdateTaskResponseModel> UpdateTask(int Id, UpdateTaskRequestModel request, CancellationToken cancellationToken = default);
        Task<DeleteTaskResponseModel> DeleteTask(int Id, CancellationToken cancellationToken = default);
        Task<DeleteTaskResponseModel> DeleteManyTasksAsync(DeleteManyTasksRequestModel request, CancellationToken cancellationToken = default);
        Task<DeleteTaskResponseModel> DeleteTasksPermanentlyAsync(DeleteManyTasksRequestModel request, CancellationToken cancellationToken = default);
        Task<RestoreTasksResponseModel> RestoreTaskAsync(RestoreTasksRequestModel request, CancellationToken cancellationToken = default);
        Task<GetCounterDeleteTaskTemporaryResponseModel> GetCounterDeleteTaskTemporaryAsync(CancellationToken cancellationToken = default);
    }
}
