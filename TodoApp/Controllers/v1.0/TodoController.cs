using System.Net;
using CoreLibs.Base;
using CoreLibs.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NCQ.Core.Application.Features.Task.Command;
using NCQ.Core.Application.Features.Task.Queries;

namespace Todo_service.Controllers.v1._0
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("/todo/api/v{version:apiVersion}/[controller]")]
    public class TodoController : BaseController
    {
        public TodoController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Author: QuyNC
        /// CreateDate: 07/07/2025
        /// Description: Lấy danh sách tasks
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("get-all-tasks")]
        [ProducesResponseType(typeof(Response<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllTasks()
        {
            var query = new GetAllTasksQueryRequest();
            return StatusCode((int)HttpStatusCode.OK, await mediator.Send(query));
        }

        /// <summary>
        /// Author: QuyNC
        /// CreateDate: 07/07/2025
        /// Description: Lấy danh sách task xoá tạm thời
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("get-all-tasks-deleted")]
        [ProducesResponseType(typeof(Response<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllTasksDeleted()
        {
            var query = new GetDeleteTasksTemporaryQueryRequest();
			return StatusCode((int)HttpStatusCode.OK, await mediator.Send(query));
        }

        /// <summary>
        /// Author: QuyNC
        /// CreateDate: 07/07/2025
        /// Description: Lấy danh sách task theo id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("get-task-by-id")]
        [ProducesResponseType(typeof(Response<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTaskById([FromBody] GetTaskByIdQueryRequest query)
        {
            return StatusCode((int)HttpStatusCode.OK,await mediator.Send(query));
        }

        /// <summary>
        /// Author: QuyNC
        /// CreateDate: 07/07/2025
        /// Description: Tạo task mới
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create-task")]
        [ProducesResponseType(typeof(Response<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommandRequest command)
        {
            return StatusCode((int)HttpStatusCode.OK,await mediator.Send(command));
        }

        /// <summary>
        /// Author: QuyNC
        /// CreateDate: 07/07/2025
        /// Description: Cập nhật task theo Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("update-task-by-id")]
        [ProducesResponseType(typeof(Response<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommandRequest command)
        {
            return StatusCode((int)HttpStatusCode.OK, await mediator.Send(command));
        }

        /// <summary>
        /// Author: QuyNC
        /// CreateDate: 07/07/2025
        /// Description: Xoá task theo id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("delete-task-by-id")]
        [ProducesResponseType(typeof(Response<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteTaskById([FromBody] DeleteTaskCommandRequest command)
        {
            return StatusCode((int)HttpStatusCode.OK, await mediator.Send(command));
        }

        /// <summary>
        /// Author: QuyNC
        /// CreateDate: 07/07/2025
        /// Description: Xoá nhiều task theo Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("delete-many-tasks")]
        [ProducesResponseType(typeof(Response<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteManyTasks([FromBody] DeleteManyTasksCommandRequest command)
        {
            return StatusCode((int)HttpStatusCode.OK, await mediator.Send(command));
        }

        /// <summary>
        /// Author: QuyNC
        /// CreateDate: 07/07/2025
        /// Description: Xoá vĩnh viễn task đã xoá tạm thời.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("delete-tasks-permanently")]
        [ProducesResponseType(typeof(Response<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteTasksPermanently([FromBody] DeleteTasksPermanentlyCommandRequest command)
        {
            return StatusCode((int)HttpStatusCode.OK, await mediator.Send(command));
        }

        /// <summary>
        /// Author: QuyNC
        /// CreateDate: 07/07/2025
        /// Description: Khôi phục task đã xoá tạm thời
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("restore-tasks")]
        [ProducesResponseType(typeof(Response<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RestoreTasks([FromBody] RestoreTasksCommandRequest command)
        {
            return StatusCode((int)HttpStatusCode.OK, await mediator.Send(command));
        }

        /// <summary>
        /// Author: QuyNC
        /// CreateDate: 10/07/2025
        /// Description: Đếm số lượng task đã xoá tạm thời
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("counter-deleted-tasks")]
        [ProducesResponseType(typeof(Response<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CounterDeletedTasks([FromBody] GetCounterOfDeleteTasksQueryRequest query)
        {
            return StatusCode((int)HttpStatusCode.OK, await mediator.Send(query));
        }

        /// <summary>
        /// Author: QuyNC
        /// CreateDate: 10/07/2025
        /// Description: Đánh dấu hoàn thành nhiều task
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("mark-complete-tasks")]
        [ProducesResponseType(typeof(Response<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> MarkCompleteTasks([FromBody] MarkCompleteTasksCommandRequest command)
        {
            return StatusCode((int)HttpStatusCode.OK, await mediator.Send(command));
        }
    }
}
