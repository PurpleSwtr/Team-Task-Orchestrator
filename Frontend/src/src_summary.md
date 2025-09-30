
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
  <div class="p-4 pl-7 ml-5 mt-10 bg-yellow-200 text-black rounded-lg w-80 max-w-full shadow-gray-300 shadow-xl">
    <TaskIcon class="w-5 h-5 inline mr-1 mb-1"></TaskIcon>
    <h1 class="text-lg font-semibold truncate inline">{{ props.tittle }}</h1>
    <p class="font-normal line-clamp-3">{{ props.task }}</p>
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

### 📄 `components/features/SideBar.vue`

```vue
<template>
  <!-- Используем тег aside для семантики и классы Tailwind CSS для стилизации -->
  <aside class="w-64 h-screen bg-gray-800 text-cyan-50 flex flex-col p-5">
    <!-- Логотип или название проекта -->
    <div class="text-2xl font-bold mb-10">
      To-Do-List
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

### 📄 `components/icons/AboutIcon.vue`

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

### 📄 `components/icons/HomeIcon.vue`

```vue
<template>
  <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
    <path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"></path>
    <polyline points="9 22 9 12 15 12 15 22"></polyline>
  </svg>
</template>
```

---

### 📄 `components/icons/LoginIcon.vue`

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

### 📄 `components/icons/TaskIcon.vue`

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

### 📄 `components/ui/AppButton.vue`

```vue
<template>
    <button class="bg-green-600 hover:bg-green-500 transition-colors rounded-xl px-3 py-2 text-white font-semibold">
    Создать задачу</button>   
</template>

<script setup lang="ts">

</script>

<style scoped>

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
      Это мой курсач.
    </p>
  </div>
</template>
```

---

### 📄 `views/HomePage.vue`

```vue
<template>
  <AppButton @click="addTask"></AppButton>
  <div v-for="cur_task in tasks">
    <CardTask 
      :tittle = "cur_task.tittle"
      :task = "cur_task.text_task"/>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import CardTask from '@/components/CardTask.vue'
import AppButton from '@/components/ui/AppButton.vue'

// 3. Создаем interface, который описывает объект задачи
interface Task {
  tittle: string;
  text_task: string;
  // в будущем сюда можно будет добавить id: number, completed: boolean и т.д.
}

// 4. Указываем, что `tasks` - это ref, содержащий МАССИВ объектов типа Task
const tasks = ref<Task[]>([])

// 5. Убираем неиспользуемый `event`, чтобы код был чище
function addTask() {
  console.log("Задача добавлена!")
  tasks.value.push({ 
    tittle: `Заголовок ${tasks.value.length + 1}`,
    text_task: `текст ${tasks.value.length + 1}`
  })
}
</script>

<style>
</style>
```

---

### 📄 `views/LogRegPage.vue`

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
