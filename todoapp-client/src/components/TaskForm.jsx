import React from "react";
import { useTodoContext } from "../stores/TodoContext";
import useTodo from "../hook/useTodo";

const API_BASE_URL = import.meta.env.VITE_APP_API_BASE_URL;

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

export default TaskForm;
