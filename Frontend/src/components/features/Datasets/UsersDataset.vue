<template>
    <TableForm
        class="flex-1"
        :items="users"
        :columns="userColumns"
    >
        <template #row="{ item }">
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 rounded-l-xl border-y-2 border-l-2 border-gray-200">
                <AppIcon icon_name="miniuser" class="mx-auto text-gray-700 hover:cursor-pointer hover:-translate-y-0.5 duration-500 hover:scale-110" @click="showUserDetails(item.id)"/>
            </td>
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate border-y-2 border-gray-200">
                {{ item.shortName || '—' }}
            </td>
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate border-y-2 border-gray-200">
                {{ item.gender || '—' }}
            </td>
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate border-y-2 border-gray-200">
                {{ item.roles || '—' }}
            </td>

            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate border-y-2 border-gray-200 last:border-r-2 last:rounded-r-xl">
                {{ item.email || '—' }}
            </td>
        </template>
    </TableForm>
</template>

<script setup lang="ts">
import TableForm from '@/components/features/Table/TableForm.vue';
import { ref, onMounted, defineExpose } from 'vue';
import type { UserData } from '@/types/tables';
import apiClient from '@/api';
import AppIcon from '@/components/ui/AppIcon.vue';
import { useApiAsyncGet } from '@/composables';

const userColumns = ref([
    { key: 'actions', label: '' },
    { key: 'shortName', label: 'ФИО' },
    { key: 'gender', label: 'Пол' },
    { key: 'roles', label: 'Роль' },
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
                roles: user.roles[0],
                email: user.email,
                registrationTime: user.registrationTime,
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

const emit = defineEmits(['showUserDetails']);

const showUserDetails = (userId: string) => {
  console.log(`Пользователь ${userId}`)
  const response = useApiAsyncGet(`/Users/${userId}`)
  console.log(response)
  emit('showUserDetails', userId);

}

onMounted(() => {
    fetchUsers();
});

defineExpose({
  fetchUsers
});
</script>
