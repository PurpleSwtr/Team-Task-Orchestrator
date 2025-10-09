<template>
    <TableForm 
        class="flex-1"
        :items="users"
        :columns="userColumns"
    >
        <!-- 
            Вот она, магия. Мы ловим слот #row и получаем доступ к переменной `item`,
            которую нам передал TableForm. Теперь мы сами решаем, как будет выглядеть
            каждая ячейка в строке.
        -->
        <template #row="{ item }">
            <!-- Ячейка с иконкой, которая нужна ТОЛЬКО в этой таблице -->
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 rounded-l-xl border-y-2 border-l-2 border-gray-200">
                <AppIcon icon_name="miniuser" class="mx-auto text-gray-700 hover:cursor-pointer hover:-translate-y-0.5 duration-500 hover:scale-110" @click="showUserDetails(item.id)"/>
            </td>
            <!-- Ячейки с данными. Обрати внимание, я скопировал все классы из твоего TableElement -->
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate border-y-2 border-gray-200">
                {{ item.shortName || '—' }}
            </td>
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate border-y-2 border-gray-200">
                {{ item.gender || '—' }}
            </td>
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate border-y-2 border-gray-200">
                {{ item.roles || '—' }}
            </td>
            <!-- У последней ячейки особые классы для скругления углов -->
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate border-y-2 border-gray-200 last:border-r-2 last:rounded-r-xl">
                {{ item.email || '—' }}
            </td>
        </template>
    </TableForm>
</template>

<script setup lang="ts">
import TableForm from '@/components/features/Table/TableForm.vue';
import { ref, onMounted, defineExpose } from 'vue';
import type { UserDataForAdmin } from '@/types/tables';
import apiClient from '@/api';
import AppIcon from '@/components/ui/AppIcon.vue'; // Не забудь импортировать AppIcon
import { useApiAsyncGet } from '@/composables';

// Теперь в userColumns нам не нужен столбец для иконки, мы его рендерим вручную
const userColumns = ref([
    { key: 'actions', label: '' }, // Пустая колонка для иконки
    { key: 'shortName', label: 'ФИО' },
    { key: 'gender', label: 'Пол' },
    { key: 'roles', label: 'Роль' },
    { key: 'email', label: 'Email' },
]);

const users = ref<UserDataForAdmin[]>([]);
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
                roles: user.roles[0],
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

// Функция для обработки клика по иконке
const showUserDetails = (userId: string) => {
    console.log(`Пользователь ${userId}`)
    const response = useApiAsyncGet(`/Users/${userId}`)
    console.log(response)  
}

onMounted(() => {
    fetchUsers();
});

defineExpose({
  fetchUsers
});
</script>