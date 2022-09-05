import { createApp } from 'vue'
import { store, key } from './store'
import App from './App.vue'
import vSelect from 'vue-select'
import Datepicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css'
import emitter from '@/plugins/mitt';

const app = createApp(App);

app.component('v-select', vSelect)
app.component('v-datepicker', Datepicker)

app.use(store, key)

// app.provide('emitter', emitter);
app.config.globalProperties.emitter = emitter;

app.mount('#app')
