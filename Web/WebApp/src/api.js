import axios from 'axios';

export function pollInterval(transactionId) {
  return axios.get(`Movements/state/${transactionId}`);
}

export function makePayment(payload) {
  return axios.post('Payment', payload);
}

export function createButton(button) {
	return axios.post('PaymentButtons', button);
}

export function deleteButton(id) {
	return axios.delete(`PaymentButtons/${id}`);	
}

export function getButtons() {
 	return axios.get('PaymentButtons'); 
}

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

export const getOperations = () => {
	return axios.get(`Movements`);
};

export const sendResetPasswordRequest = email => {
	return axios.post(`PasswordRecovery`, email);
};

export const recoverPassword = recoverPasswordDto => {
	return axios.post(`PasswordRecovery/reset`, recoverPasswordDto);	
};