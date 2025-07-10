import React from "react";
import { useTodoContext } from "../stores/TodoContext";

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

export default Tabs;
