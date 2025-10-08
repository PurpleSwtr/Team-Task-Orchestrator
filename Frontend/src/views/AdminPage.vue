<template>
    <AppWarnButton message="Удалить всех пользователей" @click="delAllUsers" :statusLoading="isLoading"></AppWarnButton>
</template>

<script setup lang="ts">
import apiClient from '@/api';
import AppWarnButton from '@/components/ui/AppWarnButton.vue';
import { ref } from 'vue';

const isLoading = ref(false)

const delAllUsers = async () => {
    isLoading.value = true
    const response = await apiClient.delete('/Users/DeleteAll');
    if (response) {
        isLoading.value = false
    }
}

</script>
