<script setup lang="ts">
import AppButton from '@/components/ui/AppButton.vue';
import AppIcon from '@/components/ui/AppIcon.vue';
import AppChat from '@/components/ui/AppChat.vue';

import { ref } from 'vue';
import AppWarnButton from '@/components/ui/AppWarnButton.vue';

const props = defineProps<{
  title?: string,
  text?: string,
}>();

const w_side = ref('w-64')
const question_button = ref('Спросить')
const chatIsOpen = ref(false)

const onQuestionClick = () => {
  chatIsOpen.value = !chatIsOpen.value
  w_side.value = w_side.value == 'w-110' ? 'w-64' : 'w-110'
  question_button.value = question_button.value == 'Спросить' ? 'Закрыть' : 'Спросить'

};

</script>

<template>
  <div class="flex relative w-200 h-150 bg-white rounded-4xl shadow-2xl overflow-hidden">

    <div class="flex-1 p-15 overflow-y-auto min-w-0 tracking-wide text-pretty">
      <h1 class="text-3xl font-bold">{{props.title}}</h1>
      <p class="pt-5 text-xl font-normal">Описае Описание Описание Описание Описание Описание Описание Описание Описание Описание Описание </p>
  </div>
    <div class="relative h-full bg-amber-500 saturate-50 overflow-hidden flex-shrink-0 duration-500"
    :class="[w_side]">
      <div class="flex flex-col gap-4 p-4 pt-15 mx-10">
        <AppButton :message="question_button" @click="onQuestionClick"/>
        <AppChat class="z-11" :is-open="chatIsOpen"/>
        <!-- <AppButton class="z-11" v-show="chatIsOpen" message="Отправить"/> -->
        <AppWarnButton v-show="!chatIsOpen" message="Завершить"/>
      </div>
          <AppIcon icon_name="task" class="absolute
    z-10
    duration-700
    hover:-translate-16
    hover:rotate-5
    text-yellow-400
    "
    :class="{
      'right-12 bottom-15 scale-900 ': !chatIsOpen,
      'right-52 bottom-65 -rotate-18 scale-1200': chatIsOpen,

    }"
    />
      <!-- <div class="absolute w-35 h-32 rounded-full right-0 bottom-0 z-2 bg-amber-600 scale-225"></div> -->
    </div>

  </div>
</template>
