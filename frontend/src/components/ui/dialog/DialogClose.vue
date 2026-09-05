<script setup>
import { DialogClose } from 'reka-ui'
</script>

<template>
  <DialogClose v-bind="$attrs">
    <slot />
  </DialogClose>
</template>