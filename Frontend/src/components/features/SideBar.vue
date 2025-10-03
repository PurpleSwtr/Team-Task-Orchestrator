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