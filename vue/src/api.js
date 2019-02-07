
import axios from 'axios';

export const createMethod = method => {
	return axios.post('PaymentMethods', method);
};

export const login = credentials => {
	return axios.post('/login', credentials);
};

export const renewToken = token => {
	return axios.post('/renew', {}, {headers: {"Authorization": `Bearer ${token}`}});
};

export const register = credentials => {
	return axios.post('users', credentials);
};

export const getMethods = () => {
	return axios.get('PaymentMethods', {});
};

export const getProviders = () => {
	return axios.get('Providers', {});
};

export const createProvider = provider => {
	return axios.post("Providers", provider)
};

export const deleteProvider = id => {
	return axios.delete(`Providers/${id}`);
};

export const deleteMethod = id => {
	return axios.delete(`PaymentMethods/${id}`);
};