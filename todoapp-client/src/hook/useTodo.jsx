import { useCallback } from "react";
import { useTodoContext } from "../stores/TodoContext";

const useTodo = (apiBaseUrl) => {
  const {
    setTasks,
    setDeletedTasks,
    setDeletedTasksCount,
    showError,
    newTask,
    setNewTask,
    editTaskData,
    setEditTaskData,
    currentTab,
    deletedTasks,
  } = useTodoContext();

  // const fetchTasks = async () => {
  //   try {
  //     const response = await fetch(
  //       `${apiBaseUrl}/todo/api/v1/Todo/get-all-tasks`
  //     );
  //     const result = await response.json();
  //     if (result.succeeded) {
  //       setTasks(result.data || []);
  //     } else {
  //       showError(result.message || "Failed to fetch tasks");
  //       setTasks([]);
  //     }
  //   } catch (err) {
  //     showError("Error fetching tasks");
  //     setTasks([]);
  //   }
  // };

  // const fetchDeletedTasks = async () => {
  //   try {
  //     const response = await fetch(
  //       `${apiBaseUrl}/todo/api/v1/Todo/get-all-tasks-deleted`
  //     );
  //     const result = await response.json();
  //     if (result.succeeded) {
  //       setDeletedTasks(result.data || []);
  //     } else {
  //       showError(result.message || "Failed to fetch deleted tasks");
  //       setDeletedTasks([]);
  //     }
  //   } catch (err) {
  //     showError("Error fetching deleted tasks");
  //     setDeletedTasks([]);
  //   }
  // };

  const fetchTasks = useCallback(async () => {
    try {
      const response = await fetch(
        `${apiBaseUrl}/todo/api/v1/Todo/get-all-tasks`
      );
      const result = await response.json();
      if (result.succeeded) {
        setTasks(result.data || []);
        await handleCounterDeletedTasks();
      } else {
        showError(result.message || "Failed to fetch tasks");
        setTasks([]);
      }
    } catch (err) {
      showError("Error fetching tasks");
      setTasks([]);
    }
  }, [setTasks]);

  const fetchDeletedTasks = useCallback(async () => {
    try {
      const response = await fetch(
        `${apiBaseUrl}/todo/api/v1/Todo/get-all-tasks-deleted`
      );
      const result = await response.json();
      if (result.succeeded) {
        setDeletedTasks(result.data || []);
        await handleCounterDeletedTasks();
      } else {
        showError(result.message || "Failed to fetch deleted tasks");
        setDeletedTasks([]);
      }
    } catch (err) {
      showError("Error fetching deleted tasks");
      setDeletedTasks([]);
    }
  }, [setDeletedTasks]);

  const handleSubmit = async () => {
    if (!newTask.title) {
      showError("Title is required");
      return;
    }
    try {
      const response = await fetch(
        `${apiBaseUrl}/todo/api/v1/Todo/create-task`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(newTask),
        }
      );
      const result = await response.json();
      if (result.succeeded) {
        setNewTask({ title: "", description: "" });
        await fetchTasks();
      } else {
        showError(result.message || "Failed to save task");
      }
    } catch (err) {
      showError("Error saving task");
    }
  };

  const editTask = (id, title, description) => {
    setEditTaskData({ id, title, description });
  };

  const handleEditSubmit = async () => {
    if (!editTaskData.title) {
      showError("Title is required");
      return;
    }
    const payload = { ...editTaskData, havedone: 0 };
    try {
      const response = await fetch(
        `${apiBaseUrl}/todo/api/v1/Todo/update-task-by-id`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(payload),
        }
      );
      const result = await response.json();
      if (result.succeeded) {
        setEditTaskData(null);
        await fetchTasks();
      } else {
        showError(result.message || "Failed to update task");
      }
    } catch (err) {
      showError("Error updating task");
    }
  };

  const completeTask = async (id, currentStatus) => {
    const havedone = currentStatus ? 0 : 1;
    const payload = { id, havedone };
    try {
      const response = await fetch(
        `${apiBaseUrl}/todo/api/v1/Todo/update-task-by-id`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(payload),
        }
      );
      const result = await response.json();
      if (result.succeeded) {
        await fetchTasks();
      } else {
        showError(result.message || "Failed to update task status");
      }
    } catch (err) {
      showError("Error updating task status");
    }
  };

  const deleteTask = async (id) => {
    try {
      const response = await fetch(
        `${apiBaseUrl}/todo/api/v1/Todo/delete-task-by-id`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ id }),
        }
      );
      const result = await response.json();
      if (result.succeeded) {
        await fetchTasks();
        await handleCounterDeletedTasks();
        if (currentTab === "trash") await fetchDeletedTasks();
      } else {
        showError(result.message || "Failed to delete task");
      }
    } catch (err) {
      showError("Error deleting task");
    }
  };

  const handleDeleteSelected = async () => {
    const checkboxes = document.querySelectorAll(".task-checkbox:checked");
    const taskIds = Array.from(checkboxes)
      .map((checkbox) => parseInt(checkbox.getAttribute("data-task-id")))
      .filter((id) => !isNaN(id));

    if (taskIds.length === 0) {
      showError("No tasks selected");
      return;
    }

    try {
      const response = await fetch(
        `${apiBaseUrl}/todo/api/v1/Todo/delete-many-tasks`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ taskIds }),
        }
      );
      const result = await response.json();
      if (result.succeeded) {
        await fetchTasks();
        await handleCounterDeletedTasks();
        if (currentTab === "trash") await fetchDeletedTasks();
      } else {
        showError(result.message || "Failed to delete selected tasks");
      }
    } catch (err) {
      showError("Error deleting selected tasks");
    }
  };

  const restoreTask = async (id) => {
    try {
      const response = await fetch(
        `${apiBaseUrl}/todo/api/v1/Todo/restore-tasks`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ taskIds: [id] }),
        }
      );
      const result = await response.json();
      if (result.succeeded) {
        await fetchDeletedTasks();
        await fetchTasks();
      } else {
        showError(result.message || "Failed to restore task");
      }
    } catch (err) {
      showError("Error restoring task");
    }
  };

  const handleRestoreAll = async () => {
    const taskIds = deletedTasks
      .map((task) => task.id)
      .filter((id) => !isNaN(id));

    if (!taskIds.length) {
      showError("No tasks to restore");
      return;
    }

    try {
      const response = await fetch(
        `${apiBaseUrl}/todo/api/v1/Todo/restore-tasks`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ taskIds }),
        }
      );
      const result = await response.json();
      if (result.succeeded) {
        await fetchDeletedTasks();
        await fetchTasks();
      } else {
        showError(result.message || "Failed to restore tasks");
      }
    } catch (err) {
      showError("Error restoring tasks");
    }
  };

  const permanentDelete = async (id) => {
    try {
      const response = await fetch(
        `${apiBaseUrl}/todo/api/v1/Todo/delete-tasks-permanently`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ taskIds: [id] }),
        }
      );
      const result = await response.json();
      if (result.succeeded) {
        await fetchDeletedTasks();
      } else {
        showError(result.message || "Failed to permanently delete task");
      }
    } catch (err) {
      showError("Error permanently deleting task");
    }
  };

  const handlePermanentDeleteAll = async () => {
    const taskIds = deletedTasks
      .map((task) => parseInt(task.id))
      .filter((id) => !isNaN(id));

    if (!taskIds.length) {
      showError("No tasks to delete");
      return;
    }

    try {
      const response = await fetch(
        `${apiBaseUrl}/todo/api/v1/Todo/delete-tasks-permanently`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ taskIds }),
        }
      );
      const result = await response.json();
      if (result.succeeded) {
        await fetchDeletedTasks();
      } else {
        showError(result.message || "Failed to permanently delete tasks");
      }
    } catch (err) {
      showError("Error permanently deleting tasks");
    }
  };

  const handleCounterDeletedTasks = async () => {
    try {
      const response = await fetch(
        `${apiBaseUrl}/todo/api/v1/Todo/counter-deleted-tasks`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({}),
        }
      );
      const result = await response.json();
      if (result.succeeded) {
        setDeletedTasksCount(result.data.counter);
      } else {
        showError(result.message || "Failed to fetch deleted tasks");
        setDeletedTasksCount(0);
      }
    } catch (err) {
      showError("Error fetching deleted tasks");
      setDeletedTasks(0);
    }
  };

  return {
    fetchTasks,
    fetchDeletedTasks,
    handleSubmit,
    editTask,
    handleEditSubmit,
    completeTask,
    deleteTask,
    handleDeleteSelected,
    restoreTask,
    handleRestoreAll,
    permanentDelete,
    handlePermanentDeleteAll,
  };
};

export default useTodo;
