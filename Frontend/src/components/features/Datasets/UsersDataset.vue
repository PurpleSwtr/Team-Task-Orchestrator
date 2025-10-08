<template>
    <TableForm 
        class="flex-1"
        :items="users"
        :columns="userColumns"
    />
</template>

<script setup lang="ts">
import TableForm from '@/components/features/Table/TableForm.vue';
import { ref, onMounted, defineExpose } from 'vue';
import type { UserData } from '@/types/tables';
import apiClient from '@/api';

const userColumns = ref([
    { key: 'shortName', label: 'ФИО' },
    { key: 'gener', label: 'Пол' },
    // { key: 'firstName', label: 'Имя' },
    // { key: 'secondName', label: 'Фамилия' },
    // { key: 'lastName', label: 'Отчество' },
    { key: 'email', label: 'Email' },
]);

const users = ref<UserData[]>([]);
const isLoading = ref(false);

const normalizeGender = (gender: string): string => 
  gender === 'Male' ? 'М' : gender === 'Female' ? 'Ж' : '';

const fetchUsers = async () => {
    isLoading.value = true;
    try {
        const response = await apiClient.get('/Users');
        
        const usersFromApi = response.data;

        if (Array.isArray(usersFromApi)) {
            users.value = usersFromApi.map((user: any) => ({
                id: user.id,
                shortName: user.shortName,
                gender: normalizeGender(user.gender),
                // firstName: user.firstName,
                // secondName: user.secondName,
                // lastName: user.lastName,
                email: user.email,
            }));
        } else {
            console.error("Ожидался массив, но получен другой тип данных:", usersFromApi);
        }

    } catch (error) {
        console.error("Ошибка при загрузке пользователей:", error);
    } finally {
        isLoading.value = false;
    }
};

onMounted(() => {
    fetchUsers();
});

defineExpose({
  fetchUsers
});
</script>