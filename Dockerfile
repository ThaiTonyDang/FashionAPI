FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY . .
RUN mkdir /data
ENTRYPOINT ["dotnet", "API.dll"]