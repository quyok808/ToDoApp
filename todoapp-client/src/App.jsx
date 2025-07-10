import React from "react";
import { TodoProvider, useTodoContext } from "./stores/TodoContext";
import useTodo from "./hook/useTodo";

const API_BASE_URL = import.meta.env.VITE_APP_API_BASE_URL;

const ErrorMessage = () => {
  const { error } = useTodoContext();
  if (!error) return null;
  return (
    <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
      {error}
    </div>
  );
};

const TaskForm = () => {
  const { newTask, setNewTask } = useTodoContext();
  const { handleSubmit } = useTodo(API_BASE_URL);

  const handleChange = (e) => {
    setNewTask({ ...newTask, [e.target.name]: e.target.value });
  };

  return (
    <div className="bg-white p-6 rounded-lg shadow-md mb-6">
      <h2 className="text-xl font-semibold mb-4">Add New Task</h2>
      <div className="space-y-4">
        <input
          name="title"
          value={newTask.title}
          onChange={handleChange}
          type="text"
          className="w-full p-2 border rounded"
          placeholder="Task Title"
        />
        <textarea
          name="description"
          value={newTask.description}
          onChange={handleChange}
          className="w-full p-2 border rounded"
          placeholder="Task Description"
        ></textarea>
        <button
          onClick={handleSubmit}
          className="w-full bg-blue-500 text-white p-2 rounded hover:bg-blue-600"
        >
          Add Task
        </button>
      </div>
    </div>
  );
};

const Tabs = () => {
  const { currentTab, setCurrentTab, DeletedTasksCount } = useTodoContext();

  return (
    <div className="mb-4">
      <div className="flex border-b">
        <button
          onClick={() => setCurrentTab("tasks")}
          className={`flex-1 py-2 px-4 text-center font-semibold ${
            currentTab === "tasks"
              ? "text-blue-500 border-b-2 border-blue-500"
              : "text-gray-500 hover:text-blue-500"
          }`}
        >
          Tasks
        </button>
        <button
          onClick={() => setCurrentTab("trash")}
          className={`flex-1 py-2 px-4 text-center font-semibold ${
            currentTab === "trash"
              ? "text-blue-500 border-b-2 border-blue-500"
              : "text-gray-500 hover:text-blue-500"
          }`}
        >
          Trash{" "}
          {DeletedTasksCount > 0 && (
            <span className="text-red-500">({DeletedTasksCount})</span>
          )}
        </button>
      </div>
    </div>
  );
};

const TaskList = () => {
  const { tasks } = useTodoContext();
  const { deleteTask, completeTask, editTask } = useTodo(API_BASE_URL);

  if (tasks.length === 0) {
    return <p className="text-gray-500">No tasks available</p>;
  }

  const formatDate = (dateString) => {
    if (!dateString || dateString === "0001-01-01T00:00:00Z") return "N/A";
    return new Date(dateString).toLocaleString("en-US", {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  return (
    <ul className="space-y-2">
      {tasks.map((task) => (
        <li
          key={task.id}
          data-task-id={task.id}
          className={`relative flex justify-between items-center p-4 border-b ${
            task.havedone ? "bg-green-100" : ""
          }`}
        >
          {task.havedone === 1 && (
            <div className="absolute inset-0 flex items-center justify-center pointer-events-none z-30">
              <span className="text-red-500 text-xl font-bold opacity-80 rotate-[-30deg]">
                ĐÃ HOÀN THÀNH
              </span>
            </div>
          )}
          <div className="flex items-center space-x-2">
            <input
              type="checkbox"
              className="task-checkbox"
              data-task-id={task.id}
            />
            <div>
              <h3 className="font-medium">{task.title}</h3>
              <p className="text-gray-600">{task.description || ""}</p>
              <p className="text-sm text-gray-500">
                Created: {formatDate(task.createdat)}
              </p>
              <p className="text-sm text-gray-500">
                Updated: {formatDate(task.updatedat)}
              </p>
            </div>
          </div>
          <div className="space-x-2">
            {task.havedone === 0 && (
              <button
                onClick={() => editTask(task.id, task.title, task.description)}
                className="text-blue-500 hover:underline"
              >
                Edit
              </button>
            )}
            <button
              onClick={() => deleteTask(task.id)}
              className="text-red-500 hover:underline"
            >
              Delete
            </button>
            <button
              onClick={() => completeTask(task.id, task.havedone)}
              className={`text-${
                task.havedone ? "yellow" : "green"
              }-500 hover:underline`}
            >
              {task.havedone ? "Undo" : "Complete"}
            </button>
          </div>
        </li>
      ))}
    </ul>
  );
};

const TrashList = () => {
  const { deletedTasks } = useTodoContext();
  const { restoreTask, permanentDelete } = useTodo(API_BASE_URL);

  if (deletedTasks.length === 0) {
    return <p className="text-gray-500">No deleted tasks</p>;
  }

  const formatDate = (dateString) => {
    if (!dateString || dateString === "0001-01-01T00:00:00Z") return "N/A";
    return new Date(dateString).toLocaleString("en-US", {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  return (
    <ul className="space-y-2">
      {deletedTasks.map((task) => (
        <li
          key={task.id}
          data-task-id={task.id}
          className="flex justify-between items-center p-4 border-b"
        >
          <div>
            <h3 className="font-medium text-gray-500">{task.title}</h3>
            <p className="text-gray-400">{task.description || ""}</p>
            <p className="text-sm text-gray-400">
              Created: {formatDate(task.createdat)}
            </p>
            <p className="text-sm text-gray-400">
              Updated: {formatDate(task.updatedat)}
            </p>
          </div>
          <div className="space-x-2">
            <button
              onClick={() => restoreTask(task.id)}
              className="text-green-500 hover:underline"
            >
              Restore
            </button>
            <button
              onClick={() => permanentDelete(task.id)}
              className="text-red-500 hover:underline"
            >
              Delete Permanently
            </button>
          </div>
        </li>
      ))}
    </ul>
  );
};

const EditModal = () => {
  const { editTaskData, setEditTaskData, closeModal } = useTodoContext();
  const { handleEditSubmit } = useTodo(API_BASE_URL);

  if (!editTaskData) return null;

  const handleChange = (e) => {
    setEditTaskData({ ...editTaskData, [e.target.name]: e.target.value });
  };

  return (
    <div className="fixed inset-0 bg-gray-600 bg-opacity-50 flex items-center justify-center">
      <div className="bg-white p-6 rounded-lg shadow-lg w-full max-w-md">
        <h2 className="text-xl font-semibold mb-4">Edit Task</h2>
        <div className="space-y-4">
          <input
            name="title"
            value={editTaskData.title}
            onChange={handleChange}
            type="text"
            className="w-full p-2 border rounded"
            placeholder="Task Title"
          />
          <textarea
            name="description"
            value={editTaskData.description}
            onChange={handleChange}
            className="w-full p-2 border rounded"
            placeholder="Task Description"
          ></textarea>
          <div className="flex justify-end space-x-2">
            <button
              onClick={closeModal}
              className="px-4 py-2 bg-gray-300 rounded hover:bg-gray-400"
            >
              Cancel
            </button>
            <button
              onClick={handleEditSubmit}
              className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
            >
              Update Task
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

const TodoAppContent = () => {
  const { currentTab } = useTodoContext();
  const {
    fetchTasks,
    fetchDeletedTasks,
    handleDeleteSelected,
    handleRestoreAll,
    handlePermanentDeleteAll,
  } = useTodo(API_BASE_URL);

  React.useEffect(() => {
    if (currentTab === "tasks") {
      fetchTasks();
    } else {
      fetchDeletedTasks();
    }
  }, [currentTab, fetchTasks, fetchDeletedTasks]);

  return (
    <div className="container mx-auto p-4 max-w-2xl">
      <h1 className="text-3xl font-bold mb-6 text-center">TODO List</h1>
      <ErrorMessage />
      <TaskForm />
      <Tabs />
      {currentTab === "tasks" ? (
        <div className="bg-white p-6 rounded-lg shadow-md">
          <div className="flex justify-between items-center mb-4">
            <h2 className="text-xl font-semibold">Tasks</h2>
            <button
              onClick={handleDeleteSelected}
              className="bg-red-500 text-white p-2 rounded hover:bg-red-600"
            >
              Delete Selected
            </button>
          </div>
          <TaskList />
        </div>
      ) : (
        <div className="bg-white p-6 rounded-lg shadow-md">
          <h2 className="text-xl font-semibold mb-4">Trash</h2>
          <TrashList />
          <div className="mt-4 flex space-x-2">
            <button
              onClick={handlePermanentDeleteAll}
              className="flex-1 bg-red-500 text-white p-2 rounded hover:bg-red-600"
            >
              Delete All Permanently
            </button>
            <button
              onClick={handleRestoreAll}
              className="flex-1 bg-green-500 text-white p-2 rounded hover:bg-green-600"
            >
              Restore All
            </button>
          </div>
        </div>
      )}
      <EditModal />
    </div>
  );
};

const App = () => {
  return (
    <TodoProvider>
      <TodoAppContent />
    </TodoProvider>
  );
};

export default App;
