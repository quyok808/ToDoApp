import React from "react";
import { useTodoContext } from "../stores/TodoContext";
import useTodo from "../hook/useTodo";

const API_BASE_URL = import.meta.env.VITE_APP_API_BASE_URL;

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

export default TrashList;
