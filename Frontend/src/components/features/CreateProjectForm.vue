<script setup lang="ts">
import { ref, onMounted } from 'vue';
import apiClient from '@/api';
import AppButton from '@/components/ui/AppButton.vue';
import AppInput from '@/components/ui/AppInput.vue';

const emit = defineEmits(['close', 'created']);

const projectName = ref('');
const description = ref('');
const selectedTeamId = ref('');
const myTeams = ref<any[]>([]); // Список команд для селекта

const isLoading = ref(false);
const errorMessage = ref('');

// Загружаем команды пользователя при открытии модалки
onMounted(async () => {
    try {
        const response = await apiClient.get('/Teams/my');
        myTeams.value = response.data;
        // Если команд нет, выводим предупреждение (можно улучшить UX)
    } catch (e) {
        console.error(e);
    }
});

const createProject = async () => {
    if (!projectName.value || !selectedTeamId.value) {
        errorMessage.value = 'Название и Команда обязательны';
        return;
    }

    isLoading.value = true;
    errorMessage.value = '';

    try {
        await apiClient.post('/Projects', {
            projectName: projectName.value,
            descryption: description.value,
            idTeam: selectedTeamId.value,
            // Даты можно добавить позже
            startDate: new Date().toISOString()
        });
        emit('created');
        emit('close');
    } catch (error: any) {
        if (error.response?.status === 403) errorMessage.value = 'Нет прав (Нужен Тимлид)';
        else errorMessage.value = 'Ошибка сервера';
    } finally {
        isLoading.value = false;
    }
};
</script>

<template>
    <div class="bg-white p-8 rounded-2xl shadow-2xl w-96 flex flex-col gap-4">
        <h2 class="text-2xl font-bold text-gray-700 text-center">Новый проект</h2>

        <AppInput i_type="text" i_placeholder="Название проекта" v-model="projectName" />
        <AppInput i_type="text" i_placeholder="Описание" v-model="description" />

        <!-- Кастомный селект (или обычный HTML для скорости) -->
        <select v-model="selectedTeamId" class="bg-white p-2 mx-20 border-2 rounded-md border-gray-300 border-b-green-600 outline-blue-600">
            <option disabled value="">Выберите команду</option>
            <option v-for="team in myTeams" :key="team.idTeam" :value="team.idTeam">
                {{ team.teamName }}
            </option>
        </select>

        <p v-if="errorMessage" class="text-red-500 text-center text-sm">{{ errorMessage }}</p>

        <div class="flex justify-between mt-4">
            <button @click="$emit('close')" class="text-gray-500 hover:text-gray-700 px-4">Отмена</button>
            <AppButton message="Создать" :statusLoading="isLoading" @click="createProject" />
        </div>
    </div>
</template>
