<script setup>
import { cn } from '@/lib/utils'
</script>

<template>
  <div :class="cn('flex flex-col gap-y-1.5 text-center sm:text-left', $attrs.class)" v-bind="$attrs">
    <slot />
  </div>
</template>