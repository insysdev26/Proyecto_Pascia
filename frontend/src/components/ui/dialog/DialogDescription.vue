<script setup>
import { cn } from '@/lib/utils'
import { DialogDescription } from 'reka-ui'
</script>

<template>
  <DialogDescription :class="cn('text-sm text-slate-400', $attrs.class)" v-bind="$attrs">
    <slot />
  </DialogDescription>
</template>