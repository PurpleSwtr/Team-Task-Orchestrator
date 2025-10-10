<template>
    <AppWarnButton message="Удалить всех пользователей" @click="delAllUsers" :statusLoading="isLoading"></AppWarnButton>
    <AdminPanel ref="adminPanelRef" @show-user-details="handleShowUserDetails" class="mt-7"/>
    <ModalForm 
        :is-open="isModalOpen" 
        :user-data="selectedUser" 
        @close="isModalOpen = false"
        @update="refreshUsersData"
    />
</template>

<script setup lang="ts">
import apiClient from '@/api';
import AdminPanel from '@/components/features/Datasets/AdminPanel.vue';
import ModalForm from '@/components/features/ModalForm.vue';
import AppWarnButton from '@/components/ui/AppWarnButton.vue';
import { useApiAsyncDelete } from '@/composables/useApi';
import { ref } from 'vue';

const isLoading = ref(false)
const adminPanelRef = ref<InstanceType<typeof AdminPanel> | null>(null);

const delAllUsers = async () => {
    isLoading.value = true
    await useApiAsyncDelete('/Users/DeleteAll')
    await refreshUsersData()
    isLoading.value = false
}

const isModalOpen = ref(false);
const selectedUser = ref(null); 

const handleShowUserDetails = async (userId: string) => {
    try {
        const response = await apiClient.get(`/Users/${userId}`);
        if(response) {
            selectedUser.value = response.data;
            isModalOpen.value = true;
        }
    } catch (error) {
        console.error("Не удалось получить данные пользователя:", error);
    }
};

const refreshUsersData = () => {
    if (adminPanelRef.value) {
        adminPanelRef.value.fetchUsers();
    }
}
</script>