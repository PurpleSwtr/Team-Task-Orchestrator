
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
/* 
@import url('https://fonts.googleapis.com/css2?family=Montserrat:wght@400;500;600;700&display=swap');

@theme {
  fontFamily: {
    sans: ['Montserrat', ...theme('fontFamily.sans')],
  }
} */

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
<template>
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
        <AppButton @click="tryLogin" :statusLoading="buttonLoading" message="Войти" class="mx-40 mb-10"></AppButton>
        <div class="text-center">
            <p class="inline">У вас нет аккаунта? </p>
            <button 
                @click="$emit('switchToRegister')"
                class="inline text-blue-500 cursor-pointer underline-offset-2 hover:underline hover:text-blue-700"
            >
                Зарегистрироваться
            </button>
        </div>
        <div class="pb-15"></div>
      </div>
    </div>
</template>

<script setup lang="ts">
import axios from 'axios';
import { ref } from 'vue';
import AppButton from '@/components/ui/AppButton.vue';

const emit = defineEmits(['switchToRegister']);

const buttonLoading = ref(false);
const email = ref('');
const password = ref('');

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
```

---

### 📄 `components/features/RegisterForm.vue`

```vue
<template>
    <div class="flex flex-col items-center w-full max-w-md bg-gray-100 pt-30 rounded-3xl shadow-2xl shadow-gray-300">
      <h1 class="text-3xl font-bold mb-6 text-gray-700">Регистрация</h1>
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
        <AppButton @click="tryLogin" :statusLoading="buttonLoading" message="Зарегестрироваться"
        </AppButton>
        <div class="text-center">
            <p class="inline">У вас уже есть аккаунт? </p>
            <button 
                @click="$emit('switchToLogin')"
                class="inline text-blue-500 cursor-pointer underline-offset-2 hover:underline hover:text-blue-700"
            >
                Войти
            </button>
        </div>
        <div class="pb-15"></div>
      </div>
    </div>
</template>

<script setup lang="ts">
import axios from 'axios';
import { ref } from 'vue';
import AppButton from '@/components/ui/AppButton.vue';

const emit = defineEmits(['switchToLogin']);

const buttonLoading = ref(false);
const email = ref('');
const password = ref('');

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
    {message: "Войти", route_path: "/login_register", icon: "login"},
    {message: "Главная", route_path: "/", icon: "home"},
    {message: "Мои задачи", route_path: "/tasks", icon: "tasks"},
    {message: "Пользователи", route_path: "/users", icon: "users"},
    {message: "Команды", route_path: "/teams", icon: "teams"},
    {message: "Проекты", route_path: "/projects", icon: "projects"},
    {message: "Генератор", route_path: "/generator", icon: "generator"},
    {message: "Статистика", route_path: "/statistics", icon: "statistics"},
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

### 📄 `components/icons/MenuTaskIcon.vue`

```vue
<template>
  <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
    <polyline points="9 11 12 14 22 4"></polyline>
    <path d="M21 12v7a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h11"></path>
  </svg>
</template>
```

---

### 📄 `components/icons/ProjectsIcon.vue`

```vue
<template>
  <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
    <polygon points="12 2 2 7 12 12 22 7 12 2"></polygon>
    <polyline points="2 12 12 17 22 12"></polyline>
    <polyline points="2 17 12 22 22 17"></polyline>
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
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="currentColor" stroke="currentColor" stroke-width="1"><path d="M24 3.875l-6 1.221 1.716 1.708-5.351 5.358-3.001-3.002-7.336 7.242 1.41 1.418 5.922-5.834 2.991 2.993 6.781-6.762 1.667 1.66 1.201-6.002zm0 17.125v2h-24v-22h2v20h22z"/></svg>
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
    <svg width="24" height="24" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" stroke="currentColor" stroke-width="0.5" stroke-linecap="round"><path d="M22 21c.53 0 1.039.211 1.414.586.376.375.586.884.586 1.414v1h-6v-1c0-1.104.896-2 2-2h2zm-7 0c.53 0 1.039.211 1.414.586.376.375.586.884.586 1.414v1h-6v-1c0-1.104.896-2 2-2h2zm7 1h-2c-.551 0-1 .448-1 1h4c0-.564-.461-1-1-1zm-7 0h-2c-.551 0-1 .448-1 1h4c0-.564-.461-1-1-1zm-6.758-1.216c-.025.679-.576 1.21-1.256 1.21-.64 0-1.179-.497-1.254-1.156l-.406-4.034-.317 4.019c-.051.656-.604 1.171-1.257 1.171-.681 0-1.235-.531-1.262-1.21l-.262-6.456-.308.555c-.241.437-.8.638-1.265.459-.404-.156-.655-.538-.655-.951 0-.093.012-.188.039-.283l1.134-4.098c.17-.601.725-1.021 1.351-1.021h4.096c.511 0 1.012-.178 1.285-.33.723-.403 2.439-1.369 3.136-1.793.394-.243.949-.147 1.24.217.32.396.286.95-.074 1.297l-3.048 2.906c-.375.359-.595.849-.617 1.381-.061 1.397-.3 8.117-.3 8.117zm-5.718-10.795c-.18 0-.34.121-.389.294-.295 1.04-1.011 3.666-1.134 4.098l1.511-2.593c.172-.295.623-.18.636.158l.341 8.797c.01.278.5.287.523.002 0 0 .269-3.35.308-3.944.041-.599.449-1.017.992-1.017.547.002.968.415 1.029 1.004.036.349.327 3.419.385 3.938.043.378.505.326.517.022 0 0 .239-6.725.3-8.124.033-.791.362-1.523.925-2.061l3.045-2.904c-.661.492-2.393 1.468-3.121 1.873-.396.221-1.07.457-1.772.457h-4.096zm18.476 6.011c-1.305 0-2.364 1.06-2.364 2.364 0 1.305 1.059 2.365 2.364 2.365s2.364-1.06 2.364-2.365c0-1.304-1.059-2.364-2.364-2.364zm-7 0c-1.305 0-2.364 1.06-2.364 2.364 0 1.305 1.059 2.365 2.364 2.365s2.364-1.06 2.364-2.365c0-1.304-1.059-2.364-2.364-2.364zm7 1c.752 0 1.364.612 1.364 1.364 0 .753-.612 1.365-1.364 1.365-.752 0-1.364-.612-1.364-1.365 0-.752.612-1.364 1.364-1.364zm-7 0c.752 0 1.364.612 1.364 1.364 0 .753-.612 1.365-1.364 1.365-.752 0-1.364-.612-1.364-1.365 0-.752.612-1.364 1.364-1.364zm10-2h-14.658v-1h7.658v-1h3v1h3v-13h-22v7l-1 3v-11h24v15zm-6-6h-4v-1h4v1zm-12.727-5c-1.278 0-2.315 1.038-2.315 2.316 0 1.278 1.037 2.316 2.315 2.316s2.316-1.038 2.316-2.316c0-1.278-1.038-2.316-2.316-2.316zm0 1c.726 0 1.316.59 1.316 1.316 0 .726-.59 1.316-1.316 1.316-.725 0-1.315-.59-1.315-1.316 0-.726.59-1.316 1.315-1.316zm15.727 2h-7v-1h7v1zm0-2h-7v-1h7v1z"/></svg>
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

import MenuTaskIcon from '../icons/MenuTaskIcon.vue';
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
  tasks: MenuTaskIcon

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

// import LoginPage from '@/views/auth/LoginPage.vue'
// import RegisterPage from '@/views/auth/RegisterPage.vue'

import GeneratorPage from '../views/GeneratorPage.vue'
import StatisticsPage from '@/views/StatisticsPage.vue'

import TasksPage from '@/views/TasksPage.vue'
import TeamsPage from '@/views/TeamsPage.vue'
import ProjectsPage from '@/views/ProjectsPage.vue'
import UsersPage from '@/views/UsersPage.vue'
import LoginRegisterPage from '@/views/auth/LoginRegisterPage.vue'

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
          path: 'login_register', 
          name: 'login_register',  
          component: LoginRegisterPage,
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
        {
          path: 'tasks', 
          name: 'tasks',  
          component: TasksPage,
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

### 📄 `views/TasksPage.vue`

```vue
<template>
    
</template>
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

### 📄 `views/auth/LoginRegisterPage.vue`

```vue
// views/auth/LoginRegisterPage.vue

<template>
  <div class="min-h-screen flex items-center justify-center pb-50">
    <Transition name="fade" mode="out-in">
      <component 
        :is="activeComponent" 
        @switch-to-register="activeComponentName = 'RegisterForm'"
        @switch-to-login="activeComponentName = 'LoginForm'"
      />
    </Transition>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, shallowRef } from 'vue';
import LoginForm from '@/components/features/LoginForm.vue';
import RegisterForm from '@/components/features/RegisterForm.vue';

const components = {
  LoginForm,
  RegisterForm
};

const activeComponentName = ref<'LoginForm' | 'RegisterForm'>('LoginForm');

const activeComponent = computed(() => components[activeComponentName.value]);
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
```
