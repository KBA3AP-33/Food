# FROM node:18.17.1
# FROM node:18-alpine
FROM node:19.5.0-alpine

ENV APP_ROOT /web

WORKDIR ${APP_ROOT}
ADD . ${APP_ROOT}

RUN npm ci

RUN npm run build
CMD node .output/server/index.mjs