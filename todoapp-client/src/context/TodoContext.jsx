import React, { createContext, useState } from "react";

const TodoContext = createContext();

const TodoProvider = ({ children }) => {
  const [tasks, setTasks] = useState([]);
  const [deletedTasks, setDeletedTasks] = useState([]);
  const [error, setError] = useState("");
  const [currentTab, setCurrentTab] = useState("tasks");
  const [newTask, setNewTask] = useState({ title: "", description: "" });
  const [editTaskData, setEditTaskData] = useState(null);

  const showError = (message) => {
    setError(message);
    setTimeout(() => setError(""), 5000);
  };

  const closeModal = () => {
    setEditTaskData(null);
  };

  return (
    <TodoContext.Provider
      value={{
        tasks,
        setTasks,
        deletedTasks,
        setDeletedTasks,
        error,
        showError,
        currentTab,
        setCurrentTab,
        newTask,
        setNewTask,
        editTaskData,
        setEditTaskData,
        closeModal
      }}
    >
      {children}
    </TodoContext.Provider>
  );
};

const useTodoContext = () => {
  const context = React.useContext(TodoContext);
  if (context === undefined) {
    throw new Error("useTodoContext must be used within a TodoProvider");
  }
  return context;
};

export { TodoProvider, useTodoContext };
