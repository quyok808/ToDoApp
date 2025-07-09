namespace NCQ.Infrastructure.Repositories.Tasks.Models.UpdateTask
{
    public class UpdateTaskRequestModel
    {
        public int ID { get; set; }
        public string? TITLE { get; set; } = string.Empty;
        public string? DESCRIPTION { get; set; } = string.Empty;
        public int HAVEDONE { get; set; } = 0;
    }
}
