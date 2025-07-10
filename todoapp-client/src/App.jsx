import React from "react";
import { TodoProvider } from "./stores/TodoContext";
import TodoAppContent from "./pages/Home";

const App = () => {
  return (
    <TodoProvider>
      <TodoAppContent />
    </TodoProvider>
  );
};

export default App;
