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
    providers: []
  },
  getters: {
    isAuthenticated: (state) => state.longLivedToken !== '' && state.shortLivedToken !== ''
  },
  mutations: {
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
    }
  },
  actions: {
    async login({commit}, credentials){
      const res = await api.login(credentials);
      return Promise.all([
        commit("setLongLivedToken", res.data.longLivedToken),
        commit("setShortLivedToken", res.data.shortLivedToken),
        commit("setAdmin", res.data.isAdmin)
        ]);
    },
    async renewToken({commit, getters}){
      try{
        const res = await api.renewToken(getters.longLivedToken());
        commit("setShortLivedToken", res.data.token);
      }catch (error) {
        commit("logout");
      }
    },
    async register({commit}, credentials){
      const res = await api.register(credentials);
      commit("setUser", res.data);
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
    async createProvider({commit}, provider){
      const res = await api.createProvider(provider);
      commit("addProvider", res.data);
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
    }
  }
})
