<template>
    <form @submit="submit">
        <div class="form-row">
            <div class="form-group col">
                <label for="paymentMethod">Eliga el medio de pago</label>
                <select class="custom-select" id="paymentMethod" v-model="methodId" data-cy="methods" required>
                    <option v-for="method in getMethodsForPayment" v-bind:value="method.id">
                        {{ method.name }}
                    </option>
                </select>
                <small id="methodHelp" class="form-text text-muted">
                    Este será el medio de pago al que se le acredite el dinero
                </small>
            </div>
            <div class="form-group col">
                <label for="description">Descripcion</label>
                <input type="text" v-model="description" class="form-control" id="description" data-cy="description"
                       placeholder="Ingrese companía" required>
                <small id="descriptionHelp" class="form-text text-muted">
                    Recordatorio del motivo de la transacción
                </small>
            </div>
        </div>
        <input style="float: right" :disabled="isTransactionInProgress" 
                type="submit" class="btn btn-primary" value="Submit" data-cy="submit">
    </form>
</template>

<script>
  import {mapActions, mapGetters, mapState} from "vuex";

  export default {
    name: "PaymentForm",
    data() {
      return {
        methodId: 0,
        description: ''
      }
    },
    computed: {
      ...mapGetters(['getMethodsForPayment', 'isTransactionInProgress']),
    },
    methods: {
      ...mapActions(['getMethods', 'makePayment']),
      submit(e){
        e.preventDefault();
        this.makePayment({
          buttonId: this.$route.params.id, 
          paymentMethodId: this.methodId, 
          description: this.description
        })
      }
    },
    mounted() {
      this.getMethods()
    }
  }
</script>

<style scoped>

</style>