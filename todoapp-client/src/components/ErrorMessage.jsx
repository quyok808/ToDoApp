import React from "react";
import { useTodoContext } from "../stores/TodoContext";

const ErrorMessage = () => {
  const { error } = useTodoContext();
  if (!error) return null;
  return (
    <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
      {error}
    </div>
  );
};

export default ErrorMessage;
