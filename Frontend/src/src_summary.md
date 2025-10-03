
---

### 📄 `App.vue`

```vue
<script setup lang="ts">
import { RouterView } from 'vue-router'
</script>

<template>
  <RouterView class="app"/>
</template>

<style scoped>
/* Стили пока оставим пустыми */
.app{
background-color: #e8e8e8;
}
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
  <div class="p-4 pl-7 ml-5 mt-10 bg-gray-100 text-black relative duration-500 rounded-lg w-80 max-w-full hover:-translate-y-2 shadow-gray-300 hover:shadow-xl shadow-md hover:cursor-pointer hover:scale-103">
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
import TaskIcon from '@/components/icons/TaskIcon.vue'

const props = defineProps({
  tittle: String,
  task: String,
})
</script>

<style></style>
```

---

### 📄 `components/features/LoginForm.vue`

```vue

```

---

### 📄 `components/features/RegisterForm.vue`

```vue

```

---

### 📄 `components/features/SideBar.vue`

```vue
<template>
  <aside class="w-64 h-screen bg-gray-800 text-cyan-50 flex flex-col p-5">
    <div class="text-2xl font-bold mb-10 text-yellow-500">
      Курсач24
    </div>
    
    <nav class="flex flex-col flex-1">
      <ul class="flex-1">
        <li v-for="item in items" :key="item.route_path" class="mb-4">
          <MenuButton 
            :message="item.message" 
            :icon="item.icon" 
            :route_path="item.route_path"
          ></MenuButton>
        </li>
      </ul>
      <div class="border-t border-gray-500 my-4"></div>
      <ul>
        <li>
          <MenuButton v-for="b_item in bottom_items" 
            class="mb-4"
            @click="handleClick(b_item)"
            :key="b_item.route_path" 
            :message="b_item.message" 
            :icon="b_item.icon" 
            :route_path="b_item.route_path"
          ></MenuButton>
        </li>
      </ul>
    </nav>
  </aside>
</template>

<script setup lang="ts">
  import { ref } from 'vue';
  import MenuButton from '../ui/MenuButton.vue';
  import type { MenuItem } from '@/types'
  let items = ref([
    {message: "Войти", route_path: "/login", icon: "login"},
    {message: "Главная", route_path: "/", icon: "home"},
    {message: "Проекты", route_path: "/projects", icon: "projects"},
    {message: "Команды", route_path: "/teams", icon: "teams"},
    {message: "Пользователи", route_path: "/users", icon: "users"},
    {message: "Генератор", route_path: "/generator", icon: "generator"},
    {message: "Статистика", route_path: "/satistics", icon: "satistics"},
    {message: "О проекте", route_path: "/about", icon: "about"},

  ]);

  let bottom_items = ref([
    {message: "Настройки", route_path: "/settings", icon: "settings"},
    // Для выхода нужно внести отдельный путь на главную страничку которой пока нет
    {message: "Выйти", route_path: "/", icon: "logout"},
  ])

  function handleClick(clickedItem: MenuItem){
    if (clickedItem.message === 'Выйти') {
      console.log("Выходим!")
      // Тут логика выхода из аккаунта
    }
  };

</script>

<style>
.router-link-exact-active {
  background-color: #4A5568;
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

### 📄 `components/icons/GeneratorIcon.vue`

```vue
<template>
    <!-- <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" stroke="currentColor" stroke-width="1.5" d="M6 3a2 2 0 0 0-2 2v11h2v1c0 .55.45 1 1 1h1c.55 0 1-.45 1-1v-1h6v1c0 .55.45 1 1 1h1c.55 0 1-.45 1-1v-1h2V5a2 2 0 0 0-2-2H6m6 4V5h6v2h-6m0 2h6v2h-6V9M8 5v4h2l-3 6v-4H5l3-6m14 15v2H2v-2h20Z"/>
    </svg> -->
    <svg class="icon-rotate" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" stroke-width="0" ></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <g clip-path="url(#clip0_429_11034)"> <path d="M6 14L13 2V10H18L11 22V14H6Z" stroke="currentColor" stroke-width="2" stroke-linejoin="round"></path> </g> <defs> <clipPath id="clip0_429_11034"> <rect width="24" height="24" fill="white"></rect> </clipPath> </defs> </g></svg>
</template>

<style>
.icon-rotate {
        transform: rotate(15deg);
      }
</style>
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

### 📄 `components/icons/LogoutIcon.vue`

```vue
<template>
    <svg  xmlns="http://www.w3.org/2000/svg"  width="24"  height="24"  viewBox="0 0 24 24"  fill="none"  stroke="currentColor"  stroke-width="2"  stroke-linecap="round"  stroke-linejoin="round"  class="icon icon-tabler icons-tabler-outline icon-tabler-logout"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M14 8v-2a2 2 0 0 0 -2 -2h-7a2 2 0 0 0 -2 2v12a2 2 0 0 0 2 2h7a2 2 0 0 0 2 -2v-2" /><path d="M9 12h12l-3 -3" /><path d="M18 15l3 -3" /></svg>
</template>
```

---

### 📄 `components/icons/ProjectsIcon.vue`

```vue
<template>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
  <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"></path>
  <polyline points="14 2 14 8 20 8"></polyline>
  <line x1="16" y1="13" x2="8" y2="13"></line>
  <line x1="16" y1="17" x2="8" y2="17"></line>
  <polyline points="10 9 9 9 8 9"></polyline>
</svg>
</template>
```

---

### 📄 `components/icons/SettingsIcon.vue`

```vue
<template>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="3"></circle><path d="M19.4 15a1.65 1.65 0 0 0 .33 1.82l.06.06a2 2 0 0 1 0 2.83 2 2 0 0 1-2.83 0l-.06-.06a1.65 1.65 0 0 0-1.82-.33 1.65 1.65 0 0 0-1 1.51V21a2 2 0 0 1-2 2 2 2 0 0 1-2-2v-.09A1.65 1.65 0 0 0 9 19.4a1.65 1.65 0 0 0-1.82.33l-.06.06a2 2 0 0 1-2.83 0 2 2 0 0 1 0-2.83l.06-.06a1.65 1.65 0 0 0 .33-1.82 1.65 1.65 0 0 0-1.51-1H3a2 2 0 0 1-2-2 2 2 0 0 1 2-2h.09A1.65 1.65 0 0 0 4.6 9a1.65 1.65 0 0 0-.33-1.82l-.06-.06a2 2 0 0 1 0-2.83 2 2 0 0 1 2.83 0l.06.06a1.65 1.65 0 0 0 1.82.33H9a1.65 1.65 0 0 0 1-1.51V3a2 2 0 0 1 2-2 2 2 0 0 1 2 2v.09a1.65 1.65 0 0 0 1 1.51 1.65 1.65 0 0 0 1.82-.33l.06-.06a2 2 0 0 1 2.83 0 2 2 0 0 1 0 2.83l-.06.06a1.65 1.65 0 0 0-.33 1.82V9a1.65 1.65 0 0 0 1.51 1H21a2 2 0 0 1 2 2 2 2 0 0 1-2 2h-.09a1.65 1.65 0 0 0-1.51 1z"></path></svg>
</template>
```

---

### 📄 `components/icons/StaticticsIcon.vue`

```vue
<template>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M24 3.875l-6 1.221 1.716 1.708-5.351 5.358-3.001-3.002-7.336 7.242 1.41 1.418 5.922-5.834 2.991 2.993 6.781-6.762 1.667 1.66 1.201-6.002zm0 17.125v2h-24v-22h2v20h22z"/></svg>
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

### 📄 `components/icons/TeamsIcon.vue`

```vue
<template>
    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
<path d="M17 10H19C20.1046 10 21 9.10457 21 8V6C21 4.89543 20.1046 4 19 4H17C15.8954 4 15 4.89543 15 6V8C15 9.10457 15.8954 10 17 10ZM5 10H7C8.10457 10 9 9.10457 9 8V6C9 4.89543 8.10457 4 7 4H5C3.89543 4 3 4.89543 3 6V8C3 9.10457 3.89543 10 5 10ZM11 20H13C14.1046 20 15 19.1046 15 18V16C15 14.8954 14.1046 14 13 14H11C9.89543 14 9 14.8954 9 16V18C9 19.1046 9.89543 20 11 20ZM7 10V14H9M17 10V14H15" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
</svg>

</template>
```

---

### 📄 `components/icons/UsersIcon.vue`

```vue
<template>
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
  <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path>
  <circle cx="9" cy="7" r="4"></circle>
  <path d="M23 21v-2a4 4 0 0 0-3-3.87"></path>
  <path d="M16 3.13a4 4 0 0 1 0 7.75"></path>
</svg>
</template>
```

---

### 📄 `components/ui/AppButton.vue`

```vue
<template>
    <button :disabled="props.statusLoading" 
    class="cursor-pointer 
    bg-green-600 
    hover:bg-green-700 
    transition-colors 
    rounded-xl 
    px-5 py-2 
    text-white 
    font-semibold 
    fade 
    disabled:bg-amber-500
    disabled:cursor-default 
    shadow-md">
    <span v-if="!props.statusLoading">{{props.message}}</span>
    <span v-else class="inline-flex items-center gap-x-2">
        Загрузка
        <LoadingCircle class="text-base " /> 
    </span>
    </button>

</template>

<script setup lang="ts">
    import LoadingCircle from './LoadingCircle.vue';
    import { ref } from 'vue';

    const props = defineProps<{
        message: string
        statusLoading?: boolean
    }>()

</script>

<style scoped>

</style>
```

---

### 📄 `components/ui/LoadingCircle.vue`

```vue
<template>
    <div class="loader"></div>
</template>

<style scoped>
.loader {
  /* 
    Мы привязываем размер лоадера к размеру шрифта родителя (кнопки).
    "width: 1em" означает, что ширина контентной части лоадера будет равна
    размеру шрифта. Например, для font-size: 16px ширина будет 16px.
  */
  width: 1em;
  aspect-ratio: 1;
  border-radius: 50%;
  
  /*
    Теперь самое главное: мы восстанавливаем оригинальные пропорции.
    В вашем коде соотношение рамки к ширине было 8px / 50px = 0.16.
    Мы сохраняем это соотношение, используя единицы em.
    `currentColor` автоматически возьмет цвет текста у родителя (в вашем случае - белый).
  */
  border: 0.16em solid currentColor;

  animation:
    l20-1 0.8s infinite linear alternate,
    l20-2 1.6s infinite linear;
}

/* Анимации не меняются, так как они работают с процентами от размера элемента */
@keyframes l20-1 {
   0%    {clip-path: polygon(50% 50%,0       0,  50%   0%,  50%    0%, 50%    0%, 50%    0%, 50%    0% )}
   12.5% {clip-path: polygon(50% 50%,0       0,  50%   0%,  100%   0%, 100%   0%, 100%   0%, 100%   0% )}
   25%   {clip-path: polygon(50% 50%,0       0,  50%   0%,  100%   0%, 100% 100%, 100% 100%, 100% 100% )}
   50%   {clip-path: polygon(50% 50%,0       0,  50%   0%,  100%   0%, 100% 100%, 50%  100%, 0%   100% )}
   62.5% {clip-path: polygon(50% 50%,100%    0, 100%   0%,  100%   0%, 100% 100%, 50%  100%, 0%   100% )}
   75%   {clip-path: polygon(50% 50%,100% 100%, 100% 100%,  100% 100%, 100% 100%, 50%  100%, 0%   100% )}
   100%  {clip-path: polygon(50% 50%,50%  100%,  50% 100%,   50% 100%,  50% 100%, 50%  100%, 0%   100% )}
}
@keyframes l20-2 { 
  0%    {transform:scaleY(1)  rotate(0deg)}
  49.99%{transform:scaleY(1)  rotate(135deg)}
  50%   {transform:scaleY(-1) rotate(0deg)}
  100%  {transform:scaleY(-1) rotate(-135deg)}
}
</style>
```

---

### 📄 `components/ui/MenuButton.vue`

```vue
<template>
    <RouterLink 
        :to="props.route_path" 
        class="flex items-center p-2 rounded-lg hover:bg-gray-900 transition-colors hover:text-yellow-600 :text-yellow-500"
    >
        <component :is="iconMap[props.icon]" class="w-6 h-6" />
        <span class="ml-3">{{props.message}}</span>
    </RouterLink>    
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router';

import { ref, type Component } from 'vue'; 
import HomeIcon from '@/components/icons/HomeIcon.vue';

import LoginIcon from '@/components/icons/LoginIcon.vue';
import LogoutIcon from '@/components/icons/LogoutIcon.vue';
import SettingsIcon from '../icons/SettingsIcon.vue';

import AboutIcon from '@/components/icons/AboutIcon.vue';
import GeneratorIcon from '../icons/GeneratorIcon.vue';
import StaticticsIcon from '../icons/StaticticsIcon.vue';

import ProjectsIcon from '../icons/ProjectsIcon.vue';
import TeamsIcon from '../icons/TeamsIcon.vue';
import UsersIcon from '../icons/UsersIcon.vue';

// const AppColor = ref("#fbbf24")

const iconMap: { [key: string]: Component } = {
  home: HomeIcon,
  login: LoginIcon,
  about: AboutIcon,
  generator: GeneratorIcon,
  settings: SettingsIcon,
  logout: LogoutIcon,
  projects: ProjectsIcon,
  teams: TeamsIcon,
  users: UsersIcon,
  statistics: StaticticsIcon,

};

const props = defineProps<{
    icon: string
    message: string
    route_path: string
    }>()
</script>

<style>
.router-link-exact-active {
  background-color: #4A5568;
  font-weight: bold;
}
.router-link-exact-active span {
  color: #f8c146; 
}

.router-link-exact-active svg {
  color: #f8c146; 
}
</style>
```

---

### 📄 `layouts/MainLayout.vue`

```vue
<template>
  <div class="flex h-screen">
    
    <SideBar /> 

    <main class="flex-1 p-8 overflow-y-auto">
      <RouterView />
    </main>

</div>
</template>

<script setup>
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
import SettingsPage from '@/views/SettingsPage.vue'

import LoginPage from '@/views/auth/LoginPage.vue'
import RegisterPage from '@/views/auth/RegisterPage.vue'

import GeneratorPage from '../views/GeneratorPage.vue'
import StatisticsPage from '@/views/StatisticsPage.vue'

import TeamsPage from '@/views/TeamsPage.vue'
import ProjectsPage from '@/views/ProjectsPage.vue'
import UsersPage from '@/views/UsersPage.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: MainLayout, 
      children: [ 
        {
          path: '', 
          name: 'home',
          component: HomePage,
        },
        {
          path: 'about',
          name: 'about',  
          component: AboutPage,
        },
        {
          path: 'login', 
          name: 'login',  
          component: LoginPage,
        },
        {
          path: 'register', 
          name: 'register',  
          component: RegisterPage,
        },
        {
          path: 'generator', 
          name: 'generator',  
          component: GeneratorPage,
        },
        {
          path: 'settings', 
          name: 'settings',  
          component: SettingsPage,
        },
        {
          path: 'projects', 
          name: 'projects',  
          component: ProjectsPage,
        },
        {
          path: 'teams', 
          name: 'teams',  
          component: TeamsPage,
        },
        {
          path: 'users', 
          name: 'users',  
          component: UsersPage,
        },
        {
          path: 'statistics', 
          name: 'statistics',  
          component: StatisticsPage,
        },
      ],
    },
  ],
})

export default router
```

---

### 📄 `types/index.ts`

```typescript
export interface MenuItem {
  message: string;
  icon: string;
  route_path?: string; 
}

export interface Task {
  id: number;
  tittle: string;
  text_task: string;
}
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

### 📄 `views/GeneratorPage.vue`

```vue
<template>
    <!-- <span>Таблица: </span> -->
    <select v-model="selected" class="border rounded-md mr-4 py-1 bg-gray-100 border-gray-400">
        <option disabled value="">Выберите таблицу</option>
        <option v-for="table in tables">{{table}}</option>
    </select>

    <input type="number" class="border rounded-md mr-4 py-1 bg-gray-100 border-gray-400 outline-0" placeholder="Количество генераций">


    <AppButton message='Сгенерировать' @click="GenStart"></AppButton>

</template>

<script setup lang="ts">
import AppButton from '@/components/ui/AppButton.vue';
import {ref} from 'vue'
let selected = ref('')
const tables = ref(['Задачи', 'Пользователи', 'Команды', 'Роли'])
function GenStart() {
// Будем отправлять в апишку данные наших плейсхолдеорв
};

</script>

<style scoped>

</style>
```

---

### 📄 `views/HomePage.vue`

```vue
<template>
  <AppButton message='Создать задачу' :statusLoading="buttonLoading" @click="addTask"></AppButton>

  <TransitionGroup name="fade" tag="div" class="grid grid-cols-3">
    <CardTask
      v-for="cur_task in tasks"
      :key="cur_task.id"
      :tittle="cur_task.tittle"
      :task="cur_task.text_task"
    />
  </TransitionGroup>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import CardTask from '@/components/CardTask.vue'
import AppButton from '@/components/ui/AppButton.vue'

import type { Task } from '@/types'


const tasks = ref<Task[]>([])
const buttonLoading = ref(false)
function addTask(event: Event) {
  console.log("Задача добавлена!")
  tasks.value.push({
    id: Date.now(), 
    tittle: `Заголовок ${tasks.value.length + 1}`,
    text_task: `текст ${tasks.value.length + 1}`
  })
  // buttonLoading.value = !buttonLoading.value
}
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
```

---

### 📄 `views/ProjectsPage.vue`

```vue
<template>

</template>

<script setup lang="ts">

</script>

<style scoped>
    
</style>
```

---

### 📄 `views/SettingsPage.vue`

```vue
<template></template>
```

---

### 📄 `views/StatisticsPage.vue`

```vue
<template></template>
```

---

### 📄 `views/TeamsPage.vue`

```vue
<template>

</template>

<script setup lang="ts">

</script>

<style scoped>
    
</style>
```

---

### 📄 `views/UsersPage.vue`

```vue
<template>

</template>

<script setup lang="ts">

</script>

<style scoped>
    
</style>
```

---

### 📄 `views/auth/LoginPage.vue`

```vue
<template>
  <div class="min-h-screen flex items-center justify-center pb-50">
    <div class="flex flex-col items-center w-full max-w-md bg-gray-100 pt-30 rounded-3xl shadow-2xl shadow-gray-300">
      <h1 class="text-3xl font-bold mb-6 text-gray-700">Вход в аккаунт</h1>
      <div class="flex flex-col gap-4 w-full">
        <input 
          type="email" 
          placeholder="Email" 
          class="bg-white p-2 mx-20 border-2 rounded-md border-gray-300 border-b-green-600 outline-blue-600"
          v-model="email"
        >
        <input 
          type="password" 
          placeholder="Пароль" 
          class="bg-white p-2 mx-20 border-2 rounded-md border-gray-300 border-b-green-600 outline-blue-600"
          v-model="password"
        >
        <AppButton @click="tryLogin":statusLoading="buttonLoading" message="Войти" class="mx-40 mb-10"></AppButton>
        <div class="text-center">
        <p class="inline">У вас нет аккаунта? </p>
        <RouterLink 
        to="/register">
        <button class="inline text-blue-500 cursor-pointer underline-offset-2 hover:underline hover:text-blue-700">Зарегистрироваться</button>
        
        </RouterLink>
      </div>
        <div class="pb-15"></div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import {ref} from 'vue'
  import { RouterLink } from 'vue-router';
  import AppButton from '@/components/ui/AppButton.vue';
  import axios from 'axios';

  const password = ref('')
  const email = ref('')
  const buttonLoading = ref(false)

  const tryLogin = async () => {
    try {
      buttonLoading.value = !buttonLoading.value
      console.log('click');
      const url = `http://localhost:8080/api/Auth/login`;
      console.log(url, {
        email: email.value,
        password: password.value
      });
      // Правильно передаем данные в теле запроса
      const response = await axios.post(url, {
        email: email.value,
        password: password.value
      });
      console.log(response)
    } catch (error) {
      // Обрабатываем различные типы ошибок
      console.error('Ошибка при входе:', error);
      
    }
  };
    
</script>

<style>

</style>
```

---

### 📄 `views/auth/RegisterPage.vue`

```vue
<template>
  <div class="min-h-screen flex items-center justify-center pb-50">
    <div class="flex flex-col items-center w-full max-w-md bg-gray-100 pt-30 rounded-3xl shadow-2xl shadow-gray-300">
      <h1 class="text-3xl font-bold mb-6 text-gray-700">Регистрация аккаунта</h1>
      <div class="flex flex-col gap-4 w-full">
        <input 
          type="email" 
          placeholder="Email" 
          class="bg-white p-2 mx-20 border-2 rounded-md border-gray-300 border-b-green-600 outline-blue-600"
          v-model="email"
        >
        <input 
          type="password" 
          placeholder="Пароль" 
          class="bg-white p-2 mx-20 border-2 rounded-md border-gray-300 border-b-green-600 outline-blue-600"
          v-model="password"
        >
        <AppButton @click="tryLogin" :status-loading="buttonLoading" message="Войти" class="mx-40 mb-10"></AppButton>
        <div class="text-center">
        <p class="inline">У вас уже есть аккаунт? </p>
        <RouterLink 
        to="/login">
        <button class="inline text-blue-500 cursor-pointer underline-offset-2 hover:underline hover:text-blue-700">Войти</button>
        </RouterLink>
        
      </div>
        <div class="pb-15"></div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import {ref} from 'vue'
import { RouterLink } from 'vue-router';
import AppButton from '@/components/ui/AppButton.vue';
import axios from 'axios';
const buttonLoading = ref(false)
const password = ref('')
const email = ref('')

const tryLogin = async () => {
  try {
    buttonLoading.value = !buttonLoading.value
    console.log('click');
    const url = `http://localhost:8080/api/Auth/login`;
    console.log(url, {
      email: email.value,
      password: password.value
    });
    // Правильно передаем данные в теле запроса
    const response = await axios.post(url, {
      email: email.value,
      password: password.value
    });
    console.log(response)
  } catch (error) {
    // Обрабатываем различные типы ошибок
    console.error('Ошибка при входе:', error);
    
  }
};
  
</script>

<style>

</style>
```
