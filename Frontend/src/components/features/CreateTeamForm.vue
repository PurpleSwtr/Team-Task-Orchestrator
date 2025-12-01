<script setup lang="ts">
import { ref } from 'vue';
import apiClient from '@/api';
import AppButton from '@/components/ui/AppButton.vue';
import AppInput from '@/components/ui/AppInput.vue';

const emit = defineEmits(['close', 'created']);

const teamName = ref('');
const description = ref('');
const isLoading = ref(false);
const errorMessage = ref('');

const createTeam = async () => {
    if (!teamName.value) {
        errorMessage.value = 'Название команды обязательно';
        return;
    }

    isLoading.value = true;
    errorMessage.value = '';

    try {
        await apiClient.post('/Teams', {
            teamName: teamName.value,
            description: description.value
        });
        emit('created'); // Сообщаем родителю, что команда создана
        emit('close');   // Закрываем модалку
    } catch (error: any) {
        console.error(error);
        if (error.response?.status === 403) {
             errorMessage.value = 'У вас нет прав (Нужен Тимлид или Админ)';
        } else {
             errorMessage.value = 'Ошибка при создании команды';
        }
    } finally {
        isLoading.value = false;
    }
};
</script>

<template>
    <div class="bg-white p-8 rounded-2xl shadow-2xl w-96 flex flex-col gap-4">
        <h2 class="text-2xl font-bold text-gray-700 text-center">Новая команда</h2>

        <AppInput i_type="text" i_placeholder="Название команды" v-model="teamName" />
        <AppInput i_type="text" i_placeholder="Описание (опционально)" v-model="description" />

        <p v-if="errorMessage" class="text-red-500 text-center text-sm">{{ errorMessage }}</p>

        <div class="flex justify-between mt-4">
             <!-- Обычная кнопка для отмены (можно добавить стиль secondary) -->
            <button @click="$emit('close')" class="text-gray-500 hover:text-gray-700 px-4">Отмена</button>
            <AppButton message="Создать" :statusLoading="isLoading" @click="createTeam" />
        </div>
    </div>
</template>
