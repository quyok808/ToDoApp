FROM node:20-alpine

WORKDIR /app

COPY . .

# Thêm ARG cho API base URL từ docker-compose
ARG VITE_APP_API_BASE_URL
ENV VITE_APP_API_BASE_URL=$VITE_APP_API_BASE_URL

RUN npm install
RUN npm run build

# Dùng preview server để phục vụ Vite app
RUN npm install -g serve
EXPOSE 5173

CMD ["serve", "-s", "dist", "-l", "5173"]
