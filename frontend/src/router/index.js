import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import LoginView from '@/views/LoginView.vue'
import RegisterView from '@/views/RegisterView.vue'
import VehiculosView from '@/views/VehiculosView.vue'

const routes = [
  { path: '/', name: 'Home', component: HomeView },
  { path: '/login', name: 'Login', component: LoginView },
  { path: '/registro', name: 'Registro', component: RegisterView },
  { path: '/vehiculos', name: 'Vehiculos', component: VehiculosView }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router