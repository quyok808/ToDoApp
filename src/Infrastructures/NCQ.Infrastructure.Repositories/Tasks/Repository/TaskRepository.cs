using System.Data;
using CoreLibs.Helpers;
using Dapper;
using DbContext.Connections;
using DbContext.Oracles;
using Microsoft.Extensions.Configuration;
using NCQ.Infrastructure.Repositories.Configuration;
using NCQ.Infrastructure.Repositories.Tasks.Models.CreateTask;
using NCQ.Infrastructure.Repositories.Tasks.Models.DeleteTask;
using NCQ.Infrastructure.Repositories.Tasks.Models.GetAllTasks;
using NCQ.Infrastructure.Repositories.Tasks.Models.GetCounterDeleteTaskTemporary;
using NCQ.Infrastructure.Repositories.Tasks.Models.GetTaskById;
using NCQ.Infrastructure.Repositories.Tasks.Models.RestoreTask;
using NCQ.Infrastructure.Repositories.Tasks.Models.UpdateTask;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace NCQ.Infrastructure.Repositories.Tasks.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IConnection connection;
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public TaskRepository(IConnection connection, IConfiguration configuration)
        {
            this.connection = connection;
            this.configuration = configuration;
            connectionString = this.configuration[nameof(ConnectionConfigs.PLSQLServerEquipmentServiceConnection)];
        }

        public async Task<CreateTaskResponseModel> CreateTask(CreateTaskRequestModel request, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = connection.OracleConnection(connectionString);
            dbConnection.Open();
            IDbTransaction transaction = dbConnection.BeginTransaction();
            try
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_title", request.TITLE);
                parameters.Add("p_description", request.DESCRIPTION);
                parameters.Add("p_id", null, OracleDbType.Int32, ParameterDirection.Output);
                string query1 = @"INSERT INTO TASKS (NAME, DESCRIPTION, CREATEDAT, UPDATEDAT) VALUES (:p_title, :p_description, SYSTIMESTAMP, SYSTIMESTAMP) RETURNING ID INTO :p_id";

                var result = await dbConnection.ExecuteAsync(
                    sql: query1,
                    param: parameters,
                    transaction: transaction,
                    commandTimeout: ExecuteTimeout.Write,
                    commandType: CommandType.Text
                );

                if (result > 0)
                {
                    transaction.Commit();
                    var oracleDecimal = parameters.Get<OracleDecimal>("p_id");
                    int newId = oracleDecimal.ToInt32();
                    var parameter2 = new OracleDynamicParameters();
                    parameter2.Add("p_newId", newId);
                    string query2 = @"SELECT ID, NAME AS TITLE, DESCRIPTION, HAVEDONE, CREATEDAT FROM TASKS WHERE ID = :p_newId";
                    return await dbConnection.QueryFirstOrDefaultAsync<CreateTaskResponseModel>(
                        sql: query2,
                        param: parameter2,
                        transaction: null,
                        commandTimeout: ExecuteTimeout.Read,
                        commandType: CommandType.Text
                    );
                } else
                {
                    transaction.Rollback();
                    return new CreateTaskResponseModel();
                }

            }
            finally { dbConnection.Close(); }
        }

        public async Task<DeleteTaskResponseModel> DeleteManyTasksAsync(DeleteManyTasksRequestModel request, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = connection.OracleConnection(connectionString);
            dbConnection.Open();
            IDbTransaction transaction = dbConnection.BeginTransaction();

            var response = new DeleteTaskResponseModel
            {
                Result = 1,
                Description = "Xóa thành công tất cả"
            };

            try
            {
                foreach (var id in request.TaskIds)
                {
                    var parameters = new OracleDynamicParameters();
                    parameters.Add("p_Id", id);
                    parameters.Add("p_Description", null, OracleDbType.NVarchar2, ParameterDirection.Output, 32000);
                    parameters.Add("p_Result", null, OracleDbType.Int32, ParameterDirection.Output);

                    string query = @"
                        BEGIN
                            UPDATE TASKS 
                            SET ENABLE = 0, UPDATEDAT = SYSTIMESTAMP 
                            WHERE ID = :p_Id;

                            IF sql%rowcount > 0 THEN
                                :p_Description := 'Xóa thành công';
                                :p_Result := 1;
                            ELSE
                                :p_Description := 'Xóa thất bại';
                                :p_Result := 0;
                            END IF;
                        END;";

                    await dbConnection.ExecuteAsync(
                        sql: query,
                        param: parameters,
                        transaction: transaction,
                        commandTimeout: ExecuteTimeout.Read,
                        commandType: CommandType.Text);

                    int result = Convert.ToInt32(parameters.GetParameterValue("p_Result"));
                    string desc = parameters.GetParameterValue("p_Description")?.ToString();

                    if (result == 0)
                    {
                        transaction.Rollback();

                        response.Result = 0;
                        response.Description = $"Xóa thất bại với ID: {id}. Chi tiết: {desc}";
                        return response;
                    }
                }

                transaction.Commit();
                return response;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response.Result = 0;
                response.Description = $"Xảy ra lỗi khi xóa: {ex.Message}";
                return response;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public async Task<DeleteTaskResponseModel> DeleteTask(int Id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = connection.OracleConnection(connectionString);
            dbConnection.Open();
            try
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_Id", Id);
                parameters.Add("p_Description", null, OracleDbType.NVarchar2, ParameterDirection.Output, 32000);
                parameters.Add("p_Result", null, OracleDbType.Int32, ParameterDirection.Output );
				string query = @"
                        BEGIN
                            UPDATE TASKS 
                            SET ENABLE = 0, UPDATEDAT = SYSTIMESTAMP 
                            WHERE ID = :p_Id;

                            IF SQL%ROWCOUNT > 0 THEN
                                :p_Description := 'Xóa thành công';
                                :p_Result := 1;
                                COMMIT;
                            ELSE
                                :p_Result := 0;
                                :p_Description := 'Xóa thất bại';
                            END IF;
                        END;";

				var dbResult = await dbConnection.QueryMultipleAsync(
				sql: query,
				param: parameters,
				transaction: null,
				commandTimeout: ExecuteTimeout.Write,
				commandType: CommandType.Text);
				var response = new DeleteTaskResponseModel();
				using (var reader = dbResult)
				{
					response.Result = Convert.ToInt32(parameters.GetParameterValue("p_Result"));
					response.Description = parameters.GetParameterValue("p_Description").ToString();
				}
				return response;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public async Task<DeleteTaskResponseModel> DeleteTasksPermanentlyAsync(DeleteManyTasksRequestModel request, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = connection.OracleConnection(connectionString);
            dbConnection.Open();
            using var transaction = dbConnection.BeginTransaction();

            try
            {
                int totalDeleted = 0;

                foreach (var id in request.TaskIds)
                {
                    var parameters = new OracleDynamicParameters();
                    parameters.Add("p_id", id);
                    var query = "DELETE FROM TASKS WHERE ID = :p_id";

                    var result = await dbConnection.ExecuteAsync(
                        sql: query,
                        param: parameters,
                        transaction: transaction,
                        commandTimeout: ExecuteTimeout.Write,
                        commandType: CommandType.Text
                    );

                    if (result == 0)
                    {
                        transaction.Rollback();
                        return new DeleteTaskResponseModel { Result = 0, Description = $"Không thể xóa task với ID = {id}" };
                    }

                    totalDeleted += result;
                }

                transaction.Commit();
                return new DeleteTaskResponseModel
                {
                    Result = 1,
                    Description = $"Xoá thành công {totalDeleted} task"
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new DeleteTaskResponseModel
                {
                    Result = 0,
                    Description = $"Xoá thất bại: {ex.Message}"
                };
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public async Task<IEnumerable<GetAllTaskResponseModel>> GetAllDeleteTasksTemporaryAsync(CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = connection.OracleConnection(connectionString);
            dbConnection.Open();
            try
            {
                string query = @"SELECT ID, NAME AS TITLE, DESCRIPTION, HAVEDONE, CREATEDAT, UPDATEDAT FROM TASKS WHERE ENABLE=0 ORDER BY HAVEDONE ASC";
                return await dbConnection.QueryAsync<GetAllTaskResponseModel>(
                    sql: query,
                    param: null,
                    transaction: null,
                    commandTimeout: ExecuteTimeout.Read,
                    commandType: CommandType.Text
                );
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public async Task<IEnumerable<GetAllTaskResponseModel>> GetAllTasks(CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = connection.OracleConnection(connectionString);
            dbConnection.Open();
            try
            {
                string query = @"SELECT ID, NAME AS TITLE, DESCRIPTION, HAVEDONE, CREATEDAT, UPDATEDAT FROM TASKS WHERE ENABLE=1 ORDER BY HAVEDONE ASC, CREATEDAT DESC";
                return await dbConnection.QueryAsync<GetAllTaskResponseModel>(
                    sql: query,
                    param: null,
                    transaction: null,
                    commandTimeout: ExecuteTimeout.Read,
                    commandType: CommandType.Text
                );
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public async Task<GetCounterDeleteTaskTemporaryResponseModel> GetCounterDeleteTaskTemporaryAsync(CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = connection.OracleConnection(connectionString);
            dbConnection.Open();
            try
            {
                var query = @"SELECT COUNT(ID) AS COUNTER FROM TASKS WHERE ENABLE = 0";
                return await dbConnection.QueryFirstOrDefaultAsync<GetCounterDeleteTaskTemporaryResponseModel>(
                    sql: query,
                    param: null,
                    transaction: null,
                    commandTimeout: ExecuteTimeout.Read,
                    commandType: CommandType.Text
                );
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public async Task<GetTaskByIdResponseModel> GetTaskById(GetTaskByIdRequestModel request, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = connection.OracleConnection(connectionString);
            dbConnection.Open();
            try
            {
                var parameter = new OracleDynamicParameters();
                parameter.Add("id", request.ID);
                string query = @"SELECT ID, NAME AS TITLE, DESCRIPTION, HAVEDONE, CREATEDAT, UPDATEDAT FROM TASKS WHERE ID = :id AND ENABLE=1";
                return await dbConnection.QueryFirstOrDefaultAsync<GetTaskByIdResponseModel>(
                    sql: query,
                    param: parameter,
                    transaction: null,
                    commandTimeout: ExecuteTimeout.Read,
                    commandType: CommandType.Text
                );
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public async Task<RestoreTasksResponseModel> RestoreTaskAsync(RestoreTasksRequestModel request, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = connection.OracleConnection(connectionString);
            dbConnection.Open();
            using var transaction = dbConnection.BeginTransaction();

            try
            {
                int totalUpdated = 0;

                foreach (var id in request.TaskIds)
                {
                    var parameters = new OracleDynamicParameters();
                    parameters.Add("p_id", id);

                    string query = @"
                        UPDATE TASKS 
                        SET UPDATEDAT = SYSTIMESTAMP, ENABLE = 1
                        WHERE ID = :p_id";

                    var result = await dbConnection.ExecuteAsync(
                        sql: query,
                        param: parameters,
                        transaction: transaction,
                        commandTimeout: ExecuteTimeout.Write,
                        commandType: CommandType.Text
                    );

                    if (result == 0)
                    {
                        transaction.Rollback();
                        return new RestoreTasksResponseModel { Result = 0, Description = $"Không thể khôi phục task ID: {id}" };
                    }

                    totalUpdated += result;
                }

                transaction.Commit();
                return new RestoreTasksResponseModel { Result = 1, Description = $"Khôi phục thành công {totalUpdated} task(s)" };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new RestoreTasksResponseModel { Result = 0, Description = $"Lỗi: {ex.Message}" };
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public async Task<UpdateTaskResponseModel> UpdateTask(int Id, UpdateTaskRequestModel request, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = connection.OracleConnection(connectionString);
            dbConnection.Open();
            IDbTransaction transaction = dbConnection.BeginTransaction(); 
            try
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", Id);
                parameters.Add("p_title", request.TITLE);
                parameters.Add("p_description", request.DESCRIPTION);
                parameters.Add("p_haveDone", request.HAVEDONE);
                string query = @"UPDATE TASKS SET ";
                if (!string.IsNullOrEmpty(request.TITLE))
                {
                    query += @"NAME = :p_title, ";
                }
                if (!string.IsNullOrEmpty(request.DESCRIPTION))
                {
                    query += @"DESCRIPTION = :p_description, ";
                }
                if (request.HAVEDONE != null && request.HAVEDONE >= 0 && request.HAVEDONE <= 1)
                {
                    query += @"HAVEDONE = :p_haveDone, ";
                }
                query += @"UPDATEDAT = SYSTIMESTAMP
                           WHERE ID = :p_id AND ENABLE=1";
                var result = await dbConnection.ExecuteAsync(
                    sql: query,
                    param: parameters,
                    transaction: transaction,
                    commandTimeout: ExecuteTimeout.Write,
                    commandType: CommandType.Text
                );

                if (result > 0)
                {
                    transaction.Commit();
                    return new UpdateTaskResponseModel { RESULT = 1, DESCRIPTION = "cập nhật thành công" };
                }
                else
                {
                    transaction.Rollback();
                    return new UpdateTaskResponseModel { RESULT = 0, DESCRIPTION = "Cập nhật thất bại" };
                }
            } catch (Exception ex){
                Console.WriteLine(ex);
                throw ;
            }
            finally
            {
                dbConnection.Close();
            }
        }
    }
}
