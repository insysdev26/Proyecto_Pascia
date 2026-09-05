<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'

// Endpoint de la API apuntando a la IP de la red local
const LOGIN_URL = 'https://192.168.1.126:7123/api/auth/login'

const email = ref('')
const password = ref('')
const errorMessage = ref('')
const router = useRouter()

const handleLogin = async () => {
  errorMessage.value = ''

  try {
    // Petición POST a la IP de tu PC con soporte para cookies JWT
    const response = await axios.post(LOGIN_URL, {
      email: email.value,
      password: password.value
    }, {
      withCredentials: true // Permite guardar la cookie HttpOnly
    })

    // Extrae los datos del usuario o genera un objeto de respaldo
    const userData = response.data.user || {
      nombre: email.value.split('@')[0],
      email: email.value,
      rol: email.value.includes('admin') || email.value.includes('vendedor') ? 'vendedor' : 'cliente'
    }

    // Guarda los datos en localStorage para mantener el estado de sesión
    localStorage.setItem('pegasus_user', JSON.stringify(userData))

    // Redirige al menú principal
    router.push('/') 

  } catch (error) {
    if (error.response && error.response.data) {
      errorMessage.value = error.response.data.message || 'Credenciales inválidas'
    } else {
      errorMessage.value = 'Error de conexión con el servidor. Verifica que el backend esté encendido.'
      console.error(error)
    }
  }
}

const goToRegister = () => router.push('/registro')
const goToMenu = () => router.push('/')
const handleForgotPassword = () => alert('Función de recuperación en desarrollo')
</script>

<template>
  <div class="flex min-h-screen items-center justify-center bg-slate-950 p-4 relative">
    
    <!-- Botón Regresar al Menú -->
    <button @click="goToMenu" class="absolute top-6 left-6 md:top-10 md:left-10 text-slate-400 hover:text-cyan-400 transition-colors flex items-center gap-2 font-bold text-sm">
      <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <path d="m12 19-7-7 7-7"/><path d="M19 12H5"/>
      </svg>
      Volver al menú
    </button>

    <form class="form" @submit.prevent="handleLogin">
      <p id="heading">Iniciar Sesión</p>

      <p v-if="errorMessage" class="text-xs text-red-400 text-center -mt-2 mb-2 font-semibold">
        {{ errorMessage }}
      </p>
      
      <div class="field">
        <svg class="input-icon" xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
          <path d="M13.106 7.222c0-2.967-2.249-5.032-5.482-5.032-3.35 0-5.646 2.318-5.646 5.702 0 3.493 2.235 5.708 5.762 5.708.862 0 1.689-.123 2.304-.335v-.862c-.43.199-1.354.328-2.29.328-2.926 0-4.813-1.88-4.813-4.798 0-2.844 1.921-4.881 4.594-4.881 2.735 0 4.608 1.688 4.608 4.156 0 1.682-.554 2.769-1.416 2.769-.492 0-.772-.28-.772-.76V5.206H8.923v.834h-.11c-.266-.595-.881-.964-1.6-.964-1.4 0-2.378 1.162-2.378 2.823 0 1.737.957 2.906 2.379 2.906.8 0 1.415-.39 1.709-1.087h.11c.081.67.703 1.148 1.503 1.148 1.572 0 2.57-1.415 2.57-3.643zm-7.177.704c0-1.197.54-1.907 1.456-1.907.93 0 1.524.738 1.524 1.907S8.308 9.84 7.371 9.84c-.895 0-1.442-.725-1.442-1.914z"></path>
        </svg>
        <input v-model="email" autocomplete="off" placeholder="Correo electrónico" class="input-field" type="email" required />
      </div>

      <div class="field">
        <svg class="input-icon" xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
          <path d="M8 1a2 2 0 0 1 2 2v4H6V3a2 2 0 0 1 2-2zm3 6V3a3 3 0 0 0-6 0v4a2 2 0 0 0-2 2v5a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V9a2 2 0 0 0-2-2z"></path>
        </svg>
        <input v-model="password" placeholder="Contraseña" class="input-field" type="password" required />
      </div>

      <div class="btn">
        <button type="submit" class="button1">Entrar</button>
        <button type="button" class="button2" @click="goToRegister">Registrarse</button>
      </div>

      <button type="button" class="button3" @click="handleForgotPassword">
        ¿Olvidaste tu contraseña?
      </button>
    </form>
  </div>
</template>

<style scoped>
.form {
  display: flex;
  flex-direction: column;
  gap: 12px;
  padding: 2em;
  background-color: #171717;
  border-radius: 25px;
  transition: .4s ease-in-out;
  width: 100%;
  max-width: 380px;
  border: 1px solid #262626;
  box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.5);
}

.form:hover {
  border-color: #0891b2;
}

#heading {
  text-align: center;
  margin: 0.5em 0 1em 0;
  color: #ffffff;
  font-size: 1.5em;
  font-weight: 700;
}

.field {
  display: flex;
  align-items: center;
  gap: 0.5em;
  border-radius: 25px;
  padding: 0.6em 1em;
  background-color: #0a0a0a;
  box-shadow: inset 2px 5px 10px rgb(5, 5, 5);
  border: 1px solid #262626;
}

.input-icon {
  height: 1.3em;
  width: 1.3em;
  fill: #06b6d4;
  flex-shrink: 0;
}

.input-field {
  background: none;
  border: none;
  outline: none;
  width: 100%;
  color: #d3d3d3;
  font-size: 0.9em;
}

.form .btn {
  display: flex;
  justify-content: center;
  margin-top: 1em;
  gap: 0.5em;
}

.button1 {
  padding: 0.8em 1em;
  border-radius: 12px;
  border: none;
  outline: none;
  transition: .3s ease-in-out;
  background-color: #0891b2;
  color: white;
  font-weight: bold;
  cursor: pointer;
  flex: 1;
}

.button1:hover {
  background-color: #06b6d4;
  box-shadow: 0 0 15px rgba(6, 182, 212, 0.4);
}

.button2 {
  padding: 0.8em 1em;
  border-radius: 12px;
  border: none;
  outline: none;
  transition: .3s ease-in-out;
  background-color: #262626;
  color: white;
  font-weight: bold;
  cursor: pointer;
  flex: 1;
}

.button2:hover {
  background-color: #334155;
}

.button3 {
  margin-top: 0.5em;
  padding: 0.5em;
  border-radius: 8px;
  border: none;
  outline: none;
  transition: .3s ease-in-out;
  background-color: transparent;
  color: #94a3b8;
  font-size: 0.85em;
  cursor: pointer;
  text-align: center;
}

.button3:hover {
  color: #06b6d4;
}
</style>