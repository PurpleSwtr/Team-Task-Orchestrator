<template>
    <button :disabled="props.statusLoading" class="cursor-pointer bg-green-600 hover:bg-green-500 transition-colors rounded-xl px-3 py-2 text-white font-semibold fade">
    <span v-if="!props.statusLoading">{{props.message}}</span>
    <span v-else>
        Загрузка...
    </span>
    </button>

</template>

<script setup lang="ts">
    import {ref} from "vue"

    const props = defineProps<{
        message: string
        statusLoading?: boolean
    }>()

</script>

<style scoped>

</style>