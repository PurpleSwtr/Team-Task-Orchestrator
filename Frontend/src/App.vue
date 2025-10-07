<script setup lang="ts">
import { RouterView, useRoute, useRouter, type RouteLocationNormalizedLoadedGeneric } from 'vue-router'
import { ref, onMounted, provide, readonly } from 'vue' 
import apiClient from './api'; 
import LoadingCircle from './components/ui/LoadingCircle.vue';
import E_401 from './layouts/E_401.vue';
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

  if (accessList.includes(router.path)) {
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
  <E_404 v-if="existsCheck()"></E_404>
  <E_401 v-else-if="accessCheck()"></E_401>
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
  font-size: 4.5rem;
  color: #333;
}
</style>