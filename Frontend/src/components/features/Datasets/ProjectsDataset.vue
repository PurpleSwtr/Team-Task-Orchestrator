<template>
    <TableForm class="flex-1" :items="projects" :columns="columns">
        <template #row="{ item }">
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 rounded-l-xl border-y-2 border-gray-200 text-center">
                <AppIcon icon_name="projects" class="mx-auto w-6 h-6 text-gray-700"/>
            </td>
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 font-semibold border-y-2 border-gray-200">{{ item.projectName }}</td>
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-600 border-y-2 border-gray-200">{{ item.descryption || '—' }}</td>
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-500 border-y-2 border-gray-200 last:rounded-r-xl last:border-r-2">
                {{ item.startDate ? new Date(item.startDate).toLocaleDateString() : '—' }}
            </td>
        </template>
    </TableForm>
</template>

<script setup lang="ts">
import { ref, onMounted, defineExpose } from 'vue';
import TableForm from '@/components/features/Table/TableForm.vue';
import apiClient from '@/api';
import AppIcon from '@/components/ui/AppIcon.vue';

const columns = [
    { key: 'icon', label: '' },
    { key: 'projectName', label: 'Проект' },
    { key: 'descryption', label: 'Описание' },
    { key: 'startDate', label: 'Дата начала' }
];

const projects = ref([]);

const fetchProjects = async () => {
    try {
        const response = await apiClient.get('/Projects');
        projects.value = response.data;
    } catch (e) {
        console.error(e);
    }
};

onMounted(fetchProjects);
defineExpose({ fetchProjects });
</script>
