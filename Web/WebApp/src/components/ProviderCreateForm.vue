<template>
	<form @submit="submit">
		<div class="form-row">
			<div class="form-group col">
				<label for="name">Nombre</label>
				<input type="text" v-model="name" class="form-control" id="name" aria-describedby="nameHelp" placeholder="Ingrese un nombre" required>
				<small id="nameHelp" class="form-text text-muted">Este nombre servirá de identificación para los usuarios.</small>
			</div>
			<div class="form-group col">
				<label for="company">Empresa</label>
				<input type="text" v-model="company" class="form-control" id="company" placeholder="Ingrese companía" required>
			</div>
			<div class="form-group col">
				<label for="name">Código</label>
				<input type="text" v-model="code" class="form-control" id="code" aria-describedby="codeHelp" placeholder="Ingrese un código" required>
				<small id="codeHelp" class="form-text text-muted">Esto supongo que servirá para algo en algún momento...</small>
			</div>
		</div>
		<div class="form-row">
			<div class="form-group col">
				<label for="pay-endpoint">Endpoint de pago</label>
				<input type="url" v-model="paymentEndpoint" class="form-control" id="pay-endpoint"
				       aria-describedby="pay-endpointHelp" placeholder="Ingrese endpoint de pago" required>
				<small id="pay-endpointHelp" class="form-text text-muted">Esta url será a la que se posteen los pagos</small>
			</div>
			<div class="form-group col">
				<label for="roll-endpoint">Endpoint de rollback</label>
				<input type="url" v-model="rollbackEndPoint" class="form-control" id="roll-endpoint" placeholder="Ingrese endpoint de rollback" required>
			</div>
			<div class="form-group col">
				<label for="link-endpoint">Endpoint de asociación</label>
				<input type="url" v-model="linkEndPoint" class="form-control" id="link-endpoint" placeholder="Ingrese endpoint de asociación" required>
			</div>
		</div>
		<button style="float: right" type="submit" class="btn btn-primary">Submit</button>
	</form>
</template>

<script>
	import {mapActions} from "vuex";

	export default {
		name: "ProviderCreateForm",
		data(){
			return{
				name: '',
				company: '',
				code: '',
				paymentEndpoint: '',
				rollbackEndPoint: '',
				linkEndPoint: ''
			}
		},
		methods: {
			submit(e){
				e.preventDefault();
				const provider = {name: this.name, company: this.company, code: this.code,
					rollbackEndPoint: this.rollbackEndPoint, paymentEndpoint: this.paymentEndpoint, linkEndPoint: this.linkEndPoint};
				this.createProvider(provider);
			},
			...mapActions(["createProvider"])
		}
	}
</script>

<style scoped>

</style>