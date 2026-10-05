FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

RUN apt update \
    && apt install --no-install-recommends -y \
# Mandatory for build
    clang \
    zlib1g-dev

COPY . .

RUN dotnet restore 

# RUN dotnet test -c $BUILD_CONFIGURATION -o /app/tests

RUN dotnet publish -f net10.0 -o /app/publish /p:UseAppHost=false /samples/AspNetCoreMcpServer/AspNetCoreMcpServer.csproj -c $BUILD_CONFIGURATION  /p:DebugSymbols=false /p:DebugType=none

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

RUN apt update \
    && apt install --no-install-recommends -y \
# Mandatory for build
    clang \
    zlib1g-dev \
    tree \
# Optional \
    ca-certificates \
    gnupg \
    wget \
    curl \
    unzip \
    git \
    less \
    build-essential \
    zstd \
    markdown \
    mc \
    python3-pip \
    golang-go \
    dotnet-sdk-10.0 \
    ocl-icd-libopencl1 \
    libhwloc15 \
    screen \
    openssh-server \
    && rm -rf /var/lib/apt/lists/*

RUN curl -fsSL https://pkg.cloudflare.com/cloudflare-main.gpg | tee /usr/share/keyrings/cloudflare-main.gpg >/dev/null

RUN echo 'deb [signed-by=/usr/share/keyrings/cloudflare-main.gpg] https://pkg.cloudflare.com/cloudflared any main' | tee /etc/apt/sources.list.d/cloudflared.list

RUN apt-get update && \
  apt-get install --no-install-recommends -q -y \
  cloudflared \
  && rm -rf /var/lib/apt/lists/*

RUN echo "PermitRootLogin yes" >> /etc/ssh/sshd_config

COPY ./authorized_keys /root/.ssh/authorized_keys    

RUN mkdir /workspace

WORKDIR /workspace

COPY --from=build /app/publish /app/

EXPOSE 3001

ENTRYPOINT ["/app/AspNetCoreMcpServer", "--urls", "http://*:3001"]
