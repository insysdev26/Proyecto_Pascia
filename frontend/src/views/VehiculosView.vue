<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'

const router = useRouter()

// Base URL de la API apuntando a la IP local
const API_BASE = 'https://192.168.1.126:7123'
const API_URL = `${API_BASE}/api/vehiculos`

// Estado reactivo
const vehiculos = ref([])
const errorMessage = ref('')
const successMessage = ref('')

// Campos del formulario
const form = ref({
  serial: '',
  marca: '',
  modelo: '',
  anio: new Date().getFullYear(),
  color: '',
  precio: 0,
  kilometro: 0,
  sincronico: false,
  tipo: 'Sedán',
  estado: 'Disponible'
})

const fotoArchivo = ref(null)

// Configuración global de axios para incluir la cookie HttpOnly
const axiosConfig = { withCredentials: true }

// Cargar vehículos al entrar
onMounted(async () => {
  await cargarVehiculos()
})

const cargarVehiculos = async () => {
  try {
    const response = await axios.get(API_URL, axiosConfig)
    vehiculos.value = response.data
  } catch (error) {
    if (error.response?.status === 401) {
      alert('Sesión expirada. Por favor, inicia sesión de nuevo.')
      router.push('/login')
    } else {
      console.error('Error al cargar vehículos:', error)
    }
  }
}

// Capturar el archivo seleccionado desde cámara o archivo
const handleFileUpload = (event) => {
  fotoArchivo.value = event.target.files[0]
}

const submitVehiculo = async () => {
  errorMessage.value = ''
  successMessage.value = ''

  try {
    const formData = new FormData()
    formData.append('serial', form.value.serial)
    formData.append('marca', form.value.marca)
    formData.append('modelo', form.value.modelo)
    formData.append('anio', form.value.anio)
    formData.append('color', form.value.color)
    formData.append('precio', form.value.precio)
    formData.append('kilometro', form.value.kilometro)
    formData.append('sincronico', form.value.sincronico)
    formData.append('tipo', form.value.tipo)
    formData.append('estado', form.value.estado)
    
    if (fotoArchivo.value) {
      formData.append('foto', fotoArchivo.value)
    }

    await axios.post(API_URL, formData, {
      withCredentials: true,
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    })

    successMessage.value = '¡Vehículo registrado con éxito!'
    
    // Limpiar formulario
    form.value = { 
      serial: '', 
      marca: '', 
      modelo: '', 
      anio: new Date().getFullYear(), 
      color: '', 
      precio: 0, 
      kilometro: 0, 
      sincronico: false, 
      tipo: 'Sedán', 
      estado: 'Disponible' 
    }
    fotoArchivo.value = null
    const inputElement = document.getElementById('fotoInput')
    if (inputElement) inputElement.value = ''

    // Recargar la lista
    await cargarVehiculos()

  } catch (error) {
    errorMessage.value = 'Error al guardar el vehículo. Verifica los datos.'
    console.error(error)
  }
}

const volver = () => router.push('/')
</script>

<template>
  <div class="min-h-screen bg-slate-950 text-slate-100 p-4 md:p-8 font-sans">
    
    <div class="flex justify-between items-center mb-8 border-b border-slate-800 pb-4">
      <h1 class="text-2xl md:text-3xl font-black text-white">Panel <span class="text-cyan-400">Administrativo</span></h1>
      <button @click="volver" class="text-slate-400 hover:text-cyan-400 font-bold text-sm flex items-center gap-2 transition-colors">
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="m12 19-7-7 7-7"/><path d="M19 12H5"/></svg>
        Volver al menú
      </button>
    </div>

    <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
      
      <!-- FORMULARIO DE REGISTRO -->
      <div class="bg-slate-900 border border-slate-800 p-6 rounded-xl shadow-xl h-fit">
        <h2 class="text-xl font-bold mb-4 text-cyan-400">Registrar Vehículo</h2>
        
        <p v-if="successMessage" class="text-green-400 text-sm font-semibold mb-4 bg-green-400/10 p-2 rounded">{{ successMessage }}</p>
        <p v-if="errorMessage" class="text-red-400 text-sm font-semibold mb-4 bg-red-400/10 p-2 rounded">{{ errorMessage }}</p>

        <form @submit.prevent="submitVehiculo" class="flex flex-col gap-4">
          <div class="grid grid-cols-2 gap-4">
            <input v-model="form.serial" placeholder="VIN / Serial" required class="bg-slate-950 border border-slate-700 p-2 rounded text-sm outline-none focus:border-cyan-500" />
            <input v-model="form.color" placeholder="Color" class="bg-slate-950 border border-slate-700 p-2 rounded text-sm outline-none focus:border-cyan-500" />
          </div>

          <div class="grid grid-cols-2 gap-4">
            <input v-model="form.marca" placeholder="Marca (Ej. Toyota)" required class="bg-slate-950 border border-slate-700 p-2 rounded text-sm outline-none focus:border-cyan-500" />
            <input v-model="form.modelo" placeholder="Modelo (Ej. Corolla)" required class="bg-slate-950 border border-slate-700 p-2 rounded text-sm outline-none focus:border-cyan-500" />
          </div>

          <div class="grid grid-cols-3 gap-4">
            <input v-model="form.anio" type="number" placeholder="Año" required class="bg-slate-950 border border-slate-700 p-2 rounded text-sm outline-none focus:border-cyan-500" />
            <input v-model="form.precio" type="number" step="0.01" placeholder="Precio $" required class="bg-slate-950 border border-slate-700 p-2 rounded text-sm outline-none focus:border-cyan-500" />
            <input v-model="form.kilometro" type="number" placeholder="Km" required class="bg-slate-950 border border-slate-700 p-2 rounded text-sm outline-none focus:border-cyan-500" />
          </div>

          <div class="grid grid-cols-2 gap-4">
            <select v-model="form.tipo" class="bg-slate-950 border border-slate-700 p-2 rounded text-sm outline-none focus:border-cyan-500 text-slate-300">
              <option value="Sedán">Sedán</option>
              <option value="SUV">SUV</option>
              <option value="Deportivo">Deportivo</option>
              <option value="Camioneta">Camioneta</option>
            </select>
            <select v-model="form.estado" class="bg-slate-950 border border-slate-700 p-2 rounded text-sm outline-none focus:border-cyan-500 text-slate-300">
              <option value="Disponible">Disponible</option>
              <option value="Reservado">Reservado</option>
              <option value="Vendido">Vendido</option>
            </select>
          </div>

          <label class="flex items-center gap-2 text-sm text-slate-300">
            <input type="checkbox" v-model="form.sincronico" class="accent-cyan-500" />
            Es sincrónico (Manual)
          </label>

          <!-- Input con cámara nativa activada -->
          <div class="flex flex-col gap-1 mt-2">
            <label class="text-xs text-slate-400 font-semibold">Foto del vehículo</label>
            <input 
              type="file" 
              id="fotoInput" 
              @change="handleFileUpload" 
              accept="image/*" 
              capture="environment" 
              class="text-sm file:mr-4 file:py-2 file:px-4 file:rounded file:border-0 file:text-sm file:font-semibold file:bg-cyan-600 file:text-white hover:file:bg-cyan-500 bg-slate-950 border border-slate-700 p-1 rounded cursor-pointer" 
            />
          </div>

          <button type="submit" class="mt-4 bg-cyan-600 hover:bg-cyan-500 text-white font-bold py-3 rounded transition-colors shadow-lg shadow-cyan-900/20">
            Guardar Vehículo
          </button>
        </form>
      </div>

      <!-- LISTA DE INVENTARIO -->
      <div class="lg:col-span-2 bg-slate-900 border border-slate-800 p-6 rounded-xl shadow-xl">
        <h2 class="text-xl font-bold mb-4 text-white">Inventario Actual</h2>
        
        <div v-if="vehiculos.length === 0" class="text-slate-500 text-center py-10">
          No hay vehículos registrados en el inventario.
        </div>

        <div v-else class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div v-for="vehiculo in vehiculos" :key="vehiculo.id" class="bg-slate-950 border border-slate-800 p-4 rounded-lg flex gap-4">
            
            <!-- Imagen apuntando a la IP local -->
            <div class="w-24 h-24 bg-slate-800 rounded flex-shrink-0 overflow-hidden flex items-center justify-center">
              <img v-if="vehiculo.fotoUrl" :src="`${API_BASE}${vehiculo.fotoUrl}`" alt="Auto" class="w-full h-full object-cover" />
              <span v-else class="text-xs text-slate-500">Sin foto</span>
            </div>

            <!-- Detalles -->
            <div class="flex-1 flex flex-col justify-between">
              <div>
                <h3 class="font-bold text-white leading-tight">{{ vehiculo.marca }} {{ vehiculo.modelo }} <span class="text-xs text-slate-400 font-normal">({{ vehiculo.anio }})</span></h3>
                <p class="text-xs text-slate-400 mt-1">VIN: {{ vehiculo.serial }}</p>
                <p class="text-xs text-cyan-400 font-semibold">{{ vehiculo.estado }}</p>
              </div>
              <div class="flex justify-between items-end mt-2">
                <span class="text-lg font-black text-white">${{ vehiculo.precio.toLocaleString() }}</span>
                <span class="text-xs text-slate-500">{{ vehiculo.kilometro.toLocaleString() }} km</span>
              </div>
            </div>
          </div>
        </div>

      </div>

    </div>
  </div>
</template>