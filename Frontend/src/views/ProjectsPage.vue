<template>
    <div class="flex justify-between items-center mb-6">
        <h1 class="text-3xl font-bold text-gray-700">Проекты</h1>
        <AppButton message="Новый проект" @click="isModalOpen = true"/>
    </div>

    <ProjectsDataset ref="projectsRef"/>

    <BaseModal :is-open="isModalOpen" @close="isModalOpen = false">
        <CreateProjectForm @close="isModalOpen = false" @created="refreshList"/>
    </BaseModal>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import ProjectsDataset from '@/components/features/Datasets/ProjectsDataset.vue';
import CreateProjectForm from '@/components/features/CreateProjectForm.vue';
import AppButton from '@/components/ui/AppButton.vue';
import BaseModal from '@/components/ui/BaseModal.vue';

const isModalOpen = ref(false);
const projectsRef = ref<InstanceType<typeof ProjectsDataset> | null>(null);

const refreshList = () => {
    projectsRef.value?.fetchProjects();
};
</script>
