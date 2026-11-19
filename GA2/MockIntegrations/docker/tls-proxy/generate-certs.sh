#!/usr/bin/env bash
# Genera una CA local y un certificado para "localhost" (SAN localhost/127.0.0.1) en ./certs.
# La CA (certs/ga2-mocks-ca.crt) debe instalarse como confiable en el host/contenedor donde corre GA2.
set -euo pipefail
cd "$(dirname "$0")"
mkdir -p certs
openssl req -x509 -newkey rsa:2048 -nodes -days 365 -subj "/CN=GA2 Mocks Local CA" \
  -keyout certs/ga2-mocks-ca.key -out certs/ga2-mocks-ca.crt 2>/dev/null
openssl req -newkey rsa:2048 -nodes -subj "/CN=localhost" \
  -keyout certs/servicebus-local.key -out certs/servicebus-local.csr 2>/dev/null
printf "subjectAltName=DNS:localhost,IP:127.0.0.1\nextendedKeyUsage=serverAuth\n" > certs/san.ext
openssl x509 -req -in certs/servicebus-local.csr -CA certs/ga2-mocks-ca.crt -CAkey certs/ga2-mocks-ca.key \
  -CAcreateserial -days 365 -extfile certs/san.ext -out certs/servicebus-local.crt 2>/dev/null
chmod 644 certs/*.crt certs/servicebus-local.key
echo "Certificados generados en $(pwd)/certs"
