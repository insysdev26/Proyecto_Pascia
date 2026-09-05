<script setup>
import { cn } from '@/lib/utils'
import { DialogTitle } from 'reka-ui'
</script>

<template>
  <DialogTitle :class="cn('text-lg font-semibold leading-none tracking-tight', $attrs.class)" v-bind="$attrs">
    <slot />
  </DialogTitle>
</template>