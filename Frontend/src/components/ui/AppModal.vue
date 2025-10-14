<!-- UniversalModal.vue -->
<template>
  <Transition name="fade">
    <div
      v-if="isOpen"
      class="fixed inset-0 backdrop-blur-sm bg-black bg-opacity-50 flex items-center justify-center z-50 p-4"
      @click.self="onClose"
    >
      <div
        class="bg-white rounded-2xl shadow-2xl w-full max-w-2xl max-h-[90vh] overflow-hidden transform transition-all duration-300"
        :class="sizeClasses"
      >
        <!-- Заголовок -->
        <div v-if="title" class="flex justify-between items-center p-6 border-b border-gray-200">
          <h2 class="text-2xl font-bold text-gray-800">{{ title }}</h2>
          <button @click="onClose" class="text-gray-400 hover:text-gray-600">
            <AppIcon icon_name="close" class="w-6 h-6" />
          </button>
        </div>

        <!-- Контент -->
        <div class="p-6 overflow-y-auto">
          <slot></slot>
        </div>

        <!-- Футер -->
        <div v-if="$slots.footer" class="flex justify-end gap-3 p-6 border-t border-gray-200 bg-gray-50">
          <slot name="footer"></slot>
        </div>
      </div>
    </div>
  </Transition>
</template>

<script setup lang="ts">
interface Props {
  isOpen: boolean
  title?: string
  size?: 'sm' | 'md' | 'lg' | 'xl'
}

const props = withDefaults(defineProps<Props>(), {
  size: 'md'
});

const emit = defineEmits(['close']);

const sizeClasses = {
  sm: 'max-w-md',
  md: 'max-w-2xl',
  lg: 'max-w-4xl',
  xl: 'max-w-6xl'
}[props.size];

const onClose = () => {
  emit('close');
};
</script>
