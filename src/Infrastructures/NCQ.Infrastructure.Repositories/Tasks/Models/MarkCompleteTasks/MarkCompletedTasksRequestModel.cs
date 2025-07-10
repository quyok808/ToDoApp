namespace NCQ.Infrastructure.Repositories.Tasks.Models.MarkCompleteTasks
{
    public class MarkCompletedTasksRequestModel
    {
        public List<int> TaskIds { get; set; }
        public int HAVEDONE { get; set; }
    }
}
