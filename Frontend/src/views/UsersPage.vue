<template>
    <AppSearch></AppSearch>
    <div class="flex justify-center">
        <UsersDataset @show-user-details="handleShowUserDetails"/>
    </div>
    <BaseModal :isOpen="isModalOpen"
      @close="isModalOpen = false">
        <SetTask :user-data="selectedUser"></SetTask>
    </BaseModal>
</template>

<script setup lang="ts">
import AppSearch from '@/components/ui/AppSearch.vue';
import UsersDataset from '@/components/features/Datasets/UsersDataset.vue';
import BaseModal from '@/components/ui/BaseModal.vue';
import { ref } from 'vue';
import SetTask from '@/components/features/CardTask/SetTask.vue';
import apiClient from '@/api';

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
</script>
