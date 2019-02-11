<template>
	<div class="col-sm-6">
		<div class="jumbotron superpagos-login">
			<ErrorMessages :errors="errorMessages" data-cy="errors"></ErrorMessages>
			<form @submit="login">
				<div class="form-group">
					<label class="" for="emailInput">Email</label>
					<input class="form-control" type="text" v-model="email" id="emailInput" data-cy="email" placeholder="Email">
				</div>
				<div class="form-group">
					<label for="passwordInput">Contraseña</label>
					<input class="form-control" type="password" v-model="password" id="passwordInput" data-cy="password" placeholder="Contraseña">
				</div>
				<button class="btn badge-primary btn-block" type="submit" data-cy="submit">Iniciar Sesión</button>
				<br/>
				<div class="offset-sm-7 col-sm-5 no-padding">
					<button class="btn badge-primary btn-block" data-cy="register-button" @click="goRegister">
						Nuevo usuario
					</button>
					<br/>
					<a href="#" class="float-right" @click="goToPasswordRecoveryDialog()">Recuperar contraseña</a>
				</div>
			</form>
		</div>
	</div>
</template>

<script>
	import ErrorMessages from "./ErrorMessages";

	export default {
		name: "Login",
		components: {ErrorMessages},
		data() {
			return {
				email: "",
				password: "",
				errorMessages: []
			}
		},
		methods: {
			login: function (e) {
				e.preventDefault();
				this.$store.dispatch("login", {email: this.email, password: this.password})
					.then(() => this.$router.push("home"))
					.catch(err => {
						switch(err.response.status){
							case 401:
								this.errorMessages = ["Las credenciales son inválidas"];
								break;
							default:
								break;
						}
					});
			},
			goRegister: function () {
				this.$router.push('/register');
			},
			goToPasswordRecoveryDialog: function (event) {
				event.preventDefault();
				this.$router.push('/password_reset');
			}
		}
	}
</script>

<style scoped>
	.superpagos-login {
		position: relative;
		top: 20%;
	}

	.form-group label {
		text-align: left;
	}

	.no-padding {
		padding: 0;
	}
</style>