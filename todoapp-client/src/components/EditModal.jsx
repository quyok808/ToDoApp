import React from "react";
import { useTodoContext } from "../stores/TodoContext";
import useTodo from "../hook/useTodo";

const API_BASE_URL = import.meta.env.VITE_APP_API_BASE_URL;

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

export default EditModal;
