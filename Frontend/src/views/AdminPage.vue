<template>
    <AppWarnButton message="Удалить всех пользователей" @click="delAllUsers" :statusLoading="isLoading"></AppWarnButton>
</template>

<script setup lang="ts">
import AppWarnButton from '@/components/ui/AppWarnButton.vue';
import { useApiAsyncDelete } from '@/composables/useApi';
import { ref } from 'vue';

const isLoading = ref(false)

const delAllUsers = async () => {
    isLoading.value = true
    useApiAsyncDelete('/Users/DeleteAll')
    isLoading.value = false
}

</script>
