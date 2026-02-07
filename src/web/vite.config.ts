import { defineConfig, loadEnv } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), "");

  const apiTarget =
    process.env.API_PROXY_TARGET || env.API_PROXY_TARGET;

  if (!apiTarget) {
    throw new Error("API_PROXY_TARGET is not set");
  }

  console.log("Vite API proxy target =", apiTarget);

  return {
    plugins: [react()],
    build: {
      outDir: "dist",
      emptyOutDir: true,
    },
    server: {
      proxy: {
      "/api": {
        target: apiTarget,
        changeOrigin: true,
        secure: false,
      },
      "/health": {
        target: apiTarget,
        changeOrigin: true,
        secure: false,
      }
    }
    },
  };
});

