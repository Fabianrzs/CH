#!/usr/bin/env python3
"""Prueba de humo GA2 → MockIntegrations.

Llama endpoints REALES de la API GA2 (arrancada con ASPNETCORE_ENVIRONMENT=Mocks) y comprueba en el
registro del mock (/_mock/journal) que cada integración recibió la llamada y con qué resultado.

Uso:
  python3 smoke.py --ga2 http://127.0.0.1:5102 --mock http://127.0.0.1:5080 --jwt-key <clave de prueba>

--jwt-key debe ser el mismo valor que se pasó a GA2 en JwtOptions__Key (clave de prueba, no la real):
GA2 sólo valida la firma HS256 del token (JwtExtensions.cs: ValidateIssuer/ValidateAudience = false).
Sin dependencias externas: sólo la biblioteca estándar de Python.
"""
import argparse
import base64
import hashlib
import hmac
import json
import sys
import time
import urllib.error
import urllib.request


def jwt(key: str) -> str:
    def b64(data: bytes) -> str:
        return base64.urlsafe_b64encode(data).rstrip(b"=").decode()

    header = b64(json.dumps({"alg": "HS256", "typ": "JWT"}).encode())
    now = int(time.time())
    payload = b64(json.dumps({"sub": "mock-smoke", "unique_name": "mock-smoke", "nbf": now - 60, "exp": now + 3600}).encode())
    signature = b64(hmac.new(key.encode(), f"{header}.{payload}".encode(), hashlib.sha256).digest())
    return f"{header}.{payload}.{signature}"


def call(method, url, body=None, token=None):
    data = None if body is None else json.dumps(body).encode()
    request = urllib.request.Request(url, data=data, method=method)
    request.add_header("Accept", "application/json")
    if data is not None:
        request.add_header("Content-Type", "application/json")
    if token:
        request.add_header("Authorization", f"Bearer {token}")
    try:
        with urllib.request.urlopen(request, timeout=60) as response:
            return response.status, response.read().decode()
    except urllib.error.HTTPError as error:
        return error.code, error.read().decode()


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--ga2", default="http://127.0.0.1:5102")
    parser.add_argument("--mock", default="http://127.0.0.1:5080")
    parser.add_argument("--jwt-key", required=True)
    args = parser.parse_args()
    token = jwt(args.jwt_key)

    # (nombre, método, ruta GA2, body, requiere token, proveedor esperado, operación esperada)
    cases = [
        ("TusDatos ConsultarCliente", "GET", "/api/integrations/tusdatos/customer/1020304050/document-type/CC?checkHistory=true", None, False, "TusDatos", "ConsultarCliente"),
        ("TusDatos ConsultarVigia", "GET", "/api/integrations/tusdatos/vigia/customer/1000000666/document-type/CC", None, False, "TusDatos", "ConsultarVigia"),
        ("TusDatos 404 → Data=null", "GET", "/api/integrations/tusdatos/customer/1000000404/document-type/CC?checkHistory=true", None, False, "TusDatos", "ConsultarCliente"),
        ("Dodo Form_GetParameters", "POST", "/api/integrations/dodo/forms/parameters", {"formCode": "83"}, False, "Dodo", "Form_GetParameters"),
        ("Vigía F_VERIFICATERCERO", "POST", "/api/credito/verificacionTercero", {"Pe_Dato": "1000000666", "Pe_TipoVerificacion": "1", "Pe_Porcentaje": ""}, True, "Vigia", "F_VERIFICATERCERO"),
        ("Fénix ValidarBiometria", "GET", "/api/credito/validarBiometria?request=1020304050", None, True, "Fenix", "ValidarBiometria"),
        ("Seven InsertarGnTerce", "POST", "/api/credito/InsertarGnTerce", {"Emp_codi": 1, "Tip_codi": 1, "Ter_coda": "1020304050"}, True, "Seven", "InsertarGnTerce"),
    ]

    call("DELETE", f"{args.mock}/_mock/journal")
    failures = 0
    for name, method, path, body, needs_token, provider, operation in cases:
        status, text = call(method, args.ga2 + path, body, token if needs_token else None)
        _, journal = call("GET", f"{args.mock}/_mock/journal?provider={provider}&operation={operation}&take=1")
        entries = json.loads(journal)
        reached = bool(entries)
        ok = status == 200 and reached
        failures += 0 if ok else 1
        detail = f"mock {entries[0]['statusCode']} escenario={entries[0]['scenario']}" if reached else "el mock NO recibió la llamada"
        print(f"[{'OK ' if ok else 'ERR'}] {name}: GA2 HTTP {status} · {detail}")
        print(f"      respuesta GA2: {text[:220]}")
        call("DELETE", f"{args.mock}/_mock/journal")

    print(f"\n{len(cases) - failures}/{len(cases)} llamadas GA2 → mock correctas")
    return 1 if failures else 0


if __name__ == "__main__":
    sys.exit(main())
