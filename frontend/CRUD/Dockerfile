# FROM node:18-alpine

# WORKDIR /usr/app

# COPY package*.json .
# RUN npm install

# COPY . .

# RUN npm run build

# EXPOSE 5173

# ENTRYPOINT [ "npm", "run", "preview" ]

FROM node:18-alpine as build

ARG VITE_APP_SSL_CRT_FILE_PATH
ARG VITE_APP_SSL_KEY_FILE_PATH

ENV VITE_APP_SSL_CRT_FILE_PATH=${VITE_APP_SSL_CRT_FILE_PATH}
ENV VITE_APP_SSL_KEY_FILE_PATH=${VITE_APP_SSL_KEY_FILE_PATH}

WORKDIR /usr/app

COPY package*.json .

RUN npm install

COPY . .

RUN npm run build

FROM nginx

WORKDIR /usr/share/nginx/html
RUN rm -rf ./*

COPY --from=build /usr/app/dist .

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

ENTRYPOINT [ "nginx", "-g", "daemon off;" ]
