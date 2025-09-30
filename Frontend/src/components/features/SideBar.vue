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