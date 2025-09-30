<template>
  <AppButton></AppButton>
  <div v-for="cur_task in tasks">
    <CardTask 
      :tittle = "cur_task.tittle"
      :task = "cur_task.text_task"/>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import CardTask from '@/components/CardTask.vue'
import AppButton from '@/components/ui/AppButton.vue';

const tasks = ref([
  { tittle: "Заголовок задачи 1",
    text_task: "текст 1",
  },
])

</script>

<style>
</style>
