<script setup>
import { cn } from '@/lib/utils'

const props = defineProps({
  for: { type: String, default: undefined },
  class: { type: String, default: '' }
})
</script>

<template>
  <label
    :for="props.for"
    :class="
      cn(
        'text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70 text-slate-200',
        props.class
      )
    "
  >
    <slot />
  </label>
</template>