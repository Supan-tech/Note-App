import './assets/main.css';
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

//Testing before implemment login
// localStorage.setItem("access_token",
//   "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJ1c2VyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc2lkIjoiOWIwNzIxMGItYmRmYi00MGMzLWEzMzYtODdiNTU5NzFjYTA5IiwiZXhwIjoxNzY3MDgxMTk2LCJpc3MiOiJBdXRoU2VydmljZSIsImF1ZCI6IkF1dGhTZXJ2aWNlVXNlcnMifQ.DRlOfZsf9wpx93WizHIeSJAUBgtykKwYa9nFr2C_1XE"
// );

// localStorage.removeItem("access_token");

createApp(App)
  .use(router)
  .mount('#app')


