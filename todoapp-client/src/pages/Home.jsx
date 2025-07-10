import React from "react";
import { useTodoContext } from "../stores/TodoContext";
import ErrorMessage from "../components/ErrorMessage";
import TaskForm from "../components/TaskForm";
import Tabs from "../components/Tabs";
import TaskList from "../components/TaskList";
import TrashList from "../components/TrashList";
import EditModal from "../components/EditModal";
import useTodo from "../hook/useTodo";

const API_BASE_URL = import.meta.env.VITE_APP_API_BASE_URL;

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

export default TodoAppContent;
