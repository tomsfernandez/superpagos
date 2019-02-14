<template>
	<div class="col-sm-6 register">
		<div class="jumbotron superpagos-register">
			<ErrorMessages :errors="errorMessages" data-cy="messages"></ErrorMessages>
			<form @submit="register">
				<div class="form-group">
					<label class="" for="nameInput">Nombre</label>
					<input class="form-control" type="text" v-model="name" id="nameInput" data-cy="name"
					       placeholder="Nombre" required>
				</div>
				<div class="form-group">
					<label class="" for="emailInput">Email</label>
					<input class="form-control" type="text" v-model="email" id="emailInput"
					       placeholder="Dirección email" data-cy="email"
					       required>
				</div>
				<div class="form-group">
					<label for="passwordInput">Contraseña</label>
					<input class="form-control" type="password" v-model="password" id="passwordInput"
					       placeholder="Contraseña"
					       data-cy="password"
					       required>
				</div>
				<button class="btn badge-primary btn-block" type="submit" data-cy="submit">Registrarse</button>
			</form>
			<br/>
			<div class="offset-sm-8 col-sm-4">
				<button class="btn badge-primary btn-block" @click="reset()">Cancelar</button>
			</div>
		</div>
	</div>
</template>

<script>
	import ErrorMessages from "./Messages";
	import * as api from "../api";

	export default {
		name: "Register",
		components: {ErrorMessages},
		props: {
			role: {
				type: String,
				default: "USER"
			}
		},
		data() {
			return {
				name: "",
				email: "",
				password: "",
				errorMessages: [],
			}
		},
		methods: {
			reset() {
				this.name = "";
				this.email = "";
				this.password = "";
			},
			register(e) {
				e.preventDefault();
				const credentials = {name: this.name, email: this.email, password: this.password, role: this.role};
				api.register(credentials)
					.then(() => this.$router.push("login"))
					.catch(e => this.errorMessages = e.response.data);
			}
		}
	}
</script>

<style scoped>
	.register {
		overflow: auto;
	}

	.superpagos-register {
		position: relative;
		top: 8%;
	}

	.form-group label {
		text-align: left;
	}
</style>