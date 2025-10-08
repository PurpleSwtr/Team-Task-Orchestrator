import apiClient from "@/api";
import type { AxiosResponse } from "axios";

export function useApiSync(endpoint: string){
    
}

export function useApiAsyncDelete(endpoint: string){
    const apiAsync = async (point: string) => {
    const response = await apiClient.delete(point);
        if (response) {
            return response
        }
    }
    apiAsync(endpoint)
}

export function useApiAsyncGet(endpoint: string){
    const apiAsync = async (point: string) => {
    const response = await apiClient.get(point);
        if (response) {
            return response
        }
    }
    apiAsync(endpoint)
}