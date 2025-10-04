<script setup lang="ts">
import { RouterView } from 'vue-router'
import { ref, onMounted, provide, readonly } from 'vue' 
import apiClient from './api'; 

const isLoggedIn = ref(false)
const isAuthLoading = ref(true) // Флаг для отслеживания состояния загрузки

// Функция для проверки статуса авторизации при загрузке приложения
const checkAuthStatus = async () => {
  isAuthLoading.value = true;
  try {
    // Этот запрос просто проверит валидность cookie. 
    // Нам не важен результат, важно, что он не вернет ошибку 401.
    await apiClient.get('/Users'); 
    isLoggedIn.value = true;
  } catch (error) {
    isLoggedIn.value = false; 
  } finally {
    isAuthLoading.value = false;
  }
}

// Вызываем проверку при монтировании компонента
onMounted(() => {
  checkAuthStatus()
})

// Функции для явного обновления состояния из дочерних компонентов
const setLoggedIn = () => {
  isLoggedIn.value = true;
}

const setLoggedOut = () => {
  isLoggedIn.value = false;
}

// Передаем состояние и функции для его изменения вниз по дереву
provide('auth', {
  isLoggedIn: readonly(isLoggedIn), // Передаем как readonly, чтобы никто случайно не изменил
  setLoggedIn,
  setLoggedOut,
  checkAuthStatus // Можно переиспользовать для обновления статуса
})
</script>

<template>
  <!-- Пока идет проверка, показываем заглушку -->
  <div v-if="isAuthLoading" class="loading-screen">
    <p>Загрузка...</p>
  </div>
  
  <!-- Когда проверка завершена, показываем основной контент -->
  <RouterView v-else class="app"/>
</template>

<style scoped>
.app {
  background-color: #e8e8e8;
}

/* Стили для экрана загрузки, чтобы он был по центру */
.loading-screen {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  font-size: 1.5rem;
  color: #333;
}
</style>