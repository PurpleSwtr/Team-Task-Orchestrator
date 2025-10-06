import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'http://localhost:8080/api',
  withCredentials: true
});

apiClient.interceptors.request.use(request => {
  const { method, url, data } = request;
  // console.log(`${method?.toUpperCase()} ${url}`, data ? { data } : '');
  return request;
});

apiClient.interceptors.response.use(
  (response) => {
    const { status, config: { method, url }, data } = response;
    console.log(`${status} ${method?.toUpperCase()} ${url}`, { data });
    return response;
  },
  (error) => {
    if (error.response) {
      const { status, config: { method, url }, data } = error.response;
      console.error(`${status} ${method?.toUpperCase()} ${url}`, { data });
    } else {
      console.error('Ошибка:', error.message);
    }
    return Promise.reject(error);
  }
);

export default apiClient;