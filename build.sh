#!/usr/bin/env bash
# Construye las imágenes de forma secuencial (las cuatro APIs comparten la etapa de compilación en caché).
# Compose las construiría en paralelo, repitiendo la compilación .NET cuatro veces bajo emulación amd64.
set -euo pipefail
cd "$(dirname "$0")"

for target in ga2 bua identity worker; do
  tag=$([ "$target" = ga2 ] && echo ga2-api || echo "ga2-$target")
  echo "==> $tag"
  podman build --platform linux/amd64 --target "$target" -t "localhost/$tag" GA2
done

echo "==> ga2-app"
podman build --platform linux/amd64 -t localhost/ga2-app GA2APP

echo "==> mockintegrations"
podman compose build mockintegrations
