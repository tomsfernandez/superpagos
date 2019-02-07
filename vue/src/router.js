import Vue from 'vue'
import Router from 'vue-router'
import Home from './views/Home.vue'
import Landing from './views/Landing'
import store from './store/store';
import Login from "./components/Login";
import Register from "./components/Register";
import OperationsTable from "./components/OperationsTable";
import Providers from "./views/Providers";
import PaymentMethods from "./views/PaymentMethods";
import PaymentMethodConfirmation from "./views/PaymentMethodConfirmation";

Vue.use(Router);

const router = new Router({
  mode: 'history',
  routes: [
    {path: '/', name: 'landing', redirect: 'login', component: Landing,
    children: [
      {path: 'login', name: 'login', component: Login},
      {path: 'register', name: 'register', component: Register},
      {path: 'adminRegister', name: 'adminRegister', component: Register, props: {role: "ADMIN"}},
    ]},
    {path: '/home', meta: {requiresAuth: true}, component: Home, children: [
      {path: '/', name: 'operations', component: OperationsTable},
      {path: 'methodRegister', name: 'methodRegister', component: PaymentMethodConfirmation},
      {path: 'methods', name: 'methods', component: PaymentMethods},
      {path: 'providers', name: 'providers', component: Providers, meta: {requiresAdmin: true}}
    ]}
  ]
});

router.beforeEach((to, from, next) => {
  const context = {to, from, next};
  const middleware = [redirectToAuthIfNotAuthenticated, checkAdmin];
  const toCall = middleware.map(middleware => middleware(context)).filter(x => x !== undefined);
  console.log(toCall);
  if(toCall.length === 0) next();
  else toCall[0]();
});

/*const checkAuth = ({to, next}) => {
  if (to.matched.some(record => record.meta.requiresAuth)) {
    const isAuthenticated = store.getters["isAuthenticated"];
    if (!isAuthenticated){
      console.log("User is not authenticated!");
      return next.bind(null, {path: '/'});
    }
  }
};*/

let entryUrl = null;

const redirectToAuthIfNotAuthenticated = ({to, next}) => {
  if (to.matched.some(record => record.meta.requiresAuth)) {
    const isAuthenticated = store.getters["isAuthenticated"];
    if (isAuthenticated && entryUrl){
      console.log("User has authenticated! Redirecting to original request...");
      const url = entryUrl;
      entryUrl = null;
      return next(url)
    }else if(!isAuthenticated){
      console.log("User is not authenticated! Redirecting to login...");
      entryUrl = to.fullPath;
      return next('/login');
    }else {
      return next();
    }
  }
};

const checkAdmin = ({to, next}) => {
  if (to.matched.some(record => record.meta.requiresAdmin)) {
    const isAdmin = store.state.isAdmin;
    if (!isAdmin){
      // todo: add better redirect
      console.log("User is not admin!");
      return next.bind(null, {path: '/'});
    }
  }
};
export default router;
