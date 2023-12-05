FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . .
RUN dotnet add package Azure.AI.OpenAI
RUN dotnet build

EXPOSE 80
ENTRYPOINT ["dotnet", "run"]
