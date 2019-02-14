<template>
    <div class="col-sm-6">
        <div class="jumbotron superpagos-password-recovery">
            <ErrorMessages :errors="errors" data-cy="errors"></ErrorMessages>
            <form @submit="resetPassword">
                <div class="form-group">
                    <label class="" for="passwordInput">Nueva Contraseña</label>
                    <input class="form-control" type="password" v-model="passwordRecovery.password" id="passwordInput"
                           data-cy="password"
                           placeholder="Contraseña" required>
                    <input class="form-control" type="password" v-model="passwordRecovery.confirmedPassword"
                           id="confirmedPasswordInput" data-cy="confirmedPassword" placeholder="Repetir Contraseña"
                           required>
                </div>
                <button class="btn badge-primary btn-block" type="submit" data-cy="submit">Enviar</button>
                <br/>
                <div class="offset-sm-7 col-sm-5 no-padding">
                    <button class="btn badge-primary btn-block" data-cy="register-button" @click="goToLogin">Atrás
                    </button>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
    import ErrorMessages from "./Messages";
    import * as api from "../api";

    export default {
        name: "PasswordRecovery",
        components: {ErrorMessages},
        data() {
            return {
                passwordRecovery: {
                    id: this.$route.params.userId,
                    token: this.$route.params.token,
                    password: "",
                    confirmedPassword: ""
                },
                errors: []
            }
        },
        methods: {
            resetPassword: function (event) {
                event.preventDefault();
                if (this.passwordRecovery.password === this.passwordRecovery.confirmedPassword) {
                    api.recoverPassword(this.passwordRecovery).then(() => this.goToLogin());
                } else {
                    this.errors.push("Las contraseñas no son iguales.");
                }
            },
            goToLogin: function () {
                this.$router.push("/login");
            }
        },
        watch: {
            '$route'(to) {
                this.passwordRecovery.id = to.params.userId;
                this.passwordRecovery.token = to.params.token;
            }
        }
    }
</script>

<style scoped>
    .superpagos-password-recovery {
        position: relative;
        top: 25%;
    }

    .form-group label {
        text-align: left;
    }

    .no-padding {
        padding: 0;
    }
</style>