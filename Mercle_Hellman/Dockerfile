#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#COPY ["Mercle_Hellman/Mercle_Hellman.csproj", "Mercle_Hellman/"]
#RUN dotnet restore "Mercle_Hellman/Mercle_Hellman.csproj"
COPY . .
#WORKDIR "/src/Mercle_Hellman"
#RUN dotnet build "Mercle_Hellman.csproj" -c Release -o /app/build

#FROM build AS publish
#RUN dotnet publish "Mercle_Hellman.csproj" -c Release -o /app/publish /p:UseAppHost=false

#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Mercle_Hellman.dll"]
ENV tmp_value="hello, Mercle Hellman! :)"

CMD ["dotnet", "Program.cs"]