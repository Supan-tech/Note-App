import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5001/api", // change to your API domain
});

// Attach bearer token automatically to every request
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("access_token"); // or cookie, pinia, vuex
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
