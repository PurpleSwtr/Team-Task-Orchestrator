<template>
    <button class="bg-green-600 hover:bg-green-500 transition-colors rounded-xl px-3 py-2 text-white font-semibold fade">
    {{props.message}}</button>   
</template>

<script setup lang="ts">
    import {ref} from "vue"
    const props = defineProps(['message'])
</script>

<style scoped>

</style>