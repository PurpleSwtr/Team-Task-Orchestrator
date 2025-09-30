
---

### 📄 `.gitignore`

```gitignore
# Logs
logs
*.log
npm-debug.log*
yarn-debug.log*
yarn-error.log*
pnpm-debug.log*
lerna-debug.log*

node_modules
.DS_Store
dist
dist-ssr
coverage
*.local

/cypress/videos/
/cypress/screenshots/

# Editor directories and files
.vscode/*
!.vscode/extensions.json
.idea
*.suo
*.ntvs*
*.njsproj
*.sln
*.sw?

*.tsbuildinfo
```

---

### 📄 `.prettierrc.json`

```json
{
  "$schema": "https://json.schemastore.org/prettierrc",
  "semi": false,
  "singleQuote": true,
  "printWidth": 100
}
```

---

### 📄 `README.md`

```markdown
# Frontend

This template should help get you started developing with Vue 3 in Vite.

## Recommended IDE Setup

[VSCode](https://code.visualstudio.com/) + [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) (and disable Vetur).

## Type Support for `.vue` Imports in TS

TypeScript cannot handle type information for `.vue` imports by default, so we replace the `tsc` CLI with `vue-tsc` for type checking. In editors, we need [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) to make the TypeScript language service aware of `.vue` types.

## Customize configuration

See [Vite Configuration Reference](https://vite.dev/config/).

## Project Setup

```sh
npm install
```

### Compile and Hot-Reload for Development

```sh
npm run dev
```

### Type-Check, Compile and Minify for Production

```sh
npm run build
```

### Lint with [ESLint](https://eslint.org/)

```sh
npm run lint
```
```

---

### 📄 `env.d.ts`

```typescript
/// <reference types="vite/client" />
```

---

### 📄 `eslint.config.ts`

```typescript
import { globalIgnores } from 'eslint/config'
import { defineConfigWithVueTs, vueTsConfigs } from '@vue/eslint-config-typescript'
import pluginVue from 'eslint-plugin-vue'
import skipFormatting from '@vue/eslint-config-prettier/skip-formatting'

// To allow more languages other than `ts` in `.vue` files, uncomment the following lines:
// import { configureVueProject } from '@vue/eslint-config-typescript'
// configureVueProject({ scriptLangs: ['ts', 'tsx'] })
// More info at https://github.com/vuejs/eslint-config-typescript/#advanced-setup

export default defineConfigWithVueTs(
  {
    name: 'app/files-to-lint',
    files: ['**/*.{ts,mts,tsx,vue}'],
  },

  globalIgnores(['**/dist/**', '**/dist-ssr/**', '**/coverage/**']),

  pluginVue.configs['flat/essential'],
  vueTsConfigs.recommended,
  skipFormatting,
)
```

---

### 📄 `index.html`

```html
<!DOCTYPE html>
<html lang="">
  <head>
    <meta charset="UTF-8">
    <link rel="icon" href="/favicon.ico">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>ToDoApp</title>
  </head>
  <body>
    <div id="app"></div>
    <script type="module" src="/src/main.ts"></script>
  </body>
</html>
```

---

### 📄 `package.json`

```json
{
  "name": "frontend",
  "version": "0.0.0",
  "private": true,
  "type": "module",
  "engines": {
    "node": "^20.19.0 || >=22.12.0"
  },
  "scripts": {
    "dev": "vite",
    "build": "run-p type-check \"build-only {@}\" --",
    "preview": "vite preview",
    "build-only": "vite build",
    "type-check": "vue-tsc --build",
    "lint": "eslint . --fix",
    "format": "prettier --write src/"
  },
  "dependencies": {
    "pinia": "^3.0.3",
    "vue": "^3.5.18",
    "vue-router": "^4.5.1"
  },
  "devDependencies": {
    "@tailwindcss/vite": "^4.1.13",
    "@tsconfig/node22": "^22.0.2",
    "@types/node": "^22.16.5",
    "@vitejs/plugin-vue": "^6.0.1",
    "@vue/eslint-config-prettier": "^10.2.0",
    "@vue/eslint-config-typescript": "^14.6.0",
    "@vue/tsconfig": "^0.7.0",
    "eslint": "^9.31.0",
    "eslint-plugin-vue": "~10.3.0",
    "jiti": "^2.4.2",
    "npm-run-all2": "^8.0.4",
    "prettier": "3.6.2",
    "tailwindcss": "^4.1.13",
    "typescript": "~5.8.0",
    "vite": "^7.0.6",
    "vite-plugin-vue-devtools": "^8.0.0",
    "vue-tsc": "^3.0.4"
  }
}
```

---

### 📄 `tsconfig.app.json`

```json
{
  "extends": "@vue/tsconfig/tsconfig.dom.json",
  "include": ["env.d.ts", "src/**/*", "src/**/*.vue"],
  "exclude": ["src/**/__tests__/*"],
  "compilerOptions": {
    "tsBuildInfoFile": "./node_modules/.tmp/tsconfig.app.tsbuildinfo",

    "paths": {
      "@/*": ["./src/*"]
    }
  }
}
```

---

### 📄 `tsconfig.json`

```json
{
  "files": [],
  "references": [
    {
      "path": "./tsconfig.node.json"
    },
    {
      "path": "./tsconfig.app.json"
    }
  ],
  "compilerOptions": {
    "allowJs": true
  }
}
```

---

### 📄 `tsconfig.node.json`

```json
{
  "extends": "@tsconfig/node22/tsconfig.json",
  "include": [
    "vite.config.*",
    "vitest.config.*",
    "cypress.config.*",
    "nightwatch.conf.*",
    "playwright.config.*",
    "eslint.config.*"
  ],
  "compilerOptions": {
    "noEmit": true,
    "tsBuildInfoFile": "./node_modules/.tmp/tsconfig.node.tsbuildinfo",

    "module": "ESNext",
    "moduleResolution": "Bundler",
    "types": ["node"]
  }
}
```

---

### 📄 `vite.config.ts`

```typescript
import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'
import tailwindcss from '@tailwindcss/vite'
// https://vite.dev/config/
export default defineConfig({
  plugins: [vue(), vueDevTools(), tailwindcss()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
    },
  },
})
```

---

### 📄 `src/App.vue`

```vue
<script setup lang="ts">
import { RouterView } from 'vue-router'
</script>

<template>
  <!-- Здесь будет отображаться содержимое наших будущих страниц -->
  <RouterView />
</template>

<style scoped>
/* Стили пока оставим пустыми */
</style>
```

---

### 📄 `src/main.ts`

```typescript
import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'

const app = createApp(App)

app.use(createPinia())
app.use(router)

app.mount('#app')
```

---

### 📄 `src/assets/base.css`

```css

```

---

### 📄 `src/assets/main.css`

```css
@import './base.css';
@import 'tailwindcss';
```

---

### 📄 `src/components/CardTask.vue`

```vue
<template>
  <div class="p-4 pl-7 ml-5 mt-10 bg-yellow-200 text-black rounded-lg w-80 max-w-full shadow-gray-300 shadow-xl">
    <TaskIcon class="w-5 h-5 inline mr-1 mb-1"></TaskIcon>
    <h1 class="text-lg font-semibold truncate inline">{{ props.tittle }}</h1>
    <p class="font-light line-clamp-3">{{ props.task }}</p>
    <div class="flex items-center gap-2 mt-2">
      <input type="checkbox" />
      <span class="text-sm">Выполнено</span>
    </div>
  </div>
</template>
<script setup>
const props = defineProps({
  tittle: String,
  task: String,
})
import TaskIcon from '@/components/icons/TaskIcon.vue'
// name = "Задача"
// text = "Текст"
// list["task_1"] = {"name": name,"text": text}
// list["task_2"] = {"name": name,"text": text}
// list["task_3"] = {"name": name,"text": text}
// console.log(list)
</script>

<style></style>
```

---

### 📄 `src/components/features/SideBar.vue`

```vue
<template>
  <!-- Используем тег aside для семантики и классы Tailwind CSS для стилизации -->
  <aside class="w-64 h-screen bg-gray-800 text-cyan-50 flex flex-col p-5">
    <!-- Логотип или название проекта -->
    <div class="text-2xl font-bold mb-10">
      Мой Проект
    </div>
    <nav>
      <ul>
        <li v-for="item in items" class="mb-4">
          <RouterLink 
            :to="item.route_path" 
            class="flex items-center p-2 rounded-lg hover:bg-gray-700 transition-colors"
          >
            <component :is="iconMap[item.icon]" class="w-6 h-6" />
            <span class="ml-3">{{item.message}}</span>
          </RouterLink>
        </li>
      </ul>
    </nav>
  </aside>
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router';
import { ref } from 'vue';
// 👇 1. Импортируй тип Component из vue
import { type Component } from 'vue'; 
import HomeIcon from '@/components/icons/HomeIcon.vue';
import LoginIcon from '@/components/icons/LoginIcon.vue';
import AboutIcon from '@/components/icons/AboutIcon.vue';

// 👇 2. Укажи тип для iconMap
const iconMap: { [key: string]: Component } = {
  home: HomeIcon,
  login: LoginIcon,
  about: AboutIcon
};

let items = ref([
  {message: "Главная", route_path: "/", icon: "home"},
  {message: "Войти", route_path: "/login", icon: "login"},
  {message: "О проекте", route_path: "/about", icon: "about"},
]);

</script>

<style>
/* Стили для активной ссылки */
.router-link-exact-active {
  background-color: #4A5568; /* bg-gray-700 */
  font-weight: bold;
}
</style>
```

---

### 📄 `src/components/icons/AboutIcon.vue`

```vue
<template>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
 <circle cx="12" cy="12" r="10"></circle>
 <line x1="12" y1="16" x2="12" y2="12"></line>
 <line x1="12" y1="8" x2="12.01" y2="8"></line>
</svg>
</template>
```

---

### 📄 `src/components/icons/HomeIcon.vue`

```vue
<template>
  <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
    <path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"></path>
    <polyline points="9 22 9 12 15 12 15 22"></polyline>
  </svg>
</template>
```

---

### 📄 `src/components/icons/LoginIcon.vue`

```vue
<template>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
 <path d="M15 3h4a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2h-4"></path>
 <polyline points="10 17 15 12 10 7"></polyline>
 <line x1="15" y1="12" x2="3" y2="12"></line>
</svg>
</template>
```

---

### 📄 `src/components/icons/TaskIcon.vue`

```vue
<template>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
  <path d="M16 4h2a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h2"></path>
  <rect x="8" y="2" width="8" height="4" rx="1" ry="1"></rect>
  <line x1="8" y1="12" x2="16" y2="12"></line>
  <line x1="8" y1="16" x2="13" y2="16"></line>
</svg>
</template>
```

---

### 📄 `src/layouts/MainLayout.vue`

```vue
<template>
  <div class="flex h-screen">
    <!-- 1. Боковая панель (всегда на месте) -->
    <SideBar /> 

    <!-- 2. Основная область контента -->
    <main class="flex-1 p-8 overflow-y-auto">
      <!-- А вот сюда будут вставляться уже НАШИ СТРАНИЦЫ (HomePage и др.) -->
      <RouterView />
    </main>
  </div>
</template>

<script setup>
// Импортируем компонент боковой панели, который мы скоро создадим
import SideBar from '@/components/features/SideBar.vue' 
</script>
```

---

### 📄 `src/router/index.ts`

```typescript
import { createRouter, createWebHistory } from 'vue-router'
import MainLayout from '../layouts/MainLayout.vue'
import HomePage from '../views/HomePage.vue'
import AboutPage from '../views/AboutPage.vue'
import LogRegPage from '../views/LogRegPage.vue'


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: MainLayout, // 👈 2. Устанавливаем MainLayout как обертку
      children: [ // 👈 3. Все дочерние маршруты будут отображаться внутри MainLayout
        {
          path: '', // Пустой путь для главной страницы ('/')
          name: 'home',
          component: HomePage,
        },
        {
          path: 'about', // Путь для страницы "О проекте" ('/about')
          name: 'about',  
          component: AboutPage,
        },
                {
          path: 'login', 
          name: 'login',  
          component: LogRegPage,
        },

      ],
    },
  ],
})

export default router
```

---

### 📄 `src/views/AboutPage.vue`

```vue
<template>
  <div>
    <h1 class="text-3xl font-bold">О проекте</h1>
    <p class="mt-4">
      Это простое приложение для демонстрации работы Sidebar и Vue Router.
    </p>
  </div>
</template>

<script setup lang="ts">
// Для этой простой страницы логика не нужна
</script>
```

---

### 📄 `src/views/HomePage.vue`

```vue
<template>
  <CardTask 
    :tittle = "tittle"
    :task = "task"/>
</template>

<script setup>
import { ref } from 'vue'
import CardTask from '@/components/CardTask.vue'

// 3. Объявляем переменные, чтобы Vue их "увидел"
const tittle = ref("Мое первое задание");
const task = ref("Нужно сделать X и Y.");
</script>

<style>
</style>
```

---

### 📄 `src/views/LogRegPage.vue`

```vue
<template>
  <div class="min-h-screen flex items-center justify-center">
    <div class="flex flex-col items-center w-full max-w-md">
      <h1 class="text-3xl font-bold mb-6">Страница входа</h1>
      <div class="flex flex-col gap-4 w-full">
        <input 
          type="email" 
          placeholder="Email" 
          class="p-2 border rounded"
        >
        <input 
          type="password" 
          placeholder="Пароль" 
          class="p-2 border rounded"
        >
        <button class="p-2 bg-blue-500 text-white rounded hover:bg-blue-600">
          Войти
        </button>
      </div>
    </div>
  </div>
</template>
```
