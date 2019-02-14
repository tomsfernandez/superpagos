import Vue from 'vue';
import Vuex from 'vuex';
import * as api from '../api';
import createLogger from 'vuex/dist/logger'

Vue.use(Vuex);

const LONG_LIVED_TOKEN = "longLivedToken";
const SHORT_LIVED_TOKEN = "shortLivedToken";

const getDefaultState = () => {
  return {
    user: {},
    longLivedToken: localStorage.getItem(LONG_LIVED_TOKEN) || '',
    shortLivedToken: localStorage.getItem(SHORT_LIVED_TOKEN) || '',
    methods: [],
    isAdmin: false
  }
};

export default new Vuex.Store({
  plugins: [createLogger()],
  state: {
    user: {},
    longLivedToken: localStorage.getItem(LONG_LIVED_TOKEN) || '',
    shortLivedToken: localStorage.getItem(SHORT_LIVED_TOKEN) || '',
    methods: [],
    isAdmin: false,
    providers: [],
    buttons: [],
    transactionState: "",
    transactionId: 0,
    pollInterval: {}
  },
  getters: {
    isAuthenticated: (state) => state.longLivedToken !== '' && state.shortLivedToken !== '',
    getMethodsForPayment: (state) => state.methods.map(x => {
      return {id: x.id, name: x.name}
    }),
    isTransactionInProgress: (state) => state.transactionState === "inProgress",
    hasTransactionFinished: (state) => state.transactionState === "finished",
    hasTransactionError: (state) => state.transactionState === "error"
  },
  mutations: {
    startTransaction(state){
      state.transactionState = "inProgress";
    },
    finishTransaction(state) {
      state.transactionState = "finished";
    },
    errorTransaction(state) {
      state.transactionState = "error";
    },
    setUser(state, user){
      state.user = user;
    },
    setAdmin(state, isAdmin){
      state.isAdmin = isAdmin;
    },
    setLongLivedToken(state, token){
      localStorage.setItem(LONG_LIVED_TOKEN, token);
      state.longLivedToken = token;
    },
    setShortLivedToken(state, token){
      localStorage.setItem(SHORT_LIVED_TOKEN, token);
      state.shortLivedToken = token;
    },
    setMethods(state, methods){
      state.methods = methods;
    },
    removeMethod(state, id){
      state.methods = state.methods.filter(x => x.id !== id);
    },
    resetState(state){
      localStorage.removeItem(SHORT_LIVED_TOKEN);
      localStorage.removeItem(LONG_LIVED_TOKEN);
      Object.assign(state, getDefaultState());
    },
    setProviders(state, providers){
      state.providers = providers;
    },
    addProvider(state, provider){
      state.providers = state.providers.concat([provider]);
    },
    removeProvider(state, id){
      state.providers = state.providers.filter(x => x.id !== id);
    },
    setButtons(state, buttons){
      state.buttons = buttons
    },
    addButton(state, button){
      state.buttons = state.buttons.concat([button]);
    },
    removeButton(state, id){
      state.buttons = state.buttons.filter(x => x.id !== id);
    },
    setTransaction(state, id){
      state.transactionId = id;
    },
    setPollingInterval(state, interval){
      state.interval = interval;
    },
    stopPollingInterval(state){
      const interval = state.interval;
      state.interval = {};
      clearInterval(interval);
    }
  },
  actions: {
    async login({commit}, credentials){
      return api.login(credentials).then(res => {
        return Promise.all([
          commit("setLongLivedToken", res.data.longLivedToken),
          commit("setShortLivedToken", res.data.shortLivedToken),
          commit("setAdmin", res.data.isAdmin)
        ])
      });
    },
    async renewToken({commit, getters}){
      try{
        const res = await api.renewToken(getters.longLivedToken());
        commit("setShortLivedToken", res.data.token);
      }catch (error) {
        commit("logout");
      }
    },
    async getMethods({commit}){
      const res = await api.getMethods();
      commit("setMethods", res.data);
    },
    async logout({commit}){
      commit("resetState");
    },
    async getProviders({commit}){
      const res = await api.getProviders();
      commit("setProviders", res.data);
    },
    async getButtons({commit}){
      const res = await api.getButtons();
      commit("setButtons", res.data);
    },
    async createProvider({commit}, provider){
      const res = await api.createProvider(provider);
      commit("addProvider", res.data);
    },
    async createButton({commit}, button){
      const res = await api.createButton(button);
      commit("addButton", res.data);
    },
    // todo: algo anda mal de esto
    async deleteProvider({commit}, id){
      return api.deleteProvider(id).then(res => {
        console.log(res);
        commit("removeProvider", id);
      }).catch(e => console.log(e));
    },
    // todo: algo anda mal de esto
    async deleteMethod({commit}, id){
      return api.deleteMethod(id).then(res => {
        console.log(res);
        commit("removeMethod", id);
      }).catch(e => console.log(e));
    },
    async deleteButton({commit}, id){
      return api.deleteButton(id).then(res => {
        commit("removeButton", id);
      })
    },
    async makePayment({commit, dispatch}, payload){
      try{
        commit("startTransaction");
        const res = await api.makePayment(payload);
        commit("setTransaction", res.data);
        dispatch("pollPayment", res.data);
      }catch(e){
        console.log(e);
      }
    },
    async pollPayment({commit}, transactionId){
      try{
        const interval = setInterval(async () => {
          const {data} = await api.pollInterval(transactionId);
          if (data.failed || data.success){
            commit("stopPollingInterval");
          }
          if (data.failed) commit("errorTransaction");
          else if (data.success) commit("finishTransaction");
        }, 1000);
        commit("setPollingInterval", interval);
      }catch (e) {
        console.log(e);
      }
    }
  }
})
