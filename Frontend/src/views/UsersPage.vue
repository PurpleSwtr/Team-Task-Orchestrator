<template>
    <AppSearch></AppSearch>
    <div class="flex justify-center">
        <UsersDataset/>
    </div>
</template>

<script setup lang="ts">
import AppSearch from '@/components/ui/AppSearch.vue';
import UsersDataset from '@/components/features/Datasets/UsersDataset.vue';
</script>