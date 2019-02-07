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
		if(error.response.status === 401 && store.state.longLivedToken){
			return store.dispatch("renewToken").then(() => axios.request(error.config));
		}
		return store.dispatch("logout");
	})
}