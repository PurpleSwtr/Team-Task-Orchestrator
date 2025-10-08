<template>
    <input 
          :type="props.i_type" 
          :placeholder="props.i_placeholder" 
          class="bg-white p-2 mx-20 border-2 rounded-md border-gray-300 border-b-green-600 outline-blue-600"
          v-model="model"
        >
</template>

<script setup lang="ts">
    const model = defineModel<string>()
    const props = defineProps<{
        i_type: string
        i_placeholder?: string
    }>()
</script>