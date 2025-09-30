
---

### 📄 `App.vue`

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

### 📄 `main.ts`

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

### 📄 `assets/base.css`

```css

```

---

### 📄 `assets/main.css`

```css
@import './base.css';
@import 'tailwindcss';
```

---

### 📄 `components/CardTask.vue`

```vue
<template>
  <div class="p-4 pl-7 ml-5 mt-10 bg-yellow-200 text-black rounded-lg w-80 max-w-full">
    <h1 class="text-lg font-semibold truncate">{{ props.tittle }}</h1>
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

### 📄 `components/features/SideBar.vue`

```vue
<template>
  <!-- Используем тег aside для семантики и классы Tailwind CSS для стилизации -->
  <aside class="w-64 h-screen bg-gray-800 text-white flex flex-col p-5">
    <!-- Логотип или название проекта -->
    <div class="text-2xl font-bold mb-10">
      Мой Проект
    </div>

    <!-- Навигационное меню -->
    <nav>
      <ul>
        <li class="mb-4">
          <!-- RouterLink - это специальный компонент Vue Router для навигации -->
          <RouterLink 
            to="/" 
            class="flex items-center p-2 rounded-lg hover:bg-gray-700 transition-colors"
          >
            <!-- Здесь можно вставить иконку -->
            <span>Главная</span>
          </RouterLink>
        </li>
        <li>
          <RouterLink 
            to="/about" 
            class="flex items-center p-2 rounded-lg hover:bg-gray-700 transition-colors"
          >
            <span>О проекте</span>
          </RouterLink>
        </li>
        <li>
          <RouterLink 
            to="/login" 
            class="flex items-center p-2 rounded-lg hover:bg-gray-700 transition-colors"
          >
            <span>Войти</span>
          </RouterLink>
        </li>
      </ul>
    </nav>
  </aside>
</template>

<script setup lang="ts">
// Импортируем RouterLink для использования в шаблоне
import { RouterLink } from 'vue-router';
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

### 📄 `layouts/MainLayout.vue`

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

### 📄 `router/index.ts`

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

### 📄 `views/AboutPage.vue`

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

### 📄 `views/HomePage.vue`

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

### 📄 `views/LogRegPage.vue`

```vue
<template>
<input type="email">
<input type="password">
</template>

<script setup>

</script>
```
