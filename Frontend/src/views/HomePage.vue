<template>
  <AppButton message='Создать задачу' :statusLoading="buttonLoading" @click="addTask"></AppButton>

  <TransitionGroup name="fade" tag="div">
    <CardTask
      v-for="cur_task in tasks"
      :key="cur_task.id"
      :tittle="cur_task.tittle"
      :task="cur_task.text_task"
    />
  </TransitionGroup>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import CardTask from '@/components/CardTask.vue'
import AppButton from '@/components/ui/AppButton.vue'

interface Task {
  id: number;
  tittle: string;
  text_task: string;
}

const tasks = ref<Task[]>([])
const buttonLoading = ref(false)
function addTask(event: Event) {
  console.log("Задача добавлена!")
  tasks.value.push({
    // FIXME: Тут дата чувак просто так попадает как id, временный костыль пока не прикручена API 
    id: Date.now(), 
    tittle: `Заголовок ${tasks.value.length + 1}`,
    text_task: `текст ${tasks.value.length + 1}`
  })
  buttonLoading.value = !buttonLoading.value
  event.target?.dispatchEvent
}
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>