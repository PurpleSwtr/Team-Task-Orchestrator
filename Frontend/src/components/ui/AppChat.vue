<template>
  <div
    class="group w-full rounded-xl shadow-xl duration-500 overflow-hidden"
    :class="{
      'h-110 bg-gray-100/60 backdrop-blur-sm': isOpen,
      'h-0': !isOpen
    }"
  >
    <div class="flex flex-col transition-all">
      <div class="bg-white/75 m-5 rounded-md text-center font-semibold text-gray-800 p-2">
        Пользователь
      </div>
      <div class="flex flex-row bg-white/75 text-start font-light text-gray-800 p-2 mt-80">
        <input type="text" class=" w-full outline-none pr-2" placeholder="Введите сообщение"></input>
        <AppButton message="" class="rounded-full bg-green-600 h-6 w-13"></AppButton>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import AppButton from './AppButton.vue';

const props = defineProps<{
  isOpen?: boolean
}>();
</script>
