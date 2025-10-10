<script setup lang="ts">
import { ref, onMounted } from 'vue';
import apiClient from '@/api';
import type { UserData } from '@/types/tables';
import AppIcon from '@/components/ui/AppIcon.vue';
import LoadingCircle from '@/components/ui/LoadingCircle.vue';

const user = ref<UserData | null>(null);
const isLoading = ref(true);

onMounted(async () => {
  try {
    const meResponse = await apiClient.get('/Auth/me');
    const userId = meResponse.data.id;

    if (userId) {
      const userResponse = await apiClient.get(`/Users/${userId}`);
      user.value = userResponse.data;
    } else {
      throw new Error("Не удалось получить ID пользователя.");
    }
  } catch (error) {
    console.error("Ошибка при загрузке данных аккаунта:", error);
  } finally {
    isLoading.value = false;
  }
});

</script>

<template>
  <div v-if="isLoading" class="flex justify-center items-center h-full">
    <LoadingCircle class="text-5xl text-gray-500" />
  </div>

  <div v-else-if="user" class="bg-gray-50 rounded-2xl shadow-xl max-w-2xl w-full p-16 mx-auto relative overflow-hidden">
    
    <div class="flex justify-center -mt-5">
      <div class="flex items-center justify-center bg-gray-300 w-32 h-32 rounded-full border-0 relative overflow-hidden">
        <AppIcon icon_name="miniuser" class="absolute scale-600 text-white mt-10"></AppIcon>
      </div>
    </div>

    <div class="text-center mt-2">
      <h1 class="text-3xl font-bold text-gray-700 mb-2 inline">
        {{ user.firstName }} {{ user.secondName }} {{ user.lastName || '' }}
      </h1>
    </div>

    <p class="text-gray-600 font-bold mt-4">Email:</p>
    <p class="text-gray-600 mb-4">{{ user.email }}</p>

    <p class="text-gray-600 font-bold mt-4">Пол:</p>
    <p class="text-gray-600 mb-4">{{ user.gender === 'Male' ? 'Мужчина' : user.gender === 'Female' ? 'Женщина' : 'Не указан' }}</p>

    <p class="text-gray-600 font-bold mt-4 mb-1">Права доступа:</p>
    <p class="text-gray-600 mb-4 capitalize">{{ user.roles.join(', ') }}</p>

    <p class="text-gray-600 font-bold mt-4">Дата регистрации:</p>
    <p class="text-gray-600 mb-4">{{ new Date(user.registrationTime).toLocaleDateString('ru-RU') }}</p>

    <AppIcon icon_name="miniuser" class="absolute right-25 bottom-32 scale-1800 text-gray-200 "></AppIcon>
  </div>

  <div v-else class="text-center text-red-500">
    Не удалось загрузить данные пользователя.
  </div>
</template>

<style scoped>
</style>