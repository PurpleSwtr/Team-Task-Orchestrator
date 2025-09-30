<template>
    <button 
    @click="addTask" 
    class="bg-green-600 hover:bg-green-500 transition-colors rounded-xl px-3 py-2 text-white font-semibold">
    Создать задачу</button>   
</template>

<script setup lang="ts">
function addTask(event: Event){
    console.log("Кнопка нажата!")
};
</script>

<style scoped>

</style>