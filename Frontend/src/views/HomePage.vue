<template>
  <CardTask 
    :tittle = "tittle"
    :task = "task"/>
</template>

<script setup>
import { ref } from 'vue'
import CardTask from '@/components/CardTask.vue'

// 3. Объявляем переменные, чтобы Vue их "увидел"
const tittle = ref("Мое первое задание");
const task = ref("Нужно сделать X и Y.");
</script>

<style>
</style>
