<script setup lang="ts">
import type { UserData, UserDataForAdmin } from '@/types/tables';
import AppModal from '../ui/AppModal.vue';
const emit = defineEmits(['close', 'update']);


const props = defineProps<{
  userData: UserDataForAdmin | null
  isOpen: boolean
}>();

const onClose = () => {
  emit('close');
};




</script>

<template>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
