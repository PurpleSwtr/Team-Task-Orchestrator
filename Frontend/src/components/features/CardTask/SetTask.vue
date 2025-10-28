<script setup lang="ts">
import AppButton from '@/components/ui/AppButton.vue';
import type { UserData } from '@/types/tables';

const props = defineProps<{
  userData: UserData | null
}>();

</script>

<template>
  <div class="bg-white p-8 rounded-2xl shadow-2xl max-w-2xl w-full mx-20">
    <h2 class="text-3xl font-bold mb-4">Детали пользователя:</h2>
    <div class="flex flex-col gap-3">
      <p class="text-xl">Имя: {{userData?.shortName}}</p>
      <p class="text-xl">Пол: {{userData?.gender === 'Male' ? 'Мужчина' : userData?.gender === 'Female' ? 'Женщина' : ''}}</p>
      <p class="text-xl">Роль: {{userData?.roles.toString()}}</p>
      <p class="text-xl">Email: {{userData?.email}}</p>
      <p class="text-xl">Дата регистрации: {{userData?.registrationTime.slice(0,10).replace(/-/g, ".") }}</p>
    </div>
    <AppButton class="mt-5" message="Назначить задачу"></AppButton>
</div>
</template>

<style scoped></style>
