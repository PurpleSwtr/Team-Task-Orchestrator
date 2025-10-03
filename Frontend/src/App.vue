<script setup lang="ts">
import { RouterView } from 'vue-router'
import { ref, onMounted, provide } from 'vue' 
import apiClient from './api'; 

const isLoggedIn = ref(false)

const checkAuthStatus = async () => {
  try {
    await apiClient.get('/Users'); 
    isLoggedIn.value = true;
  } catch (error) {
    isLoggedIn.value = false; 
  }
}

onMounted(() => {
  checkAuthStatus()
})

provide('auth', {
  isLoggedIn,
  checkAuthStatus
})
</script>

<template>
  <RouterView class="app"/>
</template>


<style scoped>
.app{
background-color: #e8e8e8;
}
</style>
