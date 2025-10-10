<template>
  <div class="flex h-screen">
    
    <SideBar /> 
    <main class="flex-1 p-8 overflow-y-auto">
      <RouterView />
    </main>

</div>
</template>

<script setup lang="ts">
import SideBar from '@/components/features/SideBar.vue' 
import { ref } from 'vue';
</script>