
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
  <div class="p-4 pl-6 ml-5 mt-10 bg-gray-100 text-black relative overflow-hidden duration-500 rounded-2xl w-70 max-w-full hover:-translate-y-2 shadow-gray-300 hover:shadow-xl shadow-md hover:cursor-pointer hover:scale-103">
    <!-- <TaskIcon class="w-5 h-5 inline mr-1 mb-1"></TaskIcon> -->
    <AppIcon icon_name="task" 
    class="absolute right-5 bottom-7 scale-500 text-gray-200"></AppIcon>
    <h1 class="text-lg font-semibold truncate inline text-gray-700">{{ props.tittle }}</h1>
    <p class="font-normal line-clamp-3 text-gray-700">{{ props.task }}</p>
    <div class="flex items-center gap-2 mt-2">
      <input type="checkbox" />
      <span class="text-sm text-gray-700">Выполнено</span>
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
        <AppInput 
        i_type="email" 
        i_placeholder="Email" 
        v-model="email"/>
        <AppInput 
        i_type="text" 
        i_placeholder="Фамилия" 
        v-model="secondName"/>
        <AppInput 
        i_type="text" 
        i_placeholder="Имя" 
        v-model="firstName"/>
        <AppInput 
        i_type="text" 
        i_placeholder="Отчество" 
        v-model="lastName"/>
        <AppInput 
        i_type="password" 
        i_placeholder="Пароль" 
        v-model="password"/>
        <AppInput 
        i_type="password" 
        i_placeholder="Подтверждение пароля" 
        v-model="password_check"/>
        <div class="flex justify-center space-x-8 py-4">
          <label class="flex items-center space-x-3 cursor-pointer group">
            <input type="radio" id="one" value="Male" v-model="gender" class="hidden" />
            <span class="w-6 h-6 border-2 border-gray-300 rounded-full flex items-center justify-center group-hover:border-blue-500 transition-all duration-200">
              <span class="w-3 h-3 rounded-full bg-transparent group-has-[:checked]:bg-blue-500 transition-all duration-200"></span>
            </span>
            <span class="text-gray-700 group-has-[:checked]:text-blue-600 font-medium transition-colors duration-200">Мужчина</span>
          </label>
          
          <label class="flex items-center space-x-3 cursor-pointer group">
            <input type="radio" id="two" value="Female" v-model="gender" class="hidden" />
            <span class="w-6 h-6 border-2 border-gray-300 rounded-full flex items-center justify-center group-hover:border-pink-500 transition-all duration-200">
              <span class="w-3 h-3 rounded-full bg-transparent group-has-[:checked]:bg-pink-500 transition-all duration-200"></span>
            </span>
            <span class="text-gray-700 group-has-[:checked]:text-pink-600 font-medium transition-colors duration-200">Женщина</span>
          </label>
        </div>
        
        <Transition name="fade" mode="out-in">
          <p v-if="errorMessage" class="text-red-500 self-center px-5 text-pretty" key="error-message">
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
import { ref, inject, reactive } from 'vue';
import { useRouter } from 'vue-router';
import AppButton from '@/components/ui/AppButton.vue';
import apiClient from '@/api';
import AppInput from '../ui/AppInput.vue';

const emit = defineEmits(['switchToLogin']);
const router = useRouter();
const auth = inject('auth') as { 
  setLoggedIn: () => void;
  checkAuthStatus: () => Promise<void>; // Убедись, что checkAuthStatus прокинут через provide
};
const buttonLoading = ref(false);

const firstName = ref('');
const secondName = ref('');
const lastName = ref('');

const email = ref('');

const password = ref('');
const password_check = ref('');

const gender = ref('');

const errorMessage = ref('')

const tryRegister = async () => {
    errorMessage.value = '';
    console.log(gender.value)
    if (password.value == password_check.value) {
      try {
          buttonLoading.value = true; 
          await apiClient.post('/Auth/register', {
            firstName: firstName.value,
            secondName: secondName.value,
            lastName: lastName.value,
            gender: gender.value,
            email: email.value,
            password: password.value
          });
          await apiClient.post('/Auth/login', {
            email: email.value,
            password: password.value
          });
          
          if (auth) {
            auth.setLoggedIn();
            await auth.checkAuthStatus();
            await router.push('/');
          }
          
          await router.push('/'); 
      } catch (error: any) {

        if (error.response && error.response.data && Array.isArray(error.response.data)) {
          errorMessage.value = error.response.data.map((e: any) => e.description).join(' ');
        }
        else{
          errorMessage.value = "Не все поля заполненны, либо не соответсвуют требованиям."
        }
        console.error('Ошибка при регистрации:', error);
      } finally {
        buttonLoading.value = false; 
      }
    }
    else
    {
      errorMessage.value = 'Пароли несовпадают!';
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

### 📄 `components/features/Datasets/AdminPanel.vue`

```vue
<template>
    <TableForm 
        class="flex-1"
        :items="users"
        :columns="userColumns"
    />
</template>

<script setup lang="ts">
import TableForm from '@/components/features/Table/TableForm.vue';
import { ref, onMounted, defineExpose } from 'vue';
import type { UserDataForAdmin } from '@/types/tables';
import apiClient from '@/api';

const userColumns = ref([
    { key: 'shortName', label: 'ФИО' },
    { key: 'gender', label: 'Пол' },
    // { key: 'firstName', label: 'Имя' },
    // { key: 'secondName', label: 'Фамилия' },
    // { key: 'lastName', label: 'Отчество' },
    { key: 'roles', label: 'Роль' },
    { key: 'email', label: 'Email' },
]);

const users = ref<UserDataForAdmin[]>([]);
const isLoading = ref(false);

const normalizeGender = (gender: string): string => 
  gender === 'Male' ? 'М' : gender === 'Female' ? 'Ж' : '';

const fetchUsers = async () => {
    isLoading.value = true;
    try {
        const response = await apiClient.get('/Users');
        
        const usersFromApi = response.data;

        if (Array.isArray(usersFromApi)) {
            users.value = usersFromApi.map((user: any) => ({
                id: user.id,
                shortName: user.shortName,
                gender: normalizeGender(user.gender),
                // firstName: user.firstName,
                // secondName: user.secondName,
                // lastName: user.lastName,
                roles: user.roles[0],
                email: user.email,
            }));
        } else {
            console.error("Ожидался массив, но получен другой тип данных:", usersFromApi);
        }

    } catch (error) {
        console.error("Ошибка при загрузке пользователей:", error);
    } finally {
        isLoading.value = false;
    }
};

onMounted(() => {
    fetchUsers();
});

defineExpose({
  fetchUsers
});
</script>
```

---

### 📄 `components/features/Datasets/UsersDataset.vue`

```vue
<template>
    <TableForm 
        class="flex-1"
        :items="users"
        :columns="userColumns"
    />
</template>

<script setup lang="ts">
import TableForm from '@/components/features/Table/TableForm.vue';
import { ref, onMounted, defineExpose } from 'vue';
import type { UserData } from '@/types/tables';
import apiClient from '@/api';

const userColumns = ref([
    { key: 'shortName', label: 'ФИО' },
    { key: 'gener', label: 'Пол' },
    // { key: 'firstName', label: 'Имя' },
    // { key: 'secondName', label: 'Фамилия' },
    // { key: 'lastName', label: 'Отчество' },
    { key: 'roles', label: 'Роль' },
    { key: 'email', label: 'Email' },
]);

const users = ref<UserData[]>([]);
const isLoading = ref(false);

const normalizeGender = (gender: string): string => 
  gender === 'Male' ? 'М' : gender === 'Female' ? 'Ж' : '';

const fetchUsers = async () => {
    isLoading.value = true;
    try {
        const response = await apiClient.get('/Users');
        
        const usersFromApi = response.data;

        if (Array.isArray(usersFromApi)) {
            users.value = usersFromApi.map((user: any) => ({
                id: user.id,
                shortName: user.shortName,
                gender: normalizeGender(user.gender),
                // firstName: user.firstName,
                // secondName: user.secondName,
                // lastName: user.lastName,
                roles: user.roles[0],
                email: user.email,
            }));
        } else {
            console.error("Ожидался массив, но получен другой тип данных:", usersFromApi);
        }

    } catch (error) {
        console.error("Ошибка при загрузке пользователей:", error);
    } finally {
        isLoading.value = false;
    }
};

onMounted(() => {
    fetchUsers();
});

defineExpose({
  fetchUsers
});
</script>
```

---

### 📄 `components/features/Table/TableElement.vue`

```vue
<template>
    <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate 
               border-y-2 border-gray-200 
               last:border-r-2 last:rounded-r-xl">
        <slot></slot>
    </td>
</template>

<script setup lang="ts">
</script>
```

---

### 📄 `components/features/Table/TableForm.vue`

```vue
<template>
    <div class="bg-white border-2 border-gray-300 rounded-3xl shadow-xl p-5 overflow-auto">
        <table class="w-full border-separate border-spacing-y-3">
            <thead>
                <tr>
                    <th class="text-left font-bold text-gray-500 p-3"></th>
                    <th v-for="column in columns" :key="column.key" class="text-left font-bold text-gray-500 p-3">
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
import type { UserData } from '@/types/tables';

const props = defineProps<{
    items: UserData[], 
    columns: { key: string, label: string }[] 
}>();
</script>
```

---

### 📄 `components/features/Table/TableLine.vue`

```vue
<template>
    <tr class="group transition-colors">
        <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 rounded-l-xl border-y-2 border-l-2 border-gray-200">
                <AppIcon icon_name="miniuser" class="mx-auto text-gray-700 hover:cursor-pointer hover:-translate-y-0.5 duration-500 hover:scale-110" @click="GetUserData"/>
        </td>

        <template v-for="(value, key) in props.element">
            <TableElement v-if="key !== 'id'">{{ value || '—' }}</TableElement>
        </template>
    </tr>
</template>

<script setup lang="ts">
import AppIcon from '@/components/ui/AppIcon.vue';
import TableElement from './TableElement.vue';
import type { UserData } from '@/types/tables';
import { useApiAsyncGet } from '@/composables/useApi';

const props = defineProps<{
    element: UserData 
}>();

function GetUserData() {
    console.log(`Пользователь ${props.element.id}`)
    const response = useApiAsyncGet(`/Users/${props.element.id}`)
    console.log(response)  
}

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

### 📄 `components/ui/AppInput.vue`

```vue
<template>
    <input 
          :type="props.i_type" 
          :placeholder="props.i_placeholder" 
          class="bg-white p-2 mx-20 border-2 rounded-md border-gray-300 border-b-green-600 outline-blue-600"
          v-model="model"
        >
</template>

<script setup lang="ts">
    const model = defineModel<string>()
    const props = defineProps<{
        i_type: string
        i_placeholder?: string
    }>()
</script>
```

---

### 📄 `components/ui/AppSearch.vue`

```vue
<script setup lang="ts">
import AppIcon from './AppIcon.vue';

</script>

<template>
    <div class="ml-15 mb-5 h-12 w-70 bg-white rounded-3xl border-2 border-gray-300">
        <input type="search" class="w-full h-full py-3 px-8 text-md text-gray-400 outline-0" placeholder="Поиск">
    </div>
</template>
```

---

### 📄 `components/ui/AppWarnButton.vue`

```vue
<template>
    <button :disabled="props.statusLoading" 
    class="cursor-pointer 
    bg-red-600 
    hover:bg-red-700 
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

### 📄 `composables/index.ts`

```typescript
export { useStrip } from './useStrip'
export { useApiSync } from './useApi'
export { useApiAsyncGet } from './useApi'
export { useApiAsyncDelete } from './useApi'
```

---

### 📄 `composables/useApi.ts`

```typescript
import apiClient from "@/api";
import type { AxiosResponse } from "axios";

export function useApiSync(endpoint: string){
    
}

export function useApiAsyncDelete(endpoint: string){
    const apiAsync = async (point: string) => {
    const response = await apiClient.delete(point);
        if (response) {
            return response
        }
    }
    apiAsync(endpoint)
}

export function useApiAsyncGet(endpoint: string){
    const apiAsync = async (point: string) => {
    const response = await apiClient.get(point);
        if (response) {
            return response
        }
    }
    apiAsync(endpoint)
}
```

---

### 📄 `composables/useStrip.ts`

```typescript
export function useStrip(text: string, toStrip: string,){
    return text.replace(toStrip, '')
}
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
import type { Component } from "vue";

export interface UserData {
  id: string;
  gender: string;
  // firstName: string;
  // secondName: string;
  // lastName: string | null; 
  shortName: string;
  roles: Array<[]>;
  email: string | null; 
}


export interface UserDataForAdmin {
  id: string;
  gender: string;
  // firstName: string;
  // secondName: string;
  // lastName: string | null; 
  shortName: string;
  roles: Array<[]>;
  email: string | null; 
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
<template>
    <AppWarnButton message="Удалить всех пользователей" @click="delAllUsers" :statusLoading="isLoading"></AppWarnButton>
    <AdminPanel class="mt-7"/>
</template>

<script setup lang="ts">
import AdminPanel from '@/components/features/Datasets/AdminPanel.vue';
import AppWarnButton from '@/components/ui/AppWarnButton.vue';
import { useApiAsyncDelete } from '@/composables/useApi';
import { ref } from 'vue';

const isLoading = ref(false)

const delAllUsers = async () => {
    isLoading.value = true
    useApiAsyncDelete('/Users/DeleteAll')
    isLoading.value = false
}

</script>
```

---

### 📄 `views/GeneratorPage.vue`

```vue
<template>
    <!-- <span>Таблица: </span> -->
    <select v-model="selected" class="w-50 border rounded-lg mr-4 py-1 bg-gray-100 border-gray-400">
        <option disabled value="">Выберите таблицу</option>
        <option v-for="table in tables
        ">{{table}}</option>
    </select>

    <input v-model="cnt_generations" type="number" class="w-50 border rounded-lg mr-4 py-1 bg-gray-100 border-gray-400 outline-0" placeholder="Количество генераций">


    <AppButton :message='message' :statusLoading="isLoading" @click="GenStart"></AppButton>
    <span class="pl-4 text-xl text-gray-800">{{status}}</span>
    <div class="flex justify-center pt-7">
        <UsersDataset ref="usersDatasetRef"/>
    </div>

</template>

<script setup lang="ts">
import apiClient from '@/api';
import AppButton from '@/components/ui/AppButton.vue';
import {onMounted, ref} from 'vue'
import UsersDataset from '@/components/features/Datasets/UsersDataset.vue';
const tables = ref<string[]>([])
const usersDatasetRef = ref<InstanceType<typeof UsersDataset> | null>(null);
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
    console.log("Начало генерации...");
    isLoading.value = true;
    status.value = '';
    try {
        await apiClient.post('/Generator', {
            generatorTable: selected.value,
            countGenerations: cnt_generations.value,
        });
        status.value = 'Готово!';

        if (usersDatasetRef.value) {
            await usersDatasetRef.value.fetchUsers();
        }

    } catch {
        status.value = 'Генерация не завершена!';
    } finally {
        isLoading.value = false;
    }
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
    <AppSearch></AppSearch>
    <div class="flex justify-center">
        <UsersDataset/>
    </div>
</template>

<script setup lang="ts">
import AppSearch from '@/components/ui/AppSearch.vue';
import UsersDataset from '@/components/features/Datasets/UsersDataset.vue';
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
