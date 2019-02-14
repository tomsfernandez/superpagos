<template>
    <div>
        <div class="col-md-10 offset-md-1">
            <PaymentForm style="margin: 1em"></PaymentForm>
        </div>
        <div v-show="isTransactionInProgress" style="margin-top: 10em" class="col-md-4 offset-md-4">
            <Loader :message="'Por favor espere, el pago está siendo realizado'" data-cy="loader"/>
        </div>
        <div v-show="hasTransactionFinished" style="margin-top: 10em" class="col-md-4 offset-md-4" data-cy="payment-success">
            <p>La transacción se ha realizado correctamente</p>
            <button class="btn btn-primary" @click="goHome">Volver al inicio</button>
        </div>
        <div v-show="hasTransactionError" style="margin-top: 10em"
             class="col-md-4 offset-md-4">
            <p>Ha habido un error al realizar la transferencia. Porfavor intente más tarde</p>
            <button class="btn btn-primary" @click="goHome">Volver al inicio</button>
        </div>
    </div>
</template>

<script>
  import PaymentForm from "../components/PaymentForm"
  import Loader from "../components/Loader";
  import {mapGetters} from "vuex";

  export default {
    name: "Payment",
    components: {Loader, PaymentForm},
    computed: {
      ...mapGetters(['isTransactionInProgress', 'hasTransactionFinished', 'hasTransactionError'])
    },
    methods: {
      goHome() {
        this.$router.push('/home')
      }
    }
  }
</script>

