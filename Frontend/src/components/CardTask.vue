<template>
  <div class="p-4 pl-7 ml-5 mt-10 bg-gray-100 text-black relative duration-500 rounded-lg w-80 max-w-full hover:-translate-y-2 shadow-gray-300 hover:shadow-xl shadow-md hover:cursor-pointer hover:scale-103">
    <TaskIcon class="w-5 h-5 inline mr-1 mb-1"></TaskIcon>
    <h1 class="text-lg font-semibold truncate inline">{{ props.tittle }}</h1>
    <p class="font-normal line-clamp-3">{{ props.task }}</p>
    <div class="flex items-center gap-2 mt-2">
      <input type="checkbox" />
      <span class="text-sm">Выполнено</span>
    </div>
  </div>
</template>

<script setup>
import TaskIcon from '@/components/icons/TaskIcon.vue'

const props = defineProps({
  tittle: String,
  task: String,
})
</script>

<style></style>
