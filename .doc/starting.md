# Starting

This is a follow-to-guide that provides the instructions to run the project.

## Prerequisites

- Git: to clone the repository
- .NET 8.0 SDK
- Docker: optional, for containerized run
- PostgreSQL: optional, if running the project outside Docker
- Visual Studio Code or Visual Studio as the IDE

## Running Locally

1. **Clone the repository**

   ```sh
   git clone https://github.com/Camarg0/abi-gth-omnia-developer-evaluation.git
   cd abi-gth-omnia-developer-evaluation
   ```

2. **Configure the database**

   - Update the connection strings in `src/Ambev.DeveloperEvaluation.WebApi/appsettings.json` for PostgreSQL as needed.

3. **Apply database migrations**

   In the src directory
   ```sh
   dotnet ef database update --project Ambev.DeveloperEvaluation.ORM --startup-project Ambev.DeveloperEvaluation.WebApi
   ```

4. **Build and run the API**

   In the src directory
   ```sh
   dotnet build Ambev.DeveloperEvaluation.sln
   dotnet run --project src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj
   ```

   The API will be available at `http://localhost:8080` (or as configured).

5. **Access Swagger UI**

   Navigate to `http://localhost:8080/swagger` for API documentation and testing.

## Running with Docker

1. **Build and start containers**

   ```sh
   docker-compose up --build
   ```

   This will start the API and any configured database containers.

2. **Access the API**

   Visit `http://localhost:8080/swagger` for the Swagger UI.

## Running Tests

1. **Run all tests**

   ```sh
   dotnet test Ambev.DeveloperEvaluation.sln
   ```
