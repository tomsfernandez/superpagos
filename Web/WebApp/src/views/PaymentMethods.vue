<template>
		<div style="margin: 1em">
			<div class="row">
                <div class="col-md-10 offset-md-1">
                    <h4>Medios de Pago</h4>    
                </div>
                <PaymentMethodTable></PaymentMethodTable>
			</div>
            <div style="margin: 1em">
                <div class="row">
                    <div class="col-md-10 offset-md-1">
                        <div class="card">
                            <PaymentButtonCreateForm style="margin: 1em"></PaymentButtonCreateForm>
                        </div>
                    </div>
                </div>
            </div>
			<div class="row" style="margin-top: 3em">
                <div class="col-md-10 offset-md-1">
                    <h4>Botones de Pago</h4>
                </div>
                <PaymentButtonTable></PaymentButtonTable>
			</div>
		</div>
</template>

<script>
	import PaymentMethodTable from "../components/PaymentMethodTable";
	import PaymentButtonTable from "../components/PaymentButtonTable";
	import PaymentButtonCreateForm from "../components/PaymentButtonCreateForm";

	export default {
		name: "PaymentMethods",
		components: {PaymentMethodTable, PaymentButtonTable, PaymentButtonCreateForm}
	}
</script>

<style scoped>

</style>