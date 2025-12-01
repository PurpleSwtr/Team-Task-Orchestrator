<template>
    <TableForm
        class="flex-1"
        :items="teams"
        :columns="teamColumns"
    >
        <template #row="{ item }">
            <!-- Иконка команды -->
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 rounded-l-xl border-y-2 border-l-2 border-gray-200 text-center">
                <AppIcon icon_name="teams" class="mx-auto w-6 h-6 text-gray-700"/>
            </td>
            <!-- Название -->
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold border-y-2 border-gray-200">
                {{ item.teamName }}
            </td>
            <!-- Описание -->
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-600 border-y-2 border-gray-200">
                {{ item.description || '—' }}
            </td>
            <!-- Дата создания -->
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-500 border-y-2 border-gray-200 last:border-r-2 last:rounded-r-xl">
               {{ new Date(item.createdAt).toLocaleDateString() }}
            </td>
        </template>
    </TableForm>
</template>

<script setup lang="ts">
import TableForm from '@/components/features/Table/TableForm.vue';
import { ref, onMounted, defineExpose } from 'vue';
import apiClient from '@/api';
import AppIcon from '@/components/ui/AppIcon.vue';

const teamColumns = [
    { key: 'icon', label: '' },
    { key: 'teamName', label: 'Название' },
    { key: 'description', label: 'Описание' },
    { key: 'createdAt', label: 'Создана' },
];

const teams = ref([]);
const isLoading = ref(false);

const fetchTeams = async () => {
    isLoading.value = true;
    try {
        // Используем наш новый endpoint
        const response = await apiClient.get('/Teams/my');
        teams.value = response.data;
    } catch (error) {
        console.error("Ошибка при загрузке команд:", error);
    } finally {
        isLoading.value = false;
    }
};

onMounted(() => {
    fetchTeams();
});

defineExpose({ fetchTeams });
</script>
