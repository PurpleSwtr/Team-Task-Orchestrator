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
        
        <li class="mb-4">
            <MenuButton message="О проекте" route_path="/about" icon="about"/>
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