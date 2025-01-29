import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
// import mkcert from 'vite-plugin-mkcert'

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
  }
})
