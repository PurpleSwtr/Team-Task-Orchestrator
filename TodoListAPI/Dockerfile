# syntax=docker/dockerfile:1

# --- Стадия 1: Сборка приложения ---
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /source

# Копируем файлы проектов для восстановления зависимостей
# Пути относительны контекста, который мы задали в compose.yaml (корень проекта)
COPY *.sln .
COPY TodoListAPI/*.csproj ./TodoListAPI/
RUN dotnet restore "./TodoListAPI/TodoListAPI.csproj"

# Копируем весь остальной исходный код
COPY . .

# Публикуем приложение
WORKDIR "/source/TodoListAPI"
ARG TARGETARCH
RUN dotnet publish TodoListAPI.csproj -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app


# --- Стадия 2: Финальный образ для запуска ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Копируем скомпилированное приложение из стадии 'build'
COPY --from=build /app .

# Устанавливаем ICU-библиотеки для поддержки глобализации
RUN apk add --no-cache icu-libs

# Устанавливаем переменные окружения, чтобы .NET использовал эти библиотеки
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV LC_ALL=en_US.UTF-8
ENV LANG=en_US.UTF-8

# Устанавливаем непривилегированного пользователя
USER $APP_UID

# Точка входа
ENTRYPOINT ["dotnet", "TodoListAPI.dll"]