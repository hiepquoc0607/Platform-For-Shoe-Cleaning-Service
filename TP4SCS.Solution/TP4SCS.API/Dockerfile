# Chọn image base cho SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["TP4SCS.Solution/TP4SCS.API/TP4SCS.API.csproj", "TP4SCS.API/"]
COPY ["TP4SCS.Solution/TP4SCS.Services/TP4SCS.Services.csproj", "TP4SCS.Services/"]

# Khôi phục các gói cho dự án TP4SCS.API
RUN dotnet restore "TP4SCS.API/TP4SCS.API.csproj"

# Sao chép toàn bộ mã nguồn vào container
COPY . .

# Thiết lập WORKDIR cho TP4SCS.API
WORKDIR "/src/TP4SCS.API"

# Xây dựng dự án
RUN dotnet build "TP4SCS.API.csproj" -c Release -o /app/build

# Stage để publish
FROM build AS publish
RUN dotnet publish "TP4SCS.API.csproj" -c Release -o /app/publish

# Stage cuối cùng cho image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .

# Đặt entry point cho container
ENTRYPOINT ["dotnet", "TP4SCS.API.dll"]
