<script setup>
import { cn } from '@/lib/utils'
import { DialogContent, DialogOverlay, DialogPortal, DialogClose } from 'reka-ui'
import { X } from 'lucide-vue-next'
</script>

<template>
  <DialogPortal>
    <DialogOverlay class="fixed inset-0 z-50 grid place-items-center overflow-y-auto bg-black/80 sm:py-10">
      <DialogContent
        v-bind="$attrs"
        :class="cn('relative z-50 grid w-full max-w-lg gap-4 border border-slate-800 bg-slate-950 p-6 shadow-lg sm:rounded-lg text-white', $attrs.class)"
      >
        <slot />
        <DialogClose class="absolute right-4 top-4 rounded-sm opacity-70 transition-opacity hover:opacity-100 focus:outline-none">
          <X class="w-4 h-4 text-slate-400" />
          <span class="sr-only">Cerrar</span>
        </DialogClose>
      </DialogContent>
    </DialogOverlay>
  </DialogPortal>
</template>