# Stage 1: Base image cho ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: Build project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy file .csproj vào thư mục build
COPY ["TP4SCS.Solution/TP4SCS.API/TP4SCS.API.csproj", "TP4SCS.API/"]
RUN ls -la TP4SCS.API/ # Debug: liệt kê các file để kiểm tra
RUN dotnet restore "TP4SCS.API/TP4SCS.API.csproj"

# Copy tất cả các file còn lại
COPY TP4SCS.Solution/TP4SCS.API/. ./TP4SCS.API/
WORKDIR "/src/TP4SCS.API"

# Debug: thêm chi tiết output khi build
RUN dotnet build "TP4SCS.API.csproj" -c $BUILD_CONFIGURATION -o /app/build --verbosity detailed

# Stage 3: Publish project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TP4SCS.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Tạo image final từ base image
FROM base AS final
WORKDIR /app

# Copy các file đã publish từ stage publish
COPY --from=publish /app/publish .

# Chạy ứng dụng
ENTRYPOINT ["dotnet", "TP4SCS.API.dll"]
