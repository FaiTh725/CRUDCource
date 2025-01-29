FROM node:18-alpine as build

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

# Add ssl sertificates
RUN mkdir -p /etc/ssl/cert/
RUN mkdir -p /etc/ssl/private/

# ARG VITE_APP_SSL_CRT_FILE_PATH
# ARG VITE_APP_SSL_KEY_FILE_PATH

# COPY  ${VITE_APP_SSL_CRT_FILE_PATH} /etc/ssl/cert/crt.crt
# COPY  ${VITE_APP_SSL_KEY_FILE_PATH} /etc/ssl/private/key.key

EXPOSE 80
EXPOSE 433

ENTRYPOINT [ "nginx", "-g", "daemon off;" ]
