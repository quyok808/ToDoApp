namespace NCQ.Infrastructure.Repositories.Tasks.Models.DeleteTask
{
    public class DeleteManyTasksRequestModel
    {
        public List<int> TaskIds { get; set; } = new();
    }
}
