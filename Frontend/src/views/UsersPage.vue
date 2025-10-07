// views/UsersPage.vue

<template>
    <div>
        <div class="ml-15 mb-5 h-15 w-70 bg-white rounded-3xl border-2 border-gray-300">
            <!-- <span class="flex justify-start pt-4 pl-8 text-xl text-gray-400 font-">Найти</span>
              -->
            <input type="search" class="flex justify-start pt-4 pl-8 text-xl text-gray-400 outline-0" placeholder="Поиск"></input>

        </div>
    </div>
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