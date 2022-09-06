<template>
  <img alt="Esoteric Finance logo" src="./assets/logo.png">
  <TransactionSelector msg="Welcome to the Esoteric Finance Web App"/>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import TransactionSelector from '@/components/TransactionSelector.vue';
    import 'vue-select/dist/vue-select.css';

    export default defineComponent({
        name: 'App',
        components: {
            TransactionSelector
        }
    });
</script>

<style>
    * {
        box-sizing: border-box
    }

    #app {
      font-family: Avenir, Helvetica, Arial, sans-serif;
      -webkit-font-smoothing: antialiased;
      -moz-osx-font-smoothing: grayscale;
      text-align: center;
      color: #2c3e50;
      margin-top: 60px;
    }

    .content {
        display: flex;
        justify-content: center
    }

    .sheet {
        display: table;
    }

    .sheet-r, .sheet-h {
        display: table-row;
    }

    .sheet-c {
        display: table-cell;
        padding: 5px;
        border: 1px solid gainsboro;
    }

    .sheet-c-split {
        display: flex;
    }

    .sheet-c-select {
        width: 100%;
        height: 100%;
    }

    .sheet-c-button {
        display: block;
        width: 100%;
    }
</style>
