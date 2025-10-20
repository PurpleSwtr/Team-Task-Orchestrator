<template>
  <AppButton message='Создать задачу' :statusLoading="buttonLoading" @click="addTask"></AppButton>

  <TransitionGroup name="fade" tag="div" class="grid grid-cols-4">
    <CardTask
      v-for="task in tasks"
      :key="task.id"
      :task_id="task.id"
      :title="task.tittle"
      :task="task.text_task"
      @click="onTaskOpen(task)"
      />
      <BaseModal :isOpen="isOpenModal"
        @close="isOpenModal = false">
        <ModalTask
        :title="openingTask?.tittle"
        :text="openingTask?.text_task" ></ModalTask>
      </BaseModal>
  </TransitionGroup>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import CardTask from '@/components/features/CardTask/CardTask.vue'
import AppButton from '@/components/ui/AppButton.vue'
import ModalTask from '@/components/features/CardTask/ModalTask.vue'
import type { Task } from '@/types'
import BaseModal from '@/components/ui/BaseModal.vue'

const tasks = ref<Task[]>([])
const buttonLoading = ref(false)

const isOpenModal = ref(false)
const openingTask = ref<Task | null>(null)


const onTaskOpen = (task: Task) => {
  openingTask.value = task
  isOpenModal.value = true
};

function addTask() {
  console.log("Задача добавлена!")
  tasks.value.push({
    id: Date.now(),
    tittle: `Заголовок ${tasks.value.length + 1}`,
    text_task: `текст ${tasks.value.length + 1}`
  })
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
