<template>
  <aside class="w-64 h-screen bg-gray-800 text-cyan-50 flex flex-col p-5">
    <div class="text-2xl font-bold mb-10">
      To-Do-List
    </div>
    <nav>
      <ul>
        <li v-for="item in items" class="mb-4">
          <MenuButton :message="item.message" :icon="item.icon" :route_path="item.route_path"></MenuButton>
        </li>
      </ul>
    </nav>
  </aside>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import MenuButton from '../ui/MenuButton.vue';
let items = ref([
  {message: "Главная", route_path: "/", icon: "home"},
  {message: "Войти", route_path: "/login", icon: "login"},
  {message: "О проекте", route_path: "/about", icon: "about"},
  {message: "Генератор", route_path: "/generator", icon: "generator"},

]);

</script>

<style>
.router-link-exact-active {
  background-color: #4A5568;
  font-weight: bold;
}
</style>