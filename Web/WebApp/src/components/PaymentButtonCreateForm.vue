<template>
	<form @submit="submit">
		<div class="form-row">
			<div class="form-group col">
				<label for="paymentMethod">Medio de Pago</label>
                <select class="custom-select" id="paymentMethod" v-model="paymentId">
                    <option v-for="method in methods" v-bind:value="method.id">
                        {{ method.name }}
                    </option>
                </select>
				<small id="nameHelp" class="form-text text-muted">
                    Este será el medio de pago al que se le acredite el dinero
                </small>
			</div>
			<div class="form-group col">
				<label for="code">Código</label>
				<input type="text" v-model="code" class="form-control" id="code" 
                       placeholder="Ingrese un código para identificar su botón" required>
			</div>
            <div class="form-group col">
                <label for="amount">Monto</label>
                <input type="number" v-model="amount" class="form-control" id="amount"
                       placeholder="Ingrese un monto" min="1" required>
            </div>
		</div>
		<button style="float: right" type="submit" class="btn btn-primary">Submit</button>
	</form>
</template>

<script>
  import {mapActions, mapState} from "vuex";

	export default {
		name: "ProviderCreateForm",
		data(){
			return{
				paymentId: '',
				code: '',
                amount: 0
			}
		},
        computed: mapState(['methods']),
		methods: {
			submit(e){
				e.preventDefault();
				const dto = {
				  paymentMethodId: this.paymentId,
                  code: this.code,
                  amount: this.amount
                };
				this.createButton(dto);
			},
			...mapActions(["createButton"])
		}
	}
</script>

<style scoped>

</style>