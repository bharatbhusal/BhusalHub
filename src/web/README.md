# BhusalHub Web

React SPA — the frontend service for BhusalHub.

## Stack

- React 18 + TypeScript
- Vite (dev server + build)
- Tailwind CSS 3 + shadcn/ui components
- Redux Toolkit + redux-persist (state management)
- react-router-dom (routing)

## Quick start

```bash
npm install
npm run dev
```

Dev server at `http://localhost:5173`, proxies `/api` to `http://localhost:5000`.

## Build

```bash
npm run build
```

Output in `dist/`.

## Project structure

```
src/
  api/          — HTTP client
  components/   — UI components (shadcn)
  pages/        — route pages (Login, Signup, Home)
  store/        — Redux store, slices, hooks
  styles/       — globals.css (Tailwind directives)
  types/        — TypeScript interfaces
```
