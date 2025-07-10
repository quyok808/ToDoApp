const TodoService = (apiBaseUrl) => {
  const fetchTasks = async () => {
    const response = await fetch(
      `${apiBaseUrl}/todo/api/v1/Todo/get-all-tasks`
    );
    return await response.json();
  };

  const fetchDeletedTasks = async () => {
    const response = await fetch(
      `${apiBaseUrl}/todo/api/v1/Todo/get-all-tasks-deleted`
    );
    return await response.json();
  };

  const createTask = async (task) => {
    const response = await fetch(`${apiBaseUrl}/todo/api/v1/Todo/create-task`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(task),
    });
    return await response.json();
  };

  const updateTask = async (payload) => {
    const response = await fetch(
      `${apiBaseUrl}/todo/api/v1/Todo/update-task-by-id`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      }
    );
    return await response.json();
  };

  const deleteTask = async (id) => {
    const response = await fetch(
      `${apiBaseUrl}/todo/api/v1/Todo/delete-task-by-id`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ id }),
      }
    );
    return await response.json();
  };

  const deleteManyTasks = async (taskIds) => {
    const response = await fetch(
      `${apiBaseUrl}/todo/api/v1/Todo/delete-many-tasks`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ taskIds }),
      }
    );
    return await response.json();
  };

  const restoreTasks = async (taskIds) => {
    const response = await fetch(
      `${apiBaseUrl}/todo/api/v1/Todo/restore-tasks`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ taskIds }),
      }
    );
    return await response.json();
  };

  const deleteTasksPermanently = async (taskIds) => {
    const response = await fetch(
      `${apiBaseUrl}/todo/api/v1/Todo/delete-tasks-permanently`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ taskIds }),
      }
    );
    return await response.json();
  };

  const counterDeletedTasks = async () => {
    const response = await fetch(
      `${apiBaseUrl}/todo/api/v1/Todo/counter-deleted-tasks`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({}),
      }
    );
    return await response.json();
  };

  return {
    fetchTasks,
    fetchDeletedTasks,
    createTask,
    updateTask,
    deleteTask,
    deleteManyTasks,
    restoreTasks,
    deleteTasksPermanently,
    counterDeletedTasks,
  };
};

export default TodoService;
