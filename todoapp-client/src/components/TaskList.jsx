import React from "react";
import { useTodoContext } from "../stores/TodoContext";
import useTodo from "../hook/useTodo";

const API_BASE_URL = import.meta.env.VITE_APP_API_BASE_URL;

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

export default TaskList;
