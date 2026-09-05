<script setup>
import { DialogRoot } from 'reka-ui'
</script>

<template>
  <DialogRoot v-bind="$attrs">
    <slot />
  </DialogRoot>
</template>