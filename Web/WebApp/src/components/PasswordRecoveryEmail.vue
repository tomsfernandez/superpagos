<template>
    <div class="col-sm-6">
        <div class="jumbotron superpagos-password-recovery">
            <Messages :success-msgs="msgs" :errors="errors" data-cy="messages"></Messages>
            <form @submit="send">
                <div class="form-group">
                    <label class="" for="emailInput">Email</label>
                    <input class="form-control" type="email" v-model="resetPasswordRequest.email" id="emailInput"
                           data-cy="email"
                           placeholder="Email para restablecer contraseña" required>
                </div>
                <button class="btn badge-primary btn-block" type="submit" data-cy="submit">Enviar</button>
                <br/>
                <div class="offset-sm-7 col-sm-5 no-padding">
                    <button class="btn badge-primary btn-block" data-cy="back-button" @click="goToLogin">Atrás
                    </button>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
    import * as api from "../api";
    import Messages from "./Messages";

    export default {
        name: "PasswordRecoveryEmail",
        components: {Messages},
        data() {
            return {
                resetPasswordRequest: {
                    email: ""
                },
                msgs: [],
                errors: []
            }
        },
        methods: {
            send: function (event) {
                event.preventDefault();
                api.sendResetPasswordRequest(this.resetPasswordRequest)
                    .then((res) => {
                        if (res.data === "")
                            this.errors.push("El email ingresado no corresponde a ninguna cuenta existente.");
                        else
                            this.msgs.push("El email ha sido enviado.");
                    });
            },
            goToLogin: function () {
                this.$router.push("/login");
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