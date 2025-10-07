<script setup lang="ts">

import AppButton from '@/components/ui/AppButton.vue';
import AppIcon from '@/components/ui/AppIcon.vue';
import router from '@/router';
import {ref} from 'vue'

const message = ref('Вернуться')
const isLoading = ref(false)
const Getback = async () => {
    await router.push('/');
};

</script>

<template>
    <div class="h-screen flex items-center justify-center">
        <div class="w-170 h-120 bg-white border-2 border-gray-300 rounded-3xl shadow-xl grid grid-rows-[auto_auto_1fr_auto] p-10">
            <div class="flex items-center gap-4">
                <h1 class="text-8xl font-medium text-gray-600">403</h1>
                <AppIcon icon_name="error" class="w-26 h-26 ml-75 mt-2 text-orange-400"/>
            </div>
            
            <h5 class="mt-4 text-3xl font-semibold tracking-wide text-gray-500">
                Вы не авторизованы!
            </h5>
            
            <div>
                <p class="mt-10 text-2xl text-gray-500 text-justify">Ресурс, к которому вы хотите получить доступ, защищён, и у вас нет необходимых разрешений для доступа.</p>
            </div>
            
            <div class="flex justify-end mr-10 mb-3">
                <AppButton 
                    :message='message' 
                    :statusLoading="isLoading" 
                    @click="Getback"
                ></AppButton>
            </div>
        </div>
    </div>
</template>