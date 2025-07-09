namespace NCQ.Infrastructure.Repositories.Tasks.Models.CreateTask
{
    public class CreateTaskResponseModel
    {
        public int ID { get; set; }
        public string TITLE { get; set; }
        public string DESCRIPTION { get; set; }
        public int HAVEDONE { get; set; }
        public DateTime CREATEDAT { get; set; }
    }
}
