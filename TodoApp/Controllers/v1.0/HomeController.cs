using CoreLibs.Base;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Mvc;
using NCQ.Infrastructure.Repositories.Configuration;
using MediatR;

namespace Todo_service.Controllers.v1._0
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("/todo/api/v{version:apiVersion}/[controller]")]
    public class HomeController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;
        public HomeController(IMediator mediator, IConfiguration configuration, IHostEnvironment hostEnvironment) : base(mediator)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Author: QuyNC
        /// CreateDate: 07/07/2025
        /// Description: Welcome Page
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("index")]
        public IActionResult Index() => Ok("Wellcome APIs V1 Home.");

        /// <summary>
        /// Author: QuyNC
        /// CreateDate: 07/07/2025
        /// Description: Check configs
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("config")]
        public IActionResult Config()
        {
            IEnumerable<KeyValuePair<string, string>> allConfiguration = _configuration.AsEnumerable();
            Dictionary<string, string> allKeyValues = new Dictionary<string, string>();

            foreach (var item in allConfiguration)
            {
                allKeyValues[item.Key] = item.Value;
            }

            return Ok(allKeyValues);
        }

        /// <summary>
        /// Author: QuyNC
        /// CreateDate: 07/07/2025
        /// Description: check connection Db
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("check-health")]
        public async Task<IActionResult> CheckHealth()
        {
            try
            {
                var connectionString = _configuration[nameof(ConnectionConfigs.PLSQLServerEquipmentServiceConnection)];
                using (var conn = new OracleConnection(connectionString))
                    conn.Open();
                return Ok(new { PLSQLServerEquipmentServiceConnection = true});
            }
            catch (Exception ex)
            {
                return Ok(new { PLSQLServerEquipmentServiceConnection = false, Message = ex.ToString()});
            }
        }
    }
}
