<template>
  <aside class="w-64 h-screen bg-gray-800 text-cyan-50 flex flex-col p-5">
    <div class="text-2xl font-bold mb-10">
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
    {message: "Главная", route_path: "/", icon: "home"},
    {message: "Войти", route_path: "/login", icon: "login"},
    {message: "Генератор", route_path: "/generator", icon: "generator"},
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