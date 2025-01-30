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

ARG SERVER_CONF
ARG DEFAULT_CONF

COPY ${DEFAULT_CONF} /etc/nginx/nginx.conf
COPY ${SERVER_CONF} /etc/nginx/conf.d/default.conf

# Add ssl sertificates
RUN mkdir -p /etc/ssl/cert/
RUN mkdir -p /etc/ssl/private/

EXPOSE 80
EXPOSE 5173

ENTRYPOINT [ "nginx", "-g", "daemon off;" ]
