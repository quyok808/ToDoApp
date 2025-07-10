# ToDoApp

A modern To-Do List application for managing tasks efficiently. The backend is built with **ASP.NET Core** using a **Layered Architecture** and **Mediator Pattern** for clean and scalable code. The frontend is developed with **ReactJS**, providing a responsive and intuitive user interface.

## Features

- **Add Tasks**: Create new tasks with a title and optional description.
- **Edit Tasks**: Update task details to keep your list organized.
- **Delete Tasks**: Remove tasks that are no longer needed.
- **Mark as Completed**: Track task completion with a simple checkbox.
- **Responsive UI**: Seamlessly works on mobile and desktop devices.
- **Persistent Storage**: Tasks are stored in a database via the backend.
- **Clean Architecture**: Backend follows layered architecture with Mediator for decoupling.

## Tech Stack

### Backend

- **Framework**: ASP.NET Core
- **Architecture**: Layered Architecture (Presentation, Application, Domain, Infrastructure)
- **Pattern**: Mediator Pattern (using MediatR library)
- **Database**: [Oracle]
- **Other Libraries**: Dapper, Oracle.ManagedDataAccess.Core

### Frontend

- **Framework**: ReactJS
- **State Management**: [React Context]
- **HTTP Client**: FetchJs for API calls
- **Styling**: [Tailwind CSS]

## Getting Started

### Prerequisites

- **Backend**:
  - [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0)
  - Database: [Oracle]
  - IDE: [Visual Studio](https://visualstudio.microsoft.com/)
- **Frontend**:
  - [Node.js](https://nodejs.org) (version 16 or later)
  - Package Manager: npm
- Git installed on your machine.

### Installation

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/quyok808/ToDoApp.git
   cd ToDoApp
   ```
2. **Set up backend**:

- Navigate to the backend folder:
  ```bash
  cd ToDoApp
  ```
- Install dependencies:
  ```bash
  dotnet restore
  ```
- Configure the database connection in `appsettings.json`
- Run the backend:
  ```bash
  dotnet run
  ```

3. **Set up frontend**:

- Navigate to the frontend folder:
  ```bash
  cd todoapp-client
  ```
- Install dependencies:
  ```bash
  npm install
  ```
- Run the frontend:
  ```bash
  npm run dev
  ```

4. **Access the App**:

- Backend API: `https://localhost:44346`
- Frontend: `http://localhost:5173`

## Usage

1. Start the backend API and frontend development server.
2. Open the app in your browser at `http://localhost:5173`.
3. Create, edit, delete, or mark tasks as completed using the intuitive interface.
4. Tasks are synced with the backend and stored in the database.

## Project Structure

### Backend

- **Presentation**: API controllers and endpoints.
- **Application**: MediatR handlers, commands, and queries.
- **Domain**: Middlewares.
- **Shared**: Shared Libraries.
- **Infrastructure**: Repositories, Interfaces, Layers Tranfer Object Models.

### Frontend

- **src/components**: Reusable React components.
- **src/pages**: Page-level components.
- **src/services**: API calls and data fetching logic.
- **src/stores**: State management using React Context.
