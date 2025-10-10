<template>
  <div class="p-4 pl-6 ml-5 mt-10 bg-gray-100 text-black relative overflow-hidden duration-500 rounded-2xl w-70 max-w-full hover:-translate-y-2 shadow-gray-300 hover:shadow-xl shadow-md hover:cursor-pointer hover:scale-103">
    <!-- <TaskIcon class="w-5 h-5 inline mr-1 mb-1"></TaskIcon> -->
    <AppIcon icon_name="task" 
    class="absolute right-5 bottom-7 scale-500 text-gray-200 z-0"></AppIcon>
    <h1 class="text-lg font-semibold truncate inline text-gray-700 relative z-10">{{ props.tittle }}</h1>
    <p class="font-normal line-clamp-3 text-gray-700 relative z-10">{{ props.task }}</p>
    <div class="flex items-center gap-2 mt-2 relative z-10">
      <input type="checkbox" />
      <span class="text-sm text-gray-700">Выполнено</span>
    </div>
  </div>
</template>

<script setup>
import AppIcon from './ui/AppIcon.vue';

const props = defineProps({
  tittle: String,
  task: String,
})
</script>

<style></style>
