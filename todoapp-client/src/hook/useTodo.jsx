import { useCallback } from "react";
import { useTodoContext } from "../stores/TodoContext";
import TodoService from "../services/TodoService";

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

  const todoService = TodoService(apiBaseUrl);

  const fetchTasks = useCallback(async () => {
    try {
      const result = await todoService.fetchTasks();
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
  }, [setTasks, showError, todoService]);

  const fetchDeletedTasks = useCallback(async () => {
    try {
      const result = await todoService.fetchDeletedTasks();
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
  }, [setDeletedTasks, showError, todoService]);

  const handleSubmit = async () => {
    if (!newTask.title) {
      showError("Title is required");
      return;
    }
    try {
      const result = await todoService.createTask(newTask);
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
      const result = await todoService.updateTask(payload);
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
      const result = await todoService.updateTask(payload);
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
      const result = await todoService.deleteTask(id);
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
      const result = await todoService.deleteManyTasks(taskIds);
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
      const result = await todoService.restoreTasks([id]);
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
      const result = await todoService.restoreTasks(taskIds);
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
      const result = await todoService.deleteTasksPermanently([id]);
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
      const result = await todoService.deleteTasksPermanently(taskIds);
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
      const result = await todoService.counterDeletedTasks();
      if (result.succeeded) {
        setDeletedTasksCount(result.data.counter);
      } else {
        showError(result.message || "Failed to fetch deleted tasks");
        setDeletedTasksCount(0);
      }
    } catch (err) {
      showError("Error fetching deleted tasks");
      setDeletedTasksCount(0);
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
