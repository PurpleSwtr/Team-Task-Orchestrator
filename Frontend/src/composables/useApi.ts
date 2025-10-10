import apiClient from "@/api";

export function useApiAsyncDelete(endpoint: string){
    const apiAsync = async (point: string) => {
    const response = await apiClient.delete(point);
        if (response) {
            return response
        }
    }
    return apiAsync(endpoint)
}

export function useApiAsyncGet(endpoint: string){
    const apiAsync = async (point: string) => {
    const response = await apiClient.get(point);
        if (response) {
            return response
        }
    }
    return apiAsync(endpoint)
}

export function useApiAsyncPatch(endpoint: string, payload: any){
    const apiAsync = async (point: string, data: any) => {
        const response = await apiClient.patch(point, data);
        if (response) {
            return response
        }
    }
    return apiAsync(endpoint, payload)
}