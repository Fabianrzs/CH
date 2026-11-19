# GA2APP (React, CRA 4). node-sass 6 requiere Node <= 16; se usa amd64 para tomar su binario precompilado.
FROM node:16-bullseye AS build
WORKDIR /app
COPY package.json package-lock.json ./
# --legacy-peer-deps: react-text-mask declara peer react <= 16 (el proyecto usa 17), igual que en local con npm 6.
RUN npm ci --no-audit --no-fund --legacy-peer-deps
COPY . .
# Las URL de las APIs quedan fijas en el bundle: son las que ve el navegador (host), no la red de contenedores.
ARG REACT_APP_API_IDENTITY=http://localhost:5001/api/
ARG REACT_APP_API_GA2=http://localhost:5003/api/
ARG REACT_APP_API_BUA=http://localhost:5005/api/
ARG REACT_APP=http://localhost:3000/
ENV REACT_APP_API_IDENTITY=$REACT_APP_API_IDENTITY \
    REACT_APP_API_GA2=$REACT_APP_API_GA2 \
    REACT_APP_API_BUA=$REACT_APP_API_BUA \
    REACT_APP=$REACT_APP \
    GENERATE_SOURCEMAP=false \
    DISABLE_ESLINT_PLUGIN=true
RUN npx react-scripts build

FROM nginx:1.27-alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
