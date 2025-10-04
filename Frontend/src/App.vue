<script setup lang="ts">
import { RouterView } from 'vue-router'
import { ref, onMounted, provide, readonly } from 'vue' 
import apiClient from './api'; 

const isLoggedIn = ref(false)
const isAuthLoading = ref(true) 

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
</script>

<template>
  <div v-if="isAuthLoading" class="loading-screen">
    <p>Загрузка...</p>
  </div>
  
  <RouterView v-else class="app"/>
</template>

<style scoped>
.app {
  background-color: #e8e8e8;
}

.loading-screen {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  font-size: 1.5rem;
  color: #333;
}
</style>