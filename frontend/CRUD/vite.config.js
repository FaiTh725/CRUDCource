import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
// import mkcert from 'vite-plugin-mkcert'

const sslKey = process.env.VITE_APP_SSL_KEY_FILE_PATH;
const sslSert = process.env.VITE_APP_SSL_CRT_FILE_PATH;

console.log(sslKey);
console.log(sslSert);

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    https: true,
    host: true,
    strictPort: true,
    port: 5173,
  },
  build: {
    outDir: "dist"
  },
  https: {
    key: sslKey,
    sert: sslSert
  }
})
