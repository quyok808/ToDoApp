FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Thiết lập proxy để mọi lệnh bên dưới đều dùng được (apt, curl, git...)
ENV http_proxy=http://proxy.fpt.vn:80
ENV https_proxy=http://proxy.fpt.vn:80
ENV no_proxy=.fpt.net

# 2. Copy nuget.config TRƯỚC khi restore
COPY nuget.config /root/.nuget/NuGet/NuGet.Config

# 3. Copy toàn bộ mã nguồn (sau khi copy config)
COPY . .

# 4. Restore và publish
RUN dotnet restore TodoApp.sln
RUN dotnet publish TodoApp/TodoApp.csproj -c Release -o /app/publish

# ------------------------------
# RUNTIME STAGE
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TodoApp.dll"]
