// views/UsersPage.vue

<template>
    <div class="flex justify-center">
        <TableForm 
            class="flex-1"
            :items="users"
            :columns="userColumns"
        />
    </div>
</template>

<script setup lang="ts">
import TableForm from '@/components/features/Table/TableForm.vue';
import { ref, onMounted } from 'vue';
import type { UserData } from '@/types/tables';
import apiClient from '@/api';

const userColumns = ref([
    { key: 'firstName', label: 'Имя' },
    { key: 'secondName', label: 'Фамилия' },
    { key: 'patronymicName', label: 'Отчество' },
    { key: 'email', label: 'Email' },
]);

const users = ref<UserData[]>([]);
const isLoading = ref(false);

const fetchUsers = async () => {
    isLoading.value = true;
    try {
        const response = await apiClient.get('/Users');
        
        const usersFromApi = response.data;

        if (Array.isArray(usersFromApi)) {
            users.value = usersFromApi.map((user: any) => ({
                id: user.id,
                firstName: user.firstName,
                secondName: user.secondName,
                patronymicName: user.patronymicName,
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
</script>