<template>
    <div class="flex justify-between items-center mb-6">
        <h1 class="text-3xl font-bold text-gray-700">Мои Команды</h1>
        <AppButton message="Создать команду" @click="isModalOpen = true"/>
    </div>

    <TeamsDataset ref="teamsDatasetRef"/>

    <BaseModal :is-open="isModalOpen" @close="isModalOpen = false">
        <CreateTeamForm @close="isModalOpen = false" @created="refreshList"/>
    </BaseModal>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import TeamsDataset from '@/components/features/Datasets/TeamsDataset.vue';
import CreateTeamForm from '@/components/features/CreateTeamForm.vue';
import AppButton from '@/components/ui/AppButton.vue';
import BaseModal from '@/components/ui/BaseModal.vue';

const isModalOpen = ref(false);
const teamsDatasetRef = ref<InstanceType<typeof TeamsDataset> | null>(null);

const refreshList = () => {
    teamsDatasetRef.value?.fetchTeams();
};
</script>
