#!/bin/bash

dotnet publish -c Release -f net10.0 -o ../AspNetCoreMcpServer-publish /p:UseAppHost=false ./AspNetCoreMcpServer.csproj