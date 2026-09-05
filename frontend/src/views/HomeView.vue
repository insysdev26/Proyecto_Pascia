<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
import { Button } from '@/components/ui/button'

// URL de Logout apuntando a la IP de la red local
const LOGOUT_URL = 'https://192.168.1.126:7123/api/auth/logout'

const router = useRouter()
const usuarioSesion = ref(null)
const isMenuOpen = ref(false) // Controla el estado del menú desplegable

onMounted(() => {
  const sesionGuardada = localStorage.getItem('pegasus_user')
  if (sesionGuardada) {
    usuarioSesion.value = JSON.parse(sesionGuardada)
  }
})

const irALogin = () => router.push('/login')
const irARegistro = () => router.push('/registro')
const irAInventario = () => router.push('/vehiculos')

const cerrarSesion = async () => {
  try {
    // 1. Destruimos la cookie HttpOnly en el backend apuntando a la IP local
    await axios.post(LOGOUT_URL, {}, { 
      withCredentials: true 
    })
  } catch (error) {
    console.error('Error al cerrar sesión en el servidor:', error)
  } finally {
    // 2. Limpiamos la sesión del navegador
    localStorage.removeItem('pegasus_user')
    usuarioSesion.value = null
    isMenuOpen.value = false
    router.push('/')
  }
}
</script>

<template>
  <div class="relative min-h-screen text-slate-100 font-sans overflow-hidden bg-slate-950">
    
    <!-- BARRA DE NAVEGACIÓN -->
    <nav class="absolute top-0 left-0 w-full flex items-center justify-between px-4 md:px-8 py-4 z-50">
      <div class="flex items-center">
        <img src="@/assets/logo.png" alt="Pegasus Logo" class="h-12 md:h-14 w-auto object-contain invert mix-blend-screen" />
      </div>
      
      <div class="hidden md:flex gap-10 text-sm font-semibold text-slate-300 tracking-wide">
        <a href="#" class="hover:text-cyan-400 transition-colors">NUEVOS 0KM</a>
        <a href="#" class="hover:text-cyan-400 transition-colors">USADOS CERTIFICADOS</a>
        <a href="#" class="hover:text-cyan-400 transition-colors">SERVICIOS</a>
      </div>
      
      <div class="flex items-center gap-3 md:gap-4">
        
        <!-- Invitado (Sin sesión) -->
        <template v-if="!usuarioSesion">
          <Button @click="irARegistro" class="bg-white hover:bg-slate-200 text-slate-950 font-bold px-4 md:px-6 text-sm">
            Registrarse
          </Button>
          <Button @click="irALogin" class="bg-cyan-600 hover:bg-cyan-500 text-white font-bold px-4 md:px-6 text-sm">
            Iniciar Sesión
          </Button>
        </template>

        <!-- Usuario Autenticado -->
        <template v-else>
          <div class="relative flex items-center gap-3 md:gap-4">
            
            <div class="flex flex-col text-right mr-1">
              <span class="font-bold text-white text-sm">{{ usuarioSesion.nombre }}</span>
              <span class="text-xs text-cyan-400 font-semibold capitalize">{{ usuarioSesion.rol }}</span>
            </div>

            <!-- Ícono Hamburguesa -->
            <button @click="isMenuOpen = !isMenuOpen" class="text-slate-300 hover:text-white transition-colors focus:outline-none p-1">
              <svg xmlns="http://www.w3.org/2000/svg" width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <line x1="3" y1="12" x2="21" y2="12"></line>
                <line x1="3" y1="6" x2="21" y2="6"></line>
                <line x1="3" y1="18" x2="21" y2="18"></line>
              </svg>
            </button>

            <!-- Menú Desplegable -->
            <div v-if="isMenuOpen" class="absolute top-12 right-0 w-52 bg-slate-900 border border-slate-700 rounded-md shadow-xl overflow-hidden z-50">
              <div class="flex flex-col">
                <button 
                  v-if="['vendedor', 'admin', 'dueño'].includes(usuarioSesion.rol?.toLowerCase())" 
                  @click="irAInventario" 
                  class="px-4 py-3 text-left text-sm text-white hover:bg-slate-800 font-semibold border-b border-slate-800 transition-colors flex items-center gap-2"
                >
                  ⚙️ Panel Administrativo
                </button>
                <button class="px-4 py-3 text-left text-sm text-white hover:bg-slate-800 font-semibold border-b border-slate-800 transition-colors">
                  👤 Mi Perfil
                </button>
                <button @click="cerrarSesion" class="px-4 py-3 text-left text-sm text-red-400 hover:bg-red-500/10 hover:text-red-300 font-semibold flex items-center gap-2 transition-colors">
                  <svg class="w-4 h-4" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                    <path d="M15 3h4a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2h-4"/><polyline points="10 17 15 12 10 7"/><line x1="15" y1="12" x2="3" y2="12"/>
                  </svg>
                  Cerrar Sesión
                </button>
              </div>
            </div>

          </div>
        </template>

      </div>
    </nav>

    <!-- HÉROE / SECCIÓN PRINCIPAL -->
    <section class="relative min-h-screen flex items-center">
      <div class="container"></div>
      <div class="absolute inset-0 bg-gradient-to-r from-slate-950 via-slate-950/80 to-transparent z-0"></div>

      <div class="relative z-10 p-6 md:p-20 max-w-4xl mt-16">
        <h1 class="text-5xl md:text-8xl font-extrabold text-white mb-6 leading-tight tracking-tight">
          Excelencia <br><span class="text-cyan-400">Multimarca</span>
        </h1>
        <p class="text-lg md:text-xl text-slate-300 mb-10 max-w-2xl leading-relaxed">
          Descubre la mayor selección de modelos nuevos y usados de las mejores marcas del mundo. Tu próximo vehículo premium te espera en Pegasus.
        </p>
        <div class="flex flex-col sm:flex-row gap-6">
          <Button size="lg" variant="outline" class="text-white border-slate-500 hover:bg-slate-100 hover:text-black text-base md:text-lg px-8 md:px-10 py-6 md:py-7 rounded-sm font-bold tracking-wider bg-slate-950/40 backdrop-blur-sm transition-all">
            EXPLORAR CATÁLOGO
          </Button>
        </div>
      </div>
    </section>

  </div>
</template>

<style scoped>
.container {
  position: absolute;
  inset: 0;
}

.container::before {
  content: "";
  position: absolute;
  inset: -145%;
  rotate: -45deg;
  background: #000000;
  background-image: radial-gradient(
      4px 100px at 0px 235px,
      rgb(255, 140, 17),
      #0000
    ),
    radial-gradient(4px 100px at 300px 235px, rgb(255, 119, 0), #884e2800),
    radial-gradient(
      1.5px 1.5px at 150px 117.5px,
      rgb(255, 144, 9) 100%,
      #0000 150%
    ),
    radial-gradient(4px 100px at 0px 252px, rgb(156, 14, 137), #0000),
    radial-gradient(4px 100px at 300px 252px, rgb(23, 41, 206), #0000),
    radial-gradient(
      1.5px 1.5px at 150px 126px,
      rgb(247, 102, 18) 100%,
      #0000 150%
    ),
    radial-gradient(4px 100px at 0px 150px, rgb(249, 121, 16), #0000),
    radial-gradient(4px 100px at 300px 150px, rgb(255, 128, 18), #0000),
    radial-gradient(
      1.5px 1.5px at 150px 75px,
      rgb(255, 116, 10) 100%,
      #0000 150%
    ),
    radial-gradient(4px 100px at 0px 253px, rgb(249, 137, 17), #0000),
    radial-gradient(4px 100px at 300px 253px, rgb(248, 107, 14), #0000),
    radial-gradient(
      1.5px 1.5px at 150px 126.5px,
      rgb(252, 129, 14) 100%,
      #0000 150%
    ),
    radial-gradient(4px 100px at 0px 204px, rgb(234, 115, 18), #0000),
    radial-gradient(4px 100px at 300px 204px, rgb(255, 139, 6), #0000),
    radial-gradient(
      1.5px 1.5px at 150px 102px,
      rgb(255, 128, 9) 100%,
      #0000 150%
    ),
    radial-gradient(4px 100px at 0px 134px, rgb(249, 133, 9), #0000),
    radial-gradient(4px 100px at 300px 134px, rgb(251, 125, 15), #0000),
    radial-gradient(
      1.5px 1.5px at 150px 67px,
      rgb(255, 146, 13) 100%,
      #0000 150%
    ),
    radial-gradient(4px 100px at 0px 179px, rgb(249, 137, 17), #0000),
    radial-gradient(4px 100px at 300px 179px, rgb(253, 122, 6), #0000),
    radial-gradient(
      1.5px 1.5px at 150px 89.5px,
      rgb(234, 132, 7) 100%,
      #0000 150%
    ),
    radial-gradient(4px 100px at 0px 299px, rgb(255, 115, 0), #0000),
    radial-gradient(4px 100px at 300px 299px, rgb(255, 136, 0), #0000),
    radial-gradient(
      1.5px 1.5px at 150px 149.5px,
      rgb(255, 123, 0) 100%,
      #0000 150%
    ),
    radial-gradient(4px 100px at 0px 215px, rgb(255, 145, 0), #0000),
    radial-gradient(4px 100px at 300px 215px, rgb(255, 132, 0), #0000),
    radial-gradient(
      1.5px 1.5px at 150px 107.5px,
      rgb(255, 136, 0) 100%,
      #0000 150%
    ),
    radial-gradient(4px 100px at 0px 281px, rgb(255, 170, 0), #0000),
    radial-gradient(4px 100px at 300px 281px, rgb(255, 115, 0), #0000),
    radial-gradient(
      1.5px 1.5px at 150px 140.5px,
      rgb(255, 119, 0) 100%,
      #0000 150%
    ),
    radial-gradient(4px 100px at 0px 158px, rgb(255, 123, 0), #0000),
    radial-gradient(4px 100px at 300px 158px, rgb(255, 132, 0), #0000),
    radial-gradient(
      1.5px 1.5px at 150px 79px,
      rgb(255, 149, 0) 100%,
      #0000 150%
    ),
    radial-gradient(4px 100px at 0px 210px, rgb(255, 123, 0), #0000),
    radial-gradient(4px 100px at 300px 210px, rgb(255, 162, 0), #0000),
    radial-gradient(
      1.5px 1.5px at 150px 105px,
      rgb(255, 136, 0) 100%,
      #0000 150%
    );
  background-size:
    300px 235px,
    300px 235px,
    300px 235px,
    300px 252px,
    300px 252px,
    300px 252px,
    300px 150px,
    300px 150px,
    300px 150px,
    300px 253px,
    300px 253px,
    300px 253px,
    300px 204px,
    300px 204px,
    300px 204px,
    300px 134px,
    300px 134px,
    300px 134px,
    300px 179px,
    300px 179px,
    300px 179px,
    300px 299px,
    300px 299px,
    300px 299px,
    300px 215px,
    300px 215px,
    300px 215px,
    300px 281px,
    300px 281px,
    300px 281px,
    300px 158px,
    300px 158px,
    300px 158px,
    300px 210px,
    300px 210px,
    300px 210px;
  animation: hi 150s linear infinite;
}

@keyframes hi {
  0% {
    background-position:
      0px 220px,
      3px 220px,
      151.5px 337.5px,
      25px 24px,
      28px 24px,
      176.5px 150px,
      50px 16px,
      53px 16px,
      201.5px 91px,
      75px 224px,
      78px 224px,
      226.5px 350.5px,
      100px 19px,
      103px 19px,
      251.5px 121px,
      125px 120px,
      128px 120px,
      276.5px 187px,
      150px 31px,
      153px 31px,
      301.5px 120.5px,
      175px 235px,
      178px 235px,
      326.5px 384.5px,
      200px 121px,
      203px 121px,
      351.5px 228.5px,
      225px 224px,
      228px 224px,
      376.5px 364.5px,
      250px 26px,
      253px 26px,
      401.5px 105px,
      275px 75px,
      278px 75px,
      426.5px 180px;
  }
  to {
    background-position:
      0px 6800px,
      3px 6800px,
      151.5px 6917.5px,
      25px 13632px,
      28px 13632px,
      176.5px 13758px,
      50px 5416px,
      53px 5416px,
      201.5px 5491px,
      75px 17175px,
      78px 17175px,
      226.5px 17301.5px,
      100px 5119px,
      103px 5119px,
      251.5px 5221px,
      125px 8428px,
      128px 8428px,
      276.5px 8495px,
      150px 9876px,
      153px 9876px,
      301.5px 9965.5px,
      175px 13391px,
      178px 13391px,
      326.5px 13540.5px,
      200px 14741px,
      203px 14741px,
      351.5px 14848.5px,
      225px 18770px,
      228px 18770px,
      376.5px 18910.5px,
      250px 5082px,
      253px 5082px,
      401.5px 5161px,
      275px 6375px,
      278px 6375px,
      426.5px 6480px;
  }
}
</style>