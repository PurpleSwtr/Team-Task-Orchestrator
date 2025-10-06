<template>
    <!-- <span>Таблица: </span> -->
    <select v-model="selected" class="border rounded-md mr-4 py-1 bg-gray-100 border-gray-400">
        <option disabled value="">Выберите таблицу</option>
        <option v-for="table in tables">{{table}}</option>
    </select>

    <input v-model="cnt_generations" type="number" class="border rounded-md mr-4 py-1 bg-gray-100 border-gray-400 outline-0" placeholder="Количество генераций">


    <AppButton :message='message' :statusLoading="isLoading" @click="GenStart"></AppButton>
    <span class="pl-4 text-xl text-gray-800">{{status}}</span>

</template>

<script setup lang="ts">
import apiClient from '@/api';
import AppButton from '@/components/ui/AppButton.vue';
import {onMounted, ref} from 'vue'
const tables = ref<string[]>([])

let status = ref('')
let selected = ref('')
let cnt_generations = ref('')
let isLoading = ref(false)
let message = ref('Сгенерировать')

const getTables = async () => {
    
    try {
    tables.value = (await apiClient.get('/Generator')).data
    }   
    catch {

    }

};

onMounted(getTables);

const GenStart = async () => {
    console.log("Начало генерации...")
    isLoading.value = true
    try{
        await apiClient.post('/Generator', {
        generatorTable: selected.value,
        countGenerations: cnt_generations.value,
        })
        status.value = 'Готово!'
    }
    catch {
        status.value = 'Генерация не завершена!'
    }
    isLoading.value = false
};

</script>

<style scoped>

</style>