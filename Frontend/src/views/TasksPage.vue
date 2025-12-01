<template>
  <div class="flex justify-between items-center mb-6">
      <h1 class="text-3xl font-bold text-gray-700">Мои задачи</h1>
      <!-- Кнопку создания добавим потом, она сложнее (нужно выбрать проект) -->
  </div>

  <div v-if="isLoading" class="flex justify-center mt-20">
      <div class="loader"></div> <!-- Твой лоадер -->
  </div>

  <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
    <CardTask
      v-for="task in tasks"
      :key="task.idTask"
      :task_id="task.idTask.toString()"
      :title="task.taskName"
      :task="task.description"
      :status="task.status || 'appointed'"
      @click="onTaskOpen(task)"
    />
  </div>

  <div v-if="!isLoading && tasks.length === 0" class="text-center text-gray-500 mt-10">
      Задач пока нет. Отдыхай! 🌴
  </div>

  <!-- Модалка просмотра задачи (твоя существующая) -->
  <BaseModal :isOpen="isOpenModal" @close="isOpenModal = false">
    <ModalTask
      :title="openingTask?.taskName"
      :text="openingTask?.description"
    />
  </BaseModal>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import apiClient from '@/api';
import CardTask from '@/components/features/CardTask/CardTask.vue';
import ModalTask from '@/components/features/CardTask/ModalTask.vue';
import BaseModal from '@/components/ui/BaseModal.vue';

const tasks = ref<any[]>([]);
const isLoading = ref(false);
const isOpenModal = ref(false);
const openingTask = ref<any>(null);

const fetchTasks = async () => {
    isLoading.value = true;
    try {
        const response = await apiClient.get('/Tasks'); // У тебя уже есть этот метод
        tasks.value = response.data;
    } catch (e) {
        console.error(e);
    } finally {
        isLoading.value = false;
    }
};

const onTaskOpen = (task: any) => {
  openingTask.value = task;
  isOpenModal.value = true;
};

onMounted(fetchTasks);
</script>
