# Base image cho ứng dụng ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Image cho quá trình build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy file .csproj vào thư mục build
COPY ["TP4SCS.Solution/TP4SCS.API/TP4SCS.API.csproj", "TP4SCS.API/"]
RUN dotnet restore "./TP4SCS.API/TP4SCS.API.csproj"

# Copy tất cả các file còn lại vào image
COPY . .

# Chuyển vào thư mục dự án và build
WORKDIR "/src/TP4SCS.API"
RUN dotnet build "./TP4SCS.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage để publish ứng dụng
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TP4SCS.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Image cuối cùng cho chạy ứng dụng
FROM base AS final
WORKDIR /app

# Copy các file đã publish từ stage publish
COPY --from=publish /app/publish .

# Chạy ứng dụng
ENTRYPOINT ["dotnet", "TP4SCS.API.dll"]
