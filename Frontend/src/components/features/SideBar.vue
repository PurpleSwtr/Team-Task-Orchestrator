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