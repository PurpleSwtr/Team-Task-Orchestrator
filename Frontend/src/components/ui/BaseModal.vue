<script setup lang="ts">
defineProps<{
  isOpen: boolean;
}>();

const emit = defineEmits(['close']);

const onClose = () => {
  emit('close');
};
</script>

<template>
  <Transition name="fade">
    <Teleport to="body">
      <div
        v-if="isOpen"
        class="fixed inset-0 backdrop-saturate-100 backdrop-opacity-100 backdrop-blur-sm backdrop-brightness-75 flex items-center justify-center z-10"
        @click.self="onClose"
      >
        <div @click.stop>
          <slot></slot>
        </div>
      </div>
    </Teleport>
  </Transition>
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
