import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store/store'
import axios from 'axios'
import interceptorSetup from './interceptors';

import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import '@progress/kendo-ui';
import '@progress/kendo-theme-default/dist/all.css'

import { Grid, GridInstaller } from '@progress/kendo-grid-vue-wrapper'
import { DataSourceInstaller } from "@progress/kendo-datasource-vue-wrapper";

Vue.use(GridInstaller);
Vue.use(DataSourceInstaller);

axios.defaults.baseURL = "http://localhost:5000/api";
Vue.config.productionTip = false;
interceptorSetup();

new Vue({
  router,
  store,
  components: {
    Grid
  },
  render: h => h(App)
}).$mount('#app');

if (window.Cypress) {
  console.log("Vuex store is included in test suite...");
  window.appStore = store
}