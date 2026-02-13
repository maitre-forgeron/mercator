import { defineConfig, loadEnv } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), "");
  const isDev = mode === "development";

  const apiTarget = process.env.API_PROXY_TARGET || env.API_PROXY_TARGET;

  if (isDev && !apiTarget) {
    throw new Error("API_PROXY_TARGET is not set");
  }

  console.log("Vite API proxy target =", apiTarget);

  const serverOptions = isDev ? 
  {
      proxy: {
      "/api": {
        target: apiTarget,
        changeOrigin: true,
        secure: false,
        rewrite: (path: any) => path.replace(/^\/api/, "")
      },
      "/health": {
        target: apiTarget,
        changeOrigin: true,
        secure: false,
      }
    }
  } : undefined;

  return {
    plugins: [react()],
    build: {
      outDir: "dist",
      emptyOutDir: true,
    },
    server: serverOptions,
  };
});

