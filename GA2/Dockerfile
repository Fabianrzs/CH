# Imágenes de las APIs de GA2 (.NET 5). Una sola etapa de compilación publica las cuatro APIs;
# cada API tiene su propia etapa final (target): ga2, bua, identity, worker.
# Se construye para linux/amd64: IronPdf, IronOcr, wkhtmltopdf y ReportViewer traen binarios nativos x64.

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY global.json GA2.sln ./
COPY HangFire/ HangFire/
COPY src/ src/
# Sólo linux-x64: IronPdf/IronOcr traen binarios nativos de todos los SO (varios GB por API).
# La caché de NuGet se borra en la misma capa para que no quede en la imagen.
RUN for p in GA2 BUA Identity Worker; do \
      dotnet publish src/GA2.Apis/$p/$p.csproj -c Release -r linux-x64 --self-contained false -o /out/$p || exit 1; \
    done \
    && rm -rf /root/.nuget/packages /tmp/* \
    && du -sh /out/*

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
# Debian 11 (bullseye) pasó al archivo de Debian: si el espejo normal falla se usa archive.debian.org.
RUN (apt-get update || ( \
      sed -i -e 's|deb.debian.org|archive.debian.org|g' -e 's|security.debian.org|archive.debian.org|g' \
             -e '/bullseye-updates/d' /etc/apt/sources.list \
      && apt-get update)) \
    && apt-get install -y --no-install-recommends \
      ca-certificates curl libgdiplus libc6-dev fontconfig fonts-dejavu-core fonts-liberation \
      libx11-6 libxext6 libxrender1 libjpeg62-turbo libpng16-16 libssl1.1 \
      libnss3 libatk1.0-0 libatk-bridge2.0-0 libcups2 libdrm2 libxkbcommon0 libxcomposite1 \
      libxdamage1 libxrandr2 libgbm1 libpango-1.0-0 libcairo2 libasound2 libxshmfence1 \
    && rm -rf /var/lib/apt/lists/*
# .NET 5 en Linux no mapea los IDs de zona horaria de Windows que usa el código (p. ej. en el login).
# tzdata ya viene en la imagen base; se crean enlaces con esos nombres hacia las zonas IANA equivalentes.
RUN ln -sf /usr/share/zoneinfo/America/New_York "/usr/share/zoneinfo/Eastern Standard Time" \
 && ln -sf /usr/share/zoneinfo/America/Bogota   "/usr/share/zoneinfo/SA Pacific Standard Time"
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
EXPOSE 8080

FROM base AS ga2
COPY --from=build /out/GA2 .
ENTRYPOINT ["dotnet", "GA2.dll"]

FROM base AS bua
COPY --from=build /out/BUA .
ENTRYPOINT ["dotnet", "BUA.dll"]

FROM base AS identity
COPY --from=build /out/Identity .
ENTRYPOINT ["dotnet", "Identity.dll"]

FROM base AS worker
COPY --from=build /out/Worker .
ENTRYPOINT ["dotnet", "Worker.dll"]
