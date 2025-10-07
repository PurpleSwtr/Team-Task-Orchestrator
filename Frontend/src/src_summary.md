
---

### 📄 `App.vue`

```vue
<script setup lang="ts">
import { RouterView, useRoute, useRouter, type RouteLocationNormalizedLoadedGeneric } from 'vue-router'
import { ref, onMounted, provide, readonly } from 'vue' 
import apiClient from './api'; 
import LoadingCircle from './components/ui/LoadingCircle.vue';
import E_403 from './layouts/E_403.vue';
import E_404 from './layouts/E_404.vue';

const isLoggedIn = ref(false)
const isAuthLoading = ref(true) 

const accessList = ['/about', '/login_register', '/']

const checkAuthStatus = async () => {
  isAuthLoading.value = true;
  try {
    await apiClient.get('/Auth/me'); 
    isLoggedIn.value = true;
  } catch (error) {
    isLoggedIn.value = false; 
  } finally {
    isAuthLoading.value = false;
  }
}

onMounted(() => {
  checkAuthStatus()
})

const setLoggedIn = () => {
  isLoggedIn.value = true;
}

const setLoggedOut = () => {
  isLoggedIn.value = false;
}

provide('auth', {
  isLoggedIn: readonly(isLoggedIn), 
  setLoggedIn,
  setLoggedOut,
  checkAuthStatus 
})

const existsCheck = () => {
  const path = useRoute()
  const router = useRouter()
  const routes = router.getRoutes()
  const routeExists = routes.some(route => 
    route.path === path.path)
  return !routeExists
};

const accessCheck = () => {
  const router = useRoute()

  if (isLoggedIn.value)
  {
    return false
  }
  else if (accessList.includes(router.path)) {
    return false
  }
  else
  {
    return true
  }
};

</script>

<template>
  <div v-if="isAuthLoading" class="loading-screen">
    <LoadingCircle/>
  </div>
  <body class="app">
    <E_404 v-if="existsCheck()"></E_404>
    <E_403 v-else-if="accessCheck()"></E_403>
    <RouterView v-else/>
  </body>
</template>

<style scoped>
.app {
  background-color: #e8e8e8;
}

.loading-screen {
  background-color: #e8e8e8;
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  font-size: 4.5rem;
  color: #333;
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

### 📄 `api/index.ts`

```typescript
import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'http://localhost:8080/api',
  withCredentials: true
});

apiClient.interceptors.request.use(request => {
  const { method, url, data } = request;
  // console.log(`${method?.toUpperCase()} ${url}`, data ? { data } : '');
  return request;
});

apiClient.interceptors.response.use(
  (response) => {
    const { status, config: { method, url }, data } = response;
    console.log(`${status} ${method?.toUpperCase()} ${url}`, { data });
    return response;
  },
  (error) => {
    if (error.response) {
      const { status, config: { method, url }, data } = error.response;
      console.error(`${status} ${method?.toUpperCase()} ${url}`, { data });
    } else {
      console.error('Ошибка:', error.message);
    }
    return Promise.reject(error);
  }
);

export default apiClient;
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
    <!-- <TaskIcon class="w-5 h-5 inline mr-1 mb-1"></TaskIcon> -->
    <AppIcon icon_name="task" class="w-6 h-6 inline mr-1 mb-1"></AppIcon>
    <h1 class="text-lg font-semibold truncate inline">{{ props.tittle }}</h1>
    <p class="font-normal line-clamp-3">{{ props.task }}</p>
    <div class="flex items-center gap-2 mt-2">
      <input type="checkbox" />
      <span class="text-sm">Выполнено</span>
    </div>
  </div>
</template>

<script setup>
import AppIcon from './ui/AppIcon.vue';

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
        <Transition name="fade" mode="out-in">
          <p v-if="errorMessage" class="text-red-500 self-center" key="error-message">
            {{ errorMessage }}
          </p>
        </Transition>
        <AppButton @click="tryLogin" :statusLoading="buttonLoading" message="Войти" class="self-center mb-10"/>
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
import { ref, inject } from 'vue';
import { useRouter } from 'vue-router';
import AppButton from '@/components/ui/AppButton.vue';
import apiClient from '@/api';

const emit = defineEmits(['switchToRegister']);
const router = useRouter();
const auth = inject('auth') as { setLoggedIn: () => void }; // Получаем доступ к функции из provide

const buttonLoading = ref(false);
const email = ref('');
const password = ref('');
const errorMessage = ref('');

const tryLogin = async () => {
    buttonLoading.value = true;
    errorMessage.value = '';
    try {
      await apiClient.post('/Auth/login', {
        email: email.value,
        password: password.value
      });

      if (auth) {
        auth.setLoggedIn();
      }
      
        await router.push('/');

    } catch (error) {
      errorMessage.value = 'Неверный email или пароль.';
      console.error('Ошибка при входе:', error);
    } finally {
      buttonLoading.value = false;
    }
  };
</script>

<style scoped>
.fade-enter-active, .fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
}
</style>
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
        <Transition name="fade" mode="out-in">
          <p v-if="errorMessage" class="text-red-500 self-center" key="error-message">
            {{ errorMessage }}
          </p>
        </Transition>
        <AppButton @click="tryRegister" :statusLoading="buttonLoading" message="Зарегистрироваться"
        class="self-center mb-10"/>
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
import { ref, inject } from 'vue';
import { useRouter } from 'vue-router';
import AppButton from '@/components/ui/AppButton.vue';
import apiClient from '@/api';

const emit = defineEmits(['switchToLogin']);
const router = useRouter();
const auth = inject('auth') as { setLoggedIn: () => void };

const buttonLoading = ref(false);
const email = ref('');
const password = ref('');
const errorMessage = ref('')

const tryRegister = async () => {
    buttonLoading.value = true; 
    errorMessage.value = '';

    try {
      await apiClient.post('/Auth/register', {
        email: email.value,
        password: password.value
      });

      await apiClient.post('/Auth/login', {
        email: email.value,
        password: password.value
      });
      
      if (auth) {
        auth.setLoggedIn();
      }

      await router.push('/'); 

    } catch (error: any) {
      if (error.response && error.response.data && Array.isArray(error.response.data)) {
        errorMessage.value = error.response.data.map((e: any) => e.description).join(' ');
      } else {
        errorMessage.value = 'Ошибка при регистрации. Возможно, такой email уже занят.';
      }
      console.error('Ошибка при регистрации:', error);
    } finally {
      buttonLoading.value = false; 
    }
  };
</script>


<style scoped>
.fade-enter-active, .fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
}
</style>
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
        <li class="mb-4">
          <MenuButton 
            message="Главная" 
            icon="home" 
            route_path="/"
          />
        </li>
        <li v-if="!auth?.isLoggedIn.value" class="mb-4">
          <MenuButton 
            message="Войти" 
            icon="login" 
            route_path="/login_register"
          />
        </li>
        
        <template v-if="auth?.isLoggedIn.value">
            <li v-for="item in protectedItems" :key="item.route_path" class="mb-4">
              <MenuButton 
                :message="item.message" 
                :icon="item.icon" 
                :route_path="item.route_path"
              />
            </li>
        </template>
        
        <li v-if="!auth?.isLoggedIn.value" class="mb-4">
            <MenuButton 
            message="О проекте" 
            route_path="/about" 
            icon="about"/>
        </li>
      </ul>

      <div v-if="auth?.isLoggedIn.value">
        <div class="border-t border-gray-500 my-4"></div>
        <ul>
          <li>
            <MenuButton 
              message="Настройки" 
              route_path="/settings" 
              icon="settings"
              class="mb-4"
            />
            <MenuButton 
              message="Выйти" 
              route_path="#" 
              icon="logout"
              @click.prevent="handleLogout"
            />
          </li>
        </ul>
      </div>
    </nav>
  </aside>
</template>

<script setup lang="ts">
  import { ref, inject, type Ref } from 'vue';
  import { useRouter } from 'vue-router';
  import MenuButton from '../ui/MenuButton.vue';
  import type { MenuItem } from '@/types';
  import apiClient from '@/api';

  const router = useRouter();
  const auth = inject('auth') as { 
    isLoggedIn: Ref<boolean>; 
    setLoggedOut: () => void;
  };
  
  const protectedItems = ref<MenuItem[]>([
    {message: "Мои задачи", route_path: "/tasks", icon: "tasks"},
    {message: "Пользователи", route_path: "/users", icon: "users"},
    {message: "Команды", route_path: "/teams", icon: "teams"},
    {message: "Проекты", route_path: "/projects", icon: "projects"},
    {message: "Генератор", route_path: "/generator", icon: "generator"},
    {message: "Статистика", route_path: "/statictics", icon: "statictics"},
    {message: "Админ", route_path: "/admin_panel", icon: "audit"},

  ]);

  const handleLogout = async () => {
    try {
      await apiClient.post('/Auth/logout');
    } catch (error) {
      console.error('Ошибка при выходе на сервере:', error);
    } finally {
      if (auth) {
        auth.setLoggedOut();
      }
      await router.push('/login_register');
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

### 📄 `components/features/Table/TableElement.vue`

```vue
<template>
    <!-- Меняем <p> на <td> и добавляем стандартные отступы для ячеек -->
    <td class="px-4 py-3 text-gray-700 font-semibold truncate border-2 border-gray-200">
        <slot></slot>
    </td>
</template>

<script setup lang="ts">
</script>

<style scoped>
    
</style>
```

---

### 📄 `components/features/Table/TableForm.vue`

```vue
<template>
    <div class="bg-white border-2 border-gray-300 rounded-3xl shadow-xl p-5 overflow-auto">
        <table class="w-full">
            <thead>
                <tr class="border-2 border-gray-200 bg-gray-200">
                    <th class="text-left font-bold text-gray-500 p-3 border-2 border-gray-300"></th>
                    <th v-for="column in columns" :key="column.key" class="text-left font-bold text-gray-500 p-3 border-2 border-gray-300">
                        {{ column.label }}
                    </th>
                </tr>
            </thead>
            <tbody>
                <TableLine v-for="item in items" :key="item.id" :element="item"/>
            </tbody>

        </table>
    </div>
</template>

<script setup lang="ts">
import TableLine from './TableLine.vue';
import type { LineData } from '@/types/tables';

const props = defineProps<{
    items: LineData[], 
    columns: { key: string, label: string }[] 
}>();

</script>
```

---

### 📄 `components/features/Table/TableLine.vue`

```vue
<template>
    <tr class="hover:bg-gray-100 transition-colors border-2 border-gray-200">
        <td class="px-4 py-3">
            <AppIcon icon_name="miniuser" class="mx-auto text-gray-700"/>
        </td>
        <template v-for="(value, key) in props.element">
            <TableElement>{{ value }}</TableElement>
        </template>
    </tr>
</template>

<script setup lang="ts">
import AppIcon from '@/components/ui/AppIcon.vue';
import TableElement from './TableElement.vue';
import type { LineData } from '@/types/tables';

const props = defineProps<{
    element: LineData 
}>();
</script>
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

### 📄 `components/ui/AppIcon.vue`

```vue
// src/components/ui/AppIcon.vue

<script setup lang="ts">
import { computed } from 'vue';

const icons = import.meta.glob('@/assets/icons/*.svg', { eager: true });

const props = defineProps({
    icon_name: String,
})

const iconComponent = computed(() => {
    const path = `/src/assets/icons/${props.icon_name}.svg`;
    return icons[path];
});
</script>

<template>
    <component :is="iconComponent"/>
</template>
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
        <!-- <component :is="iconMap[props.icon]" class="w-6 h-6" /> -->
        <AppIcon :icon_name="svgMap[props.icon]" class="w-6 h-6" />
        <span class="ml-3">{{props.message}}</span>
    </RouterLink>    
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router';
import AppIcon from './AppIcon.vue';

// import { ref, type Component } from 'vue'; 
// import HomeIcon from '@/components/icons/HomeIcon.vue';

// import LoginIcon from '@/components/icons/LoginIcon.vue';
// import LogoutIcon from '@/components/icons/LogoutIcon.vue';
// import SettingsIcon from '../icons/SettingsIcon.vue';

// import AboutIcon from '@/components/icons/AboutIcon.vue';
// import GeneratorIcon from '../icons/GeneratorIcon.vue';
// import StaticticsIcon from '../icons/StaticticsIcon.vue';

// import MenuTaskIcon from '../icons/MenuTaskIcon.vue';
// import ProjectsIcon from '../icons/ProjectsIcon.vue';
// import TeamsIcon from '../icons/TeamsIcon.vue';
// import UsersIcon from '../icons/UsersIcon.vue';

// const AppColor = ref("#fbbf24")

// const iconMap: { [key: string]: Component } = {
//   home: HomeIcon,
//   login: LoginIcon,
//   about: AboutIcon,
//   generator: GeneratorIcon,
//   settings: SettingsIcon,
//   logout: LogoutIcon,
//   projects: ProjectsIcon,
//   teams: TeamsIcon,
//   users: UsersIcon,
//   statistics: StaticticsIcon,
//   tasks: MenuTaskIcon
// };

const svgMap: {[key: string]: string}= {
  home: 'home',
  login: 'login',
  about: 'about',
  tasks: 'menutask',
  users: 'users',
  teams: 'teams',
  projects: 'projects',
  generator: 'generator',
  statictics: 'statictics',
  settings: 'settings',
  logout: 'logout',
  audit: 'audit'
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

### 📄 `layouts/E_403.vue`

```vue
<script setup lang="ts">

import AppButton from '@/components/ui/AppButton.vue';
import AppIcon from '@/components/ui/AppIcon.vue';
import router from '@/router';
import {ref} from 'vue'

const message = ref('Вернуться')
const isLoading = ref(false)
const Getback = async () => {
    await router.push('/');
};

</script>

<template>
    <div class="h-screen flex items-center justify-center">
        <div class="w-170 h-120 bg-white border-2 border-gray-300 rounded-3xl shadow-xl grid grid-rows-[auto_auto_1fr_auto] p-10">
            <div class="flex items-center gap-4">
                <h1 class="text-8xl font-medium text-gray-600">403</h1>
                <AppIcon icon_name="error" class="w-26 h-26 ml-75 mt-2 text-orange-400"/>
            </div>
            
            <h5 class="mt-4 text-3xl font-semibold tracking-wide text-gray-500">
                Вы не авторизованы!
            </h5>
            
            <div>
                <p class="mt-10 text-2xl text-gray-500 text-justify">Ресурс, к которому вы хотите получить доступ, защищён, и у вас нет необходимых разрешений для доступа.</p>
            </div>
            
            <div class="flex justify-end mr-10 mb-3">
                <AppButton 
                    :message='message' 
                    :statusLoading="isLoading" 
                    @click="Getback"
                ></AppButton>
            </div>
        </div>
    </div>
</template>
```

---

### 📄 `layouts/E_404.vue`

```vue
<script setup lang="ts">

import AppButton from '@/components/ui/AppButton.vue';
import AppIcon from '@/components/ui/AppIcon.vue';
import router from '@/router';
import {ref} from 'vue'

const message = ref('Вернуться')
const isLoading = ref(false)
const Getback = async () => {
    await router.push('/');
};

</script>

<template>
    <div class="h-screen flex items-center justify-center">
        <div class="w-170 h-120 bg-white border-2 border-gray-300 rounded-3xl shadow-xl grid grid-rows-[auto_auto_1fr_auto] p-10">
            <div class="flex items-center gap-4">
                <h1 class="text-8xl font-medium text-gray-600">404</h1>
                <AppIcon icon_name="error" class="w-26 h-26 ml-75 mt-2 text-orange-400"/>
            </div>
            
            <h5 class="mt-4 text-3xl font-semibold tracking-wide text-gray-500">
                Страничка не найдена!
            </h5>
            
            <div>
                <p class="mt-10 text-2xl text-gray-500 text-justify">Ресурс, к которому вы хотите получить доступ недоступен.</p>
            </div>
            
            <div class="flex justify-end mr-10 mb-3">
                <AppButton 
                    :message='message' 
                    :statusLoading="isLoading" 
                    @click="Getback"
                ></AppButton>
            </div>
        </div>
    </div>
</template>
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
import AdminPage from '@/views/AdminPage.vue'

export const router = createRouter({
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
          path: 'statictics', 
          name: 'statictics',   
          component: StatisticsPage,
        },
        {
          path: 'tasks', 
          name: 'tasks',  
          component: TasksPage,
        },
        {
          path: 'logout', 
          name: 'logout',  
          component: MainLayout,
        },
        {
          path: 'admin_panel', 
          name: 'admin_panel',  
          component: AdminPage,
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
  route_path: string; 
}

export interface Task {
  id: number;
  tittle: string;
  text_task: string;
}
```

---

### 📄 `types/tables.ts`

```typescript
export interface LineData {
  id: number;
  first_name: string;
  second_name: string;
  third_name: string;
  email: string
}
```

---

### 📄 `views/AboutPage.vue`

```vue
<script setup lang="ts">
  import AppIcon from '@/components/ui/AppIcon.vue';
</script>

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

### 📄 `views/AdminPage.vue`

```vue
<template></template>
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

    <input v-model="cnt_generations" type="number" class="border rounded-md mr-4 py-1 bg-gray-100 border-gray-400 outline-0" placeholder="Количество генераций">


    <AppButton :message='message' :statusLoading="isLoading" @click="GenStart"></AppButton>
    <span class="pl-4 text-xl text-gray-800">{{status}}</span>

</template>

<script setup lang="ts">
import apiClient from '@/api';
import AppButton from '@/components/ui/AppButton.vue';
import {onMounted, ref} from 'vue'
const tables = ref<string[]>([])

let status = ref('')
let selected = ref('')
let cnt_generations = ref('')
let isLoading = ref(false)
let message = ref('Сгенерировать')

const getTables = async () => {
    
    try {
    tables.value = (await apiClient.get('/Generator')).data
    }   
    catch {

    }

};

onMounted(getTables);

const GenStart = async () => {
    console.log("Начало генерации...")
    isLoading.value = true
    try{
        await apiClient.post('/Generator', {
        generatorTable: selected.value,
        countGenerations: cnt_generations.value,
        })
        status.value = 'Готово!'
    }
    catch {
        status.value = 'Генерация не завершена!'
    }
    isLoading.value = false
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
// views/UsersPage.vue

<template>
    <div class="flex justify-center">
        <TableForm 
            class="flex-1"
            :items="users"
            :columns="userColumns"
        />
    </div>
</template>

<script setup lang="ts">
import TableForm from '@/components/features/Table/TableForm.vue';
import { ref } from 'vue';
import type { LineData } from '@/types/tables';
import apiClient from '@/api';

const users_json =  apiClient.get('/Users')
console.log(users_json)

const userColumns = ref([
    { key: 'second_name', label: 'Фамилия' },
    { key: 'first_name', label: 'Имя' },
    { key: 'third_name', label: 'Отчество' },
    { key: 'email', label: 'Email' },
]);

const users = ref<LineData[]>([
    {
    id: 0,
    second_name: 'Иванов',
    first_name: 'Петя',
    third_name: 'Владимирович',
    email: '123@yandex.ru'},
    {
    id: 1,
    second_name: 'Семёнов',
    first_name: 'Александр',
    third_name: 'Владимирович',
    email: 'alex.semenov.longer.email@example.com'},
]);


</script>
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
