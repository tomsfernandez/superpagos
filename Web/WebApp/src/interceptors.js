import store from './store/store'
import axios from 'axios'

export default function setup(){
	axios.interceptors.request.use(request => {
		const token = store.state.shortLivedToken;
		if(token) {
			request.headers.Authorization = `Bearer ${token}`;
		}
		return request;
	});

	axios.interceptors.response.use(response => {
		// do something with response data
		return response;
	},error => {
		if(error.response.status === 401 && !store.getters["isAuthenticated"]) return Promise.reject(error);
		if(error.response.status === 401 && store.state.longLivedToken){
			return store.dispatch("renewToken").then(() => axios.request(error.config));
		}else if(error.response.status === 401 && !store.state.longLivedToken){
			return store.dispatch("logout");
		}
		return Promise.reject(error);
	})
  
  if (process.env.NODE_ENV === 'production') {
    axios.defaults.baseURL = `http://34.231.138.193:8080/api`;
  }else{
    axios.defaults.baseURL = "http://localhost:5000/api";
  }
}